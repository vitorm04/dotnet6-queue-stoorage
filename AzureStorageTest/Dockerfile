
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AzureStorageTest/AzureStorageTest.csproj", "AzureStorageTest/"]
RUN dotnet restore "AzureStorageTest/AzureStorageTest.csproj"
COPY . .
WORKDIR "/src/AzureStorageTest"
RUN dotnet build "AzureStorageTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureStorageTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureStorageTest.dll"]