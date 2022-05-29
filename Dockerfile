#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Gabriel/Gabriel.csproj", "Gabriel/"]
COPY ["Gabriel.Data/Gabriel.Data.csproj", "Gabriel.Data/"]
COPY ["Gabriel.Models/Gabriel.Models.csproj", "Gabriel.Models/"]
RUN dotnet restore "Gabriel/Gabriel.csproj"
COPY . .
WORKDIR "/src/Gabriel"
RUN dotnet build "Gabriel.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gabriel.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gabriel.dll"]