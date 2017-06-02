FROM microsoft/dotnet:1.1-sdk-projectjson
COPY ./pyprflow /app
WORKDIR /app
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "run"]
