FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only what User API needs (avoid other projects like UserRebate)
COPY NDISAU.Solution.sln ./
COPY NDIS.ClassLibrary/ NDIS.ClassLibrary/
COPY NDIS.User.API/ NDIS.User.API/

RUN dotnet restore NDIS.User.API/NDIS.User.API.csproj
RUN dotnet publish NDIS.User.API/NDIS.User.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "NDIS.User.API.dll"]
