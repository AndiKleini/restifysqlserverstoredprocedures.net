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
CREATE SCHEMA testSchema
GO

CREATE TABLE restifysp.testSchema.testTable (
    id int Identity(1,1) PRIMARY KEY,
    name nvarchar(100),
    creationDate date,
	globalId uniqueidentifier
);
Go

CREATE PROCEDURE testSchema.testProcedure  
    @firstParameterVarChar nvarchar(50),   
    @secondParameterInteger int,
    @thirdParameterDate date,
    @forthParameterGuid uniqueidentifier
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
https://localhost:44399/restifysp/testSchema/testProcedure/@firstParameterInt=2, @secondParameterVarChar='Denis',@thirdParameterStartDate=2020-06-27 05:36:45.00',@forthParameterEndDate=2020-06-29 15:24:42.000,@fifthParameterGuid=21A941EA-0599-46AF-970C-B99D899170ED,@sixtParameterInt=0 out
```
According to the test data we inserted above, JSON below is delivered:
```JSON
{"Result":[[{"id":2,"name":"Petra","creationDate":"2020-06-28T00:00:00","globalId":"e993ed58-1fdb-493c-a1d6-5310ecefe0dc"}],[],[{"id":1,"name":"Wolfgang","creationDate":"2020-06-27T00:00:00","globalId":"236cd99e-3316-43ce-9210-7a66588ceb62"},{"id":2,"name":"Petra","creationDate":"2020-06-28T00:00:00","globalId":"e993ed58-1fdb-493c-a1d6-5310ecefe0dc"},{"id":3,"name":"Denis","creationDate":"2020-06-29T00:00:00","globalId":"1075e085-30d4-4c0d-9f1e-99231d9342d0"},{"id":4,"name":"Robert","creationDate":"2020-06-29T00:00:00","globalId":"91b608e1-bf4a-4240-a76c-1c1696b4f955"}],[{"id":5,"name":"Andrea","creationDate":"2020-06-30T00:00:00","globalId":"21a941ea-0599-46af-970c-b99d899170ed"}],[{"id":5,"name":"Andrea","creationDate":"2020-06-30T00:00:00","globalId":"21a941ea-0599-46af-970c-b99d899170ed"}]],"OutputParameter":{"sixtParameterInt":"23"},"Return":0}
```
# Running as docker container ?
// TODO: add Docker file to project
If you don't want/can run it as VS project, you simply can use the added docker image.
# Restrictions (or maybe motivations for future releases) 
Lets summarize yet unsupported features. 
* User defined input types 
Depending on feedback those can be implemented



