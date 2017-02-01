using System;
using System.Linq;
using Apprenda.SaaSGrid.Addons;

namespace RabbitMQAddOn
{
    public class RabbitMQConfig
    {
        public string Hostname { get; set; }
        public int AdminPort { get; set; }
        public int Port { get; set; }

        public Uri ManagementUri
        {
            get
            {
                var builder = new UriBuilder
                {
                    Scheme = "http",
                    Host = Hostname,
                    Port = AdminPort,
                    Path = "api"
                };
                return builder.Uri;
            }
        }
        public string AdminUser { get; set; }
        public string AdminPassword { get; set; }

        private string RabbitEndpointKey = "RabbitEndpoint";
        private string RabbitAdminPortKey = "RabbitAdminPort";
        private string RabbitPortKey = "RabbitPort";
        private string RabbitAdminUserKey = "RabbitAdminUser";
        private string RabbitAdminPasswordKey = "RabbitAdminPassword";

        public RabbitMQConfig(AddonProvisionRequest request)
        {
            Hostname = request.Manifest.Properties.Single(p => p.Key == RabbitEndpointKey).Value;
            var portString = request.Manifest.Properties.Single(p => p.Key == RabbitPortKey).Value;
            Port = int.Parse(portString);
            var adminPortString = request.Manifest.Properties.Single(p => p.Key == RabbitAdminPortKey).Value;
            AdminPort = int.Parse(adminPortString);
            AdminUser = request.Manifest.Properties.Single(p => p.Key == RabbitAdminUserKey).Value;
            AdminPassword = request.Manifest.Properties.Single(p => p.Key == RabbitAdminPasswordKey).Value;
        }

        public RabbitMQConfig(AddonTestRequest request)
        {
            Hostname = request.Manifest.Properties.Single(p => p.Key == RabbitEndpointKey).Value;
            var portString = request.Manifest.Properties.Single(p => p.Key == RabbitPortKey).Value;
            Port = int.Parse(portString);
            var adminPortString = request.Manifest.Properties.Single(p => p.Key == RabbitAdminPortKey).Value;
            AdminPort = int.Parse(adminPortString);
            AdminUser = request.Manifest.Properties.Single(p => p.Key == RabbitAdminUserKey).Value;
            AdminPassword = request.Manifest.Properties.Single(p => p.Key == RabbitAdminPasswordKey).Value;
        }

        public RabbitMQConfig(AddonDeprovisionRequest request)
        {
            Hostname = request.Manifest.Properties.Single(p => p.Key == RabbitEndpointKey).Value;
            var portString = request.Manifest.Properties.Single(p => p.Key == RabbitPortKey).Value;
            Port = int.Parse(portString);
            var adminPortString = request.Manifest.Properties.Single(p => p.Key == RabbitAdminPortKey).Value;
            AdminPort = int.Parse(adminPortString);
            AdminUser = request.Manifest.Properties.Single(p => p.Key == RabbitAdminUserKey).Value;
            AdminPassword = request.Manifest.Properties.Single(p => p.Key == RabbitAdminPasswordKey).Value;
        }




    }
}
