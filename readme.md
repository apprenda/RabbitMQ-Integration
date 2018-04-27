# Apprenda-RabbitMQ
This is the repository that includes all the details to integrate Apprenda with RabbitMQ. Using this AddOn, Developers will be able to provision and deprovision RabbitMQ queues for use in their Apprenda applications.

## Code Repository
- source/RabbitMQAddOn, the complete source code for the implementation of the AddOn

## Integration Steps, Setting up the Apprenda Add-On in the Apprenda Operator Portal

### Configuring Your RabbitMQ Instance ###
- This add-on assumes you have a RabbitMQ instance already running. 

### Configure the Add-On ###
- Use the provided packaged Add-On, [found here](https://github.com/apprenda/RabbitMQ-Integration/releases) to upload the Add-On to the Apprenda Operator Portal (SOC). You can alternatively build or enhance the provided Visual Studio solution file to create an Add-On that meets your needs.
- Once the Add-On has uploaded, click on edit to provide the required configuration. 
- Click on the Configuration tab and fill out the various properties. *NOTE*: Your installation might have different ports configured. 


### Using the Add-On ###
- Navigate to your developer portal. On the left, click on Add-Ons
- You should see your Add-On here. Click on it, and click the '+' symbol to provision a new queue and virtual host for your applications. A new set of credentials will automatically be generated. 
- In Instance Alias enter a unique alias for your new instance
- The Add-On will connect to RabbitMQ and create a queue, virtual host and user. 
- You can token switch the Add-On instance into your app byt using the displayed token in the Developer Portal
- If you wish to delete your created resources, simply deprovision the Add-On. 

**Congratulations, you have just integrated the Apprenda Cloud Platform with RabbitMQ**
- You can learn more about Add-Ons at http://docs.apprenda.com/9-0/addons
