#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["DemoMicroservices/Messages/Messages.csproj", "DemoMicroservices/Messages/Messages.csproj"]
COPY ["DemoMicroservices/Producer/Producer.csproj", "DemoMicroservices/Producer/"]

RUN dotnet restore "DemoMicroservices/Producer/Producer.csproj"

COPY . .
WORKDIR "/src/DemoMicroservices/Producer"
RUN dotnet build "Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Producer.dll"]