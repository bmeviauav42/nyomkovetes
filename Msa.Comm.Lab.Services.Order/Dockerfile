FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Msa.Comm.Lab.Services.Order/Msa.Comm.Lab.Services.Order.csproj", "Msa.Comm.Lab.Services.Order/"]
RUN dotnet restore "Msa.Comm.Lab.Services.Order/Msa.Comm.Lab.Services.Order.csproj"
COPY . .
WORKDIR "/src/Msa.Comm.Lab.Services.Order"
RUN dotnet build "Msa.Comm.Lab.Services.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Msa.Comm.Lab.Services.Order.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Msa.Comm.Lab.Services.Order.dll"]