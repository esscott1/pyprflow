FROM microsoft/aspnetcore-build:1.0-1.1
ENV pyprflowDbType MSSql
ENV DatabaseType = MSSQL
ENV Server 10.0.0.25
ENV DatabaseName pyprflowDb
ENV ConnectionString Server=10.0.0.25;Database=pyprflowDb;User Id=sa;Password=!!Nimda1;
ENV Port 1433
ENV  Id sa
ENV Pw !!nimda1
COPY ./pyprflow /app
WORKDIR /app
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "run"]
