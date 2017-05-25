using System;
using System.Net;
using System.Text;
using System.Threading;
using Apprenda.SaaSGrid.Addons;
using Apprenda.Testing;
using RabbitMQ.Client;
using RestSharp;
using RestSharp.Authenticators;
using Apprenda.Services.Logging;

namespace RabbitMQAddOn
{
    public class RabbitMQAddOn : AddonBase
    {
        private static readonly ILogger log = LogManager.Instance().GetLogger(typeof(RabbitMQAddOn));
        public override ProvisionAddOnResult Provision(AddonProvisionRequest request)
        {            
            var rabbitConfig = new RabbitMQConfig(request);            
            var client = new RestClient(rabbitConfig.ManagementUri);
            client.Authenticator = new HttpBasicAuthenticator(rabbitConfig.AdminUser, rabbitConfig.AdminPassword);

            // Create new user
            var user = request.Manifest.CallingDeveloperAlias + "_" + request.Manifest.InstanceAlias;
            var pass = Guid.NewGuid().ToString("N");

            var createUserPath = $"users/{user}";
            var createUserBody = new {password = pass, tags = "monitoring,management,administrator"};

            var createUserRequest = new RestRequest(createUserPath, Method.PUT) {RequestFormat = DataFormat.Json};
            createUserRequest.AddBody(createUserBody);
            createUserRequest.AddHeader("content-type", "application/json");

            log.DebugFormat("The following user is being created: {0} using the following URI: ", user,createUserPath.ToString());
            var createUserResponse = client.Execute(createUserRequest);
            log.DebugFormat("The response code for the user creation REST call is: {0}", createUserResponse.StatusCode);
            if (createUserResponse.StatusCode != HttpStatusCode.Created)
            {
                return ProvisionAddOnResult.Failure("Unable to create user");
            }

            Thread.Sleep(5000);
            // Create new vhost
            var vhostName = request.Manifest.CallingDeveloperAlias + "_" + request.Manifest.InstanceAlias;
            var createVhostPath = $"vhosts/{vhostName}";
            var createVhostRequest = new RestRequest(createVhostPath, Method.PUT) { RequestFormat = DataFormat.Json };
            createVhostRequest.AddHeader("content-type", "application/json");

            log.DebugFormat("The following vhost is being created: {0} using the following URI:", vhostName, createVhostPath.ToString());
            var createVhostResponse = client.Execute(createVhostRequest);
            log.DebugFormat("The response code for the vhost creationg REST call is: {0}", createVhostResponse.StatusCode);
            if (createVhostResponse.StatusCode != HttpStatusCode.Created)
            {
                //If this fails, we need to clean up the previous command

                // Delete user
                var deleteUserPath = $"users/{user}";

                var deleteUserRequest = new RestRequest(deleteUserPath, Method.DELETE) { RequestFormat = DataFormat.Json };
                deleteUserRequest.AddHeader("content-type", "application/json");

                var deleteUserResponse = client.Execute(deleteUserRequest);

                return ProvisionAddOnResult.Failure("Unable to create vhost");
            }

            Thread.Sleep(5000);
            // Set permissions
            var permissionsPath = $"permissions/{vhostName}/{user}";
            var permissionsBody = new {configure = ".*",write=".*",read=".*"};
            var permissionsRequest = new RestRequest(permissionsPath, Method.PUT) {RequestFormat = DataFormat.Json};
            permissionsRequest.AddHeader("content-type", "application/json");
            permissionsRequest.AddBody(permissionsBody);

            log.DebugFormat("The permissions are being configuring using the following URI: {0}", permissionsPath);
            var permissionsResponse = client.Execute(permissionsRequest);
            log.DebugFormat("The response code for configuring the permissions is: {0}", permissionsResponse.StatusCode);
            if (permissionsResponse.StatusCode != HttpStatusCode.Created)
            {
                //If this fails, we need to clean up the previous command

                // Delete user
                var deleteUserPath = $"users/{user}";

                var deleteUserRequest = new RestRequest(deleteUserPath, Method.DELETE) { RequestFormat = DataFormat.Json };
                deleteUserRequest.AddHeader("content-type", "application/json");

                var deleteUserResponse = client.Execute(deleteUserRequest);

                // Delete Vhost
                var deleteVhostPath = $"vhosts/{vhostName}";
                var deleteVhostRequest = new RestRequest(deleteVhostPath, Method.DELETE) { RequestFormat = DataFormat.Json };
                deleteVhostRequest.AddHeader("content-type", "application/json");

                var deleteVhostResponse = client.Execute(deleteVhostRequest);

                return ProvisionAddOnResult.Failure("Unable to set permissions");
            }

            var connectionString = $"amqp://{user}:{pass}@{rabbitConfig.Hostname}:{rabbitConfig.Port}/{vhostName}";

            return ProvisionAddOnResult.Success(connectionString);
        }

        public override OperationResult Deprovision(AddonDeprovisionRequest request)
        {
            var rabbitConfig = new RabbitMQConfig(request);
            var client = new RestClient(rabbitConfig.ManagementUri);
            client.Authenticator = new HttpBasicAuthenticator(rabbitConfig.AdminUser, rabbitConfig.AdminPassword);

            // Delete vhost
            var vhostName = request.Manifest.CallingDeveloperAlias + "_" + request.Manifest.InstanceAlias;
            var deleteVhostPath = $"vhosts/{vhostName}";
            var deleteVhostRequest = new RestRequest(deleteVhostPath, Method.DELETE) { RequestFormat = DataFormat.Json };
            deleteVhostRequest.AddHeader("content-type", "application/json");

            var deleteVhostResponse = client.Execute(deleteVhostRequest);
            if (deleteVhostResponse.ResponseStatus != ResponseStatus.Completed)
            {
                return ProvisionAddOnResult.Failure("Unable to delete vhost");
            }

            // Delete user
            var user = request.Manifest.CallingDeveloperAlias + "_" + request.Manifest.InstanceAlias;

            var deleteUserPath = $"users/{user}";            

            var deleteUserRequest = new RestRequest(deleteUserPath, Method.DELETE) { RequestFormat = DataFormat.Json };
            deleteUserRequest.AddHeader("content-type", "application/json");

            var deleteUserResponse = client.Execute(deleteUserRequest);
            if (deleteUserResponse.ResponseStatus != ResponseStatus.Completed)
            {
                return ProvisionAddOnResult.Failure("Unable to delete user");
            }
            
            return ProvisionAddOnResult.Success("RabbitMQ Addon Instance Deprovisioned Successfully");
        }

        public override OperationResult Test(AddonTestRequest request)
        {
            var rabbitConfig = new RabbitMQConfig(request);

            var factory = new ConnectionFactory()
            {
                HostName = rabbitConfig.Hostname, UserName = rabbitConfig.AdminUser, Password = rabbitConfig.AdminPassword, Port = rabbitConfig.Port, RequestedConnectionTimeout = 20000
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {                    

                    channel.QueueDeclare(queue: "AddOnTestQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string sendMessage = "Hello from Test method!";
                    var sendBody = Encoding.UTF8.GetBytes(sendMessage);

                    channel.BasicPublish(exchange: "",
                        routingKey: "AddOnTestQueue",
                        basicProperties: null,
                        body: sendBody);
                    Console.WriteLine(" [x] Sent {0}", sendMessage);                    

                    Thread.Sleep(100);
                    
                    var result = channel.BasicGet(queue: "AddOnTestQueue",noAck: false);
                    var recvBody = result.Body;
                    channel.BasicAck(result.DeliveryTag,false);
                    var recvMessage = Encoding.UTF8.GetString(recvBody);
                    Console.WriteLine(" [x] Received {0}", recvMessage);

                    Assert.AreEqual(sendBody, recvBody);
                    Assert.AreEqual(sendMessage, recvMessage);


                    return new OperationResult() {IsSuccess = true, EndUserMessage = ""};

                }
            }
        }
    }
}
