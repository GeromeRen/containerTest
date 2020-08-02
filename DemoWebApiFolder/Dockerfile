FROM acncio.azurecr.io/dotnet31sdkcio:latest AS build
RUN dotnet --info
WORKDIR /src
COPY ["MyDemoWebApi3_1_5064.WebApi/MyDemoWebApi3_1_5064.WebApi.csproj", "MyDemoWebApi3_1_5064.WebApi/"]

COPY ["Accenture.Evergreen.Middleware/Accenture.Evergreen.Middleware.csproj", "Accenture.Evergreen.Middleware/"]
    
RUN dotnet restore "MyDemoWebApi3_1_5064.WebApi/MyDemoWebApi3_1_5064.WebApi.csproj"
COPY . .
WORKDIR "/src/MyDemoWebApi3_1_5064.WebApi"
RUN dotnet build "MyDemoWebApi3_1_5064.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyDemoWebApi3_1_5064.WebApi.csproj" -c Release -o /app

FROM acncio.azurecr.io/aspnetcio:latest AS final
COPY --from=publish /app .
CMD ["dotnet", "MyDemoWebApi3_1_5064.WebApi.dll"]