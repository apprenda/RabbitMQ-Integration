using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Apprenda.SaaSGrid.Addons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQAddOn;

namespace AddOnTestProject
{
    [TestClass]
    public class AddOnTests
    {
        [TestMethod]
        public void TestTestMethod()
        {
            var testRequest = new AddonTestRequest();

            var properties = new List<AddonProperty>();

            properties.Add(new AddonProperty() { Key = "RabbitEndpoint", Value = "hqs-engineer02.apprendalabs.local" });
            properties.Add(new AddonProperty() { Key = "RabbitAdminPort", Value = "15672" });
            properties.Add(new AddonProperty() { Key = "RabbitPort", Value = "5672" });
            properties.Add(new AddonProperty() { Key = "RabbitAdminUser", Value = "addonuser" });
            properties.Add(new AddonProperty() { Key = "RabbitAdminPassword", Value = "addonuser" });
            properties.Add(new AddonProperty() { Key = "DeveloperAlias", Value = "unit" });
            properties.Add(new AddonProperty() { Key = "InstanceAlias", Value = "sampleId" });

            testRequest.Manifest = ReadTestManifest("UnitTestAddOnManifest.xml");
            testRequest.Manifest.Properties = properties;

            var addon = new RabbitMQAddOn.RabbitMQAddOn();
            var result = addon.Test(testRequest);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestProvisioningMethods()
        {
            var request = new AddonProvisionRequest();
            var properties = new List<AddonProperty>();

            properties.Add(new AddonProperty() { Key = "RabbitEndpoint", Value = "hqs-engineer02.apprendalabs.local" });
            properties.Add(new AddonProperty() { Key = "RabbitAdminPort", Value = "15672" });
            properties.Add(new AddonProperty() { Key = "RabbitPort", Value = "5672" });
            properties.Add(new AddonProperty() { Key = "RabbitAdminUser", Value = "addonuser" });
            properties.Add(new AddonProperty() { Key = "RabbitAdminPassword", Value = "addonuser" });
            properties.Add(new AddonProperty() { Key = "DeveloperAlias", Value = "unit" });
            properties.Add(new AddonProperty() { Key = "InstanceAlias", Value = "sampleId" });

            request.Manifest = ReadTestManifest("UnitTestAddOnManifest.xml");
            request.Manifest.Properties = properties;

            var addon = new RabbitMQAddOn.RabbitMQAddOn();
            var result = addon.Provision(request);

            Assert.IsTrue(result.IsSuccess);

            var deprovisionRequest = new AddonDeprovisionRequest();
            deprovisionRequest.Manifest = ReadTestManifest("UnitTestAddOnManifest.xml");
            deprovisionRequest.Manifest.Properties = properties;

            var deprovisionResult = addon.Deprovision(deprovisionRequest);

            Assert.IsTrue(deprovisionResult.IsSuccess);


        }

        private AddonManifest ReadTestManifest(string path)
        {
            var xml = File.ReadAllText(path);
            var sr = new StringReader(xml);
            var xmlReader = new XmlTextReader(sr);
            var serializer = new XmlSerializer(typeof (AddonManifest));            
            return serializer.Deserialize(xmlReader) as AddonManifest;
        }

    }
}
