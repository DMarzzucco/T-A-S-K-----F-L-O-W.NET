FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5024
EXPOSE 5024

ENV ASPNETCORE_ENVIRONMENT Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TASK-FLOW.NET/TASK-FLOW.NET.csproj", "TASK-FLOW.NET/"]
RUN dotnet restore "./TASK-FLOW.NET/TASK-FLOW.NET.csproj"
COPY . .
WORKDIR "/src/TASK-FLOW.NET"
RUN dotnet tool install --global dotnet-ef 
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet build "./TASK-FLOW.NET.csproj" -c $BUILD_CONFIGURATION -o /app/build
CMD dotnet ef database update --environment Production --project src/TASK-FLOW.NET

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TASK-FLOW.NET.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TASK-FLOW.NET.dll"]