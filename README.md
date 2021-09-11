# YellowPages


## You should change database connection strings and RabbitMQ connection information

For YellowPages.ApiPerson and YellowPages.ApiReport

### FileNames : appsettings.Development.json and appsettings.Production.json

	"ConnectionStrings": {
		"DefaultConnection": "Server=SERVERNAME;Port=5432;Database=YellowPages;UserId=USERNAME;Password=PASSWORD"
	}
	
### For YellowPages.Client.ReportConsumer and YellowPages.Client.ReportPublisher

	"ConnectionStrings": {
		"DefaultConnection": "Server=SERVERNAME;Port=5432;Database=YellowPages;UserId=USERNAME;Password=PASSWORD"
	},
	"MessageBusConfiguration": {
		"Hostname": "RabbitMQHOST",
		"Username": "RabbitMQUsername",
		"Password": "RabbitMQPassword",
		"ParallelThreadsCount": 3
	},
	
### After you changed the database information you should init your database using YellowPages.Database project

	PM> enable-migrations
	PM> add-migration initial
	PM> update-database
