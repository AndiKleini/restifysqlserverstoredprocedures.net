# Create REST interface for SQL Server Stored Procedures
Transfers a REST url into a SQL Server Stored Procedure call where parsing logic and DB access are located in a seperate nuget Package 
Restfy3SP, supporting reusability for other scenarios as well. Additionally the repository includes an ASP.Net REST API as example application. 
## Meet the problem
Sometimes it is easier, or maybe the only possible option, to access Stored Procedures by a proper REST interface, rather than directly establishing a database connection. Typical scenarios where one would like to follow this approach are summarized below:
* Execute permissions are restricted to technical service users only
* Avoid establishing direct connection from the client to SQL Server
* Rapid development for integrating database calls in your application
* Apply middleware (e.g.: output-caching, audit tracing, authentication, authorization) to database interface 
* Resolve technological dependencies between client and SQL Server
