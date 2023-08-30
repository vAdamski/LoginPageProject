FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LoginPageProject/LoginPageProject.csproj", "LoginPageProject/"]
RUN dotnet restore "LoginPageProject/LoginPageProject.csproj"
COPY . .
WORKDIR "/src/LoginPageProject"
RUN dotnet build "LoginPageProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoginPageProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoginPageProject.dll"]
