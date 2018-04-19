# Apprenda-RabbitMQ
This is the repository that includes all the details to integrate Apprenda with RabbitMQ. Using this AddOn, Developers will be able to provision and deprovision RabbitMQ for use in their Apprenda applications.

## Code Repository
- source/RabbitMQAddOn, the complete source code for the implementation of the AddOn

## Integration Steps, Setting up the Apprenda Add-On in the Apprenda Operator Portal

### Configuring Your RabbitMQ Instance ###
- This add-on assumes you have a RabbitMQ instance running. 

### Configure the Add-On ###
- Use the provided packaged Add-On, ??? , [found here](https://github.com/apprenda/RabbitMQ-Integration/releases) to upload the Add-On to the Apprenda SOC (aka Operator Portal). You can alternatively build or enhance the provided Visual Studio solution file to create an Add-On that meets your needs.
- Once the Add-On has uploaded, click on edit. 
- Fill in rest of the steps ??

### Using the Add-On ###
- Navigate to your developer portal. On the left, click on Add-Ons
- You should see your Add-On here. Click on it, and click the '+' symbol to provision a new database and user
- In Instance Alias enter a unique alias for your new instance
- fill in details ??
- Once provisioning has complete you can verify the existence of your instance 
- You can token switch the Add-On instance into your app 
- When you deprovision this instance, what will happen???

**Congratulations, you have just integrated the Apprenda Cloud Platform with the RabbitMQ Add-On**
- You can learn more about Add-Ons at http://docs.apprenda.com/9-0/addons
