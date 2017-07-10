FROM microsoft/aspnetcore-build:1.0-1.1
#ENV pfdatabasetype mssql2017
#ENV pfdbhost 127.0.0.1
#ENV pfmsdbinstance DEV2014
#ENV pfdbname pyprflowdb

#ENV pfdbport 2250
#ENV pfdbid sa
#ENV pfdbpw !!nimda1

#ENV pfdbconnectionstring Server=10.0.0.25;Database=pyprflowDb;User Id=sa;Password=!!Nimda1;

COPY ./ /app
WORKDIR /app/pyprflow.Api
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "run"]
