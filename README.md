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
## Meet the interface of Restify3SP
Once you have installed the Restify3SP nuget package, you have to create a proper DataBaseAccess instance by passing your connection string:
```C#
 var access = new DatabaseAccess("Data Source={YourServerName};Initial Catalog=restifysp; Integrated Security=true;");
```
Next you can pass a stored procedure call specified by schema.procedurename @parameter1 = 123, @parameter2 = somename, @parameter3 = 0 out, where argument specifies the whole parameter signature (in this case: '@parameter1 = 123, @parameter2 = somename, @parameter3 = 0 out').
```C#
string result = await access.ExecuteWithParameter(schema + "." + procedurename, arguments);
```
The interface is optimized for transforming REST urls to stored procedure executions.
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
    @firstParameterInt int,
    @secondParameterVarChar nvarchar(50),   
    @thirdParameterStartDate date,
    @forthParameterEndDate date,
    @fifthParameterGuid uniqueidentifier,
    @sixtParameterInt int out
AS   
	select * from testSchema.testTable where id = @firstParameterInt;
	select * from testSchema.testTable where name = @secondParameterVarChar;
	select * from testSchema.testTable where creationDate between @thirdParameterStartDate and @forthParameterEndDate
	select * from testSchema.testTable where globalId =  @fifthParameterGuid;
	select * from testSchema.testTable where globalId = @fifthParameterGuid;
	set @sixtParameterInt = 23;
	return 13;
GO  
```
In order to see it really working, we first populate testSchema.testTable with some test data:
```SQL
insert into restifysp.testSchema.testTable values ('Wolfgang', '2020-06-27 07:36:45.000', '236CD99E-3316-43CE-9210-7A66588CEB62')
insert into restifysp.testSchema.testTable values ('Petra', '2020-06-28 08:22:17.000', 'E993ED58-1FDB-493C-A1D6-5310ECEFE0DC')
insert into restifysp.testSchema.testTable values ('Denis', '2020-06-29 10:04:12.000', '1075E085-30D4-4C0D-9F1E-99231D9342D0')
insert into restifysp.testSchema.testTable values ('Robert', '2020-06-29 15:24:42.000', '91B608E1-BF4A-4240-A76C-1C1696B4F955')
insert into restifysp.testSchema.testTable values ('Andrea', '2020-06-30 09:16:12.000', '21A941EA-0599-46AF-970C-B99D899170ED')
```
By configuring proper connection string (in this example integrated security is used) in appsettings json, which is placed directly in project root. You should also restrict your urls to a single database in order to avoid any confusions. This has to be done by specifying the database name in connection string as Catalog and in appsettings property DateBaseName :
```JSON
...
"ConnectionStrings": {
    "Db": "Data Source={YourServerName};Initial Catalog={YourDateBaseName}; Integrated Security=true;"
  }, 
  "providerName=\"System.Data.SqlClient\"\",": null,
  "DataBaseName":  "{YourDateBaseName}"
...
```
you can expose it under following REST url (method get):
```html
https://localhost:44399/restifysp/testSchema/testProcedure/@firstParameterInt=2, @secondParameterVarChar=Unknown,@thirdParameterStartDate=2020-06-27 05:36:45.00',@forthParameterEndDate=2020-06-29 15:24:42.000,@fifthParameterGuid=21A941EA-0599-46AF-970C-B99D899170ED,@sixtParameterInt=0 out
```
According to the test data we inserted above, JSON below is delivered. As you can figure out from the response, output parameters and return values are appended at the end of the result. Empty resulsets are emitted as empty JSON arrays.
```JSON
{
   "Result":[
      [
         {
            "id":2,
            "name":"Petra",
            "creationDate":"2020-06-28T00:00:00",
            "globalId":"e993ed58-1fdb-493c-a1d6-5310ecefe0dc"
         }
      ],
      [

      ],
      [
         {
            "id":1,
            "name":"Wolfgang",
            "creationDate":"2020-06-27T00:00:00",
            "globalId":"236cd99e-3316-43ce-9210-7a66588ceb62"
         },
         {
            "id":2,
            "name":"Petra",
            "creationDate":"2020-06-28T00:00:00",
            "globalId":"e993ed58-1fdb-493c-a1d6-5310ecefe0dc"
         },
         {
            "id":3,
            "name":"Denis",
            "creationDate":"2020-06-29T00:00:00",
            "globalId":"1075e085-30d4-4c0d-9f1e-99231d9342d0"
         },
         {
            "id":4,
            "name":"Robert",
            "creationDate":"2020-06-29T00:00:00",
            "globalId":"91b608e1-bf4a-4240-a76c-1c1696b4f955"
         }
      ],
      [
         {
            "id":5,
            "name":"Andrea",
            "creationDate":"2020-06-30T00:00:00",
            "globalId":"21a941ea-0599-46af-970c-b99d899170ed"
         }
      ],
      [
         {
            "id":5,
            "name":"Andrea",
            "creationDate":"2020-06-30T00:00:00",
            "globalId":"21a941ea-0599-46af-970c-b99d899170ed"
         }
      ]
   ],
   "OutputParameter":{
      "sixtParameterInt":"23"
   },
   "Return":13
}
```
# Running as docker container ?
If you don't want/can run it as VS project, you simply can use the added docker image. Please be aware that port mapping in docker run is required:
```Command
docker build . -t restify
docker run -p 80:80 restify
```
# Restrictions (or maybe motivations for future releases) 
Lets summarize yet unsupported features. 
* User defined input types 
Depending on feedback those can be implemented



