FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY backend/bin/Debug/net7.0 .
COPY dist ./dist

EXPOSE 5202

CMD ["dotnet", "backend.dll"]