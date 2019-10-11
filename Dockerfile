FROM microsoft/aspnetcore:2.2


WORKDIR /app

copy ./Grader.Web/bin/Release/netcoreapp2.2/publish

ENTRYPOINT ["dotnet", "Grader.Web.dll"]