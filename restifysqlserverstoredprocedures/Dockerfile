FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443

# RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf && \
#    sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf && \
#    sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf && \
#    sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf
    

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY restifysqlserverstoredprocedures.csproj restifysqlserverstoredprocedures.csproj
COPY nuget.config nuget.config
RUN dotnet restore "restifysqlserverstoredprocedures.csproj"
COPY . .
#WORKDIR "/src/restifysqlserverstoredprocedures"
RUN dotnet build "restifysqlserverstoredprocedures.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "restifysqlserverstoredprocedures.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "restifysqlserverstoredprocedures.dll"]
