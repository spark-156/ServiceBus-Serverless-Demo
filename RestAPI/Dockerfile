FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RestAPI-2.0/RestAPI-2.0.csproj", "RestAPI-2.0/"]
RUN dotnet restore "RestAPI-2.0/RestAPI-2.0.csproj"
COPY . .
WORKDIR "/src/RestAPI-2.0"
RUN dotnet build "RestAPI-2.0.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestAPI-2.0.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestAPI-2.0.dll"]
