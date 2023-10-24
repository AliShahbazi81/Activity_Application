FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["ActivityApplication/ActivityApplication.csproj", "ActivityApplication/"]
COPY ["ActivityApplication.DataAccess/ActivityApplication.DataAccess.csproj", "ActivityApplication.DataAccess/"]
COPY ["ActivityApplication.Domain/ActivityApplication.Domain.csproj", "ActivityApplication.Domain/"]
COPY ["ActivityApplication.Infrastructure.UploadImage/ActivityApplication.Infrastructure.UploadImage.csproj", "ActivityApplication.Infrastructure.UploadImage/"]
COPY ["ActivityApplication.Services.Courdinary/ActivityApplication.Services.Courdinary.csproj", "ActivityApplication.Services.Courdinary/"]
COPY ["ActivityApplication.Services/ActivityApplication.Services.csproj", "ActivityApplication.Services/"]
COPY ["ActivityApplication.Services.Image/ActivityApplication.Services.Image.csproj", "ActivityApplication.Services.Image/"]
COPY ["ActivityApplication.Infrastructure/ActivityApplication.Infrastructure.csproj", "ActivityApplication.Infrastructure/"]
COPY ["ActivityApplication.Services.Membership/ActivityApplication.Services.Membership.csproj", "ActivityApplication.Services.Membership/"]
COPY ["ActivityApplication.Services.Activity/ActivityApplication.Services.Activity.csproj", "ActivityApplication.Services.Activity/"]
COPY ["ActivityApplication.Services.User/ActivityApplication.Services.User.csproj", "ActivityApplication.Services.User/"]
COPY ["ActivityApplication.Services.Comment/ActivityApplication.Services.Comment.csproj", "ActivityApplication.Services.Comment/"]
RUN dotnet restore "ActivityApplication/ActivityApplication.csproj"
COPY . .
WORKDIR "/src/ActivityApplication"
RUN dotnet build "ActivityApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ActivityApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ActivityApplication.dll"]
