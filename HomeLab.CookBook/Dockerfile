#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HomeLab.CookBook.Api/HomeLab.CookBook.Api.csproj", "HomeLab.CookBook.Api/"]
RUN dotnet restore "HomeLab.CookBook.Api/HomeLab.CookBook.Api.csproj"
COPY . .
WORKDIR "/src/HomeLab.CookBook.Api"
RUN dotnet build "HomeLab.CookBook.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeLab.CookBook.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeLab.CookBook.Api.dll"]