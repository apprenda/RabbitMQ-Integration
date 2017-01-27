using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apprenda.SaaSGrid.Addons;

namespace RabbitMQAddOn
{
    public class RabbitMQConfig
    {
        public string Hostname { get; set; }
        public int Port { get; set; }

        public Uri ManagementUri
        {
            get
            {
                var builder = new UriBuilder();
                builder.Scheme = "http";
                builder.Host = Hostname;
                builder.Port = 15672; //TODO: Move to congig parameters to allow for easy configuration
                builder.Path = "api";
                return builder.Uri;
            }
        }
        public string AdminUser { get; set; }
        public string AdminPassword { get; set; }

        private string RabbitAdminEndpointKey = "RabbitAdminEndpoint";
        private string RabbitAdminEndpointPortKey = "RabbitAdminEndpointPort";
        private string RabbitAdminUserKey = "RabbitAdminUser";
        private string RabbitAdminPasswordKey = "RabbitAdminPassword";

        public RabbitMQConfig(AddonProvisionRequest request)
        {
            Hostname = request.Manifest.Properties.Single(p => p.Key == RabbitAdminEndpointKey).Value;
            var portString = request.Manifest.Properties.Single(p => p.Key == RabbitAdminEndpointPortKey).Value;
            Port = int.Parse(portString);
            AdminUser = request.Manifest.Properties.Single(p => p.Key == RabbitAdminUserKey).Value;
            AdminPassword = request.Manifest.Properties.Single(p => p.Key == RabbitAdminPasswordKey).Value;
        }

        public RabbitMQConfig(AddonTestRequest request)
        {
            Hostname = request.Manifest.Properties.Single(p => p.Key == RabbitAdminEndpointKey).Value;
            var portString = request.Manifest.Properties.Single(p => p.Key == RabbitAdminEndpointPortKey).Value;
            Port = int.Parse(portString);
            AdminUser = request.Manifest.Properties.Single(p => p.Key == RabbitAdminUserKey).Value;
            AdminPassword = request.Manifest.Properties.Single(p => p.Key == RabbitAdminPasswordKey).Value;
        }

        public RabbitMQConfig(AddonDeprovisionRequest request)
        {
            Hostname = request.Manifest.Properties.Single(p => p.Key == RabbitAdminEndpointKey).Value;
            var portString = request.Manifest.Properties.Single(p => p.Key == RabbitAdminEndpointPortKey).Value;
            Port = int.Parse(portString);
            AdminUser = request.Manifest.Properties.Single(p => p.Key == RabbitAdminUserKey).Value;
            AdminPassword = request.Manifest.Properties.Single(p => p.Key == RabbitAdminPasswordKey).Value;
        }




    }
}
