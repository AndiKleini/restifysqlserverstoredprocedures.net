# Create REST interface for SQL Server Stored Procedures
Transfers a REST url into a SQL Server Stored Procedure call where parsing logic and DB access are located in a seperate nuget Package 
Restfy3SP, supporting reusability for other scenarios as well. Additionally the repository includes an ASP.Net REST API as example application. 
## Why would you need this ?
Sometimes it is easier, or maybe the only possible option, to access Stored Procedures by a proper REST interface, rather than directly establishing a database connection. Typical scenarios where one would like to follow this approach are summarized below:
* Execute permissions are restricted to technical service users only 
* Avoid establishing direct connection from the client to SQL Server
* Rapid development for integrating database calls in your application
* Apply middleware (e.g.: output-caching, audit tracing, authentication, authorization) to database interface 
* Circumvent direct technological dependencies between client and SQL Server
## Running the VS project ?
Assuming you want to expose some stored procedure, simply running a query against a table, by a REST interface:
```SQL
#TODO: create test table here

CREATE PROCEDURE testSchema.testProcedure  
    @firstParameterVarChar nvarchar(50),   
    @secondParameterInteger int,
    @thirdParameterGuid uniqueidentifier
AS   
    Do something
    ... 
GO  
```
In order to see it really working, we first populate testSchema.testTable with some test data:
```SQL
insert test data
```
By configuring proper connection string in appsettings json:
```JSON
// TODO configure connection strinh here
```
you can expose it under following REST url (method get):
```html
http://myserver/dbname/testSchema/testProcedure/@firstParameter='somestring',@secondParameter=23,@thirdParameter=42e67646-7863-4498-9221-b38e31ebf9ed
```
According to the test data we inserted above, JSON below is delivered:
```JSON
```
# Running as docker container ?
// TODO: add Docker file to project
If you don't want/can run it as VS project, you simply can use the added docker image.
# Restrictions (or maybe motivations for future releases) 
Lets summarize yet unsupported features. 
* User defined input types 
Depending on feedback those can be implemented

