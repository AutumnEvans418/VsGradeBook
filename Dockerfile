FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-nanoserver-1903

WORKDIR /app
EXPOSE 80
EXPOSE 443
copy ./Grader.Web/bin/Release/netcoreapp2.2/publish .

ENTRYPOINT ["dotnet", "Grader.Web.dll"]

#FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-nanoserver-1903 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/core/sdk:2.2-nanoserver-1903 AS build
#WORKDIR /src
#COPY ["Grader.Web/Grader.Web.csproj", "Grader.Web/"]
#RUN dotnet restore "Grader.Web/Grader.Web.csproj"
#COPY . .
#WORKDIR "/src/Grader.Web"
#RUN dotnet build "Grader.Web.csproj" -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish "Grader.Web.csproj" -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "Grader.Web.dll"]