FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "Lab13_JhuniorGutierrez.Api/Lab13_JhuniorGutierrez.Api.csproj"

RUN dotnet publish "Lab13_JhuniorGutierrez.Api/Lab13_JhuniorGutierrez.Api.csproj" \
    -c Release \
    --no-restore \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "Lab13_JhuniorGutierrez.Api.dll"]
