FROM node:16 as node_builder
LABEL authors="Alex Roehm"
# update dependencies and install curl


# Create app directory
WORKDIR /build

COPY . .

# update each dependency in package.json to the latest version
RUN yarn

# building code for production
RUN yarn build

FROM mcr.microsoft.com/dotnet/sdk:7.0 as dotnet_builder
LABEL authors="Alex Roehm"
# update dependencies and install curl


# Create app directory
WORKDIR /build

COPY . .

WORKDIR /build/backend
# building code for production
RUN dotnet build

FROM mcr.microsoft.com/dotnet/aspnet:7.0
LABEL authors="Alex Roehm"

WORKDIR /app

COPY --from=node_builder /build/dist ./dist
COPY --from=dotnet_builder /build/backend/bin/Debug/net7.0 .
VOLUME /app/data

EXPOSE 5084

CMD ["dotnet", "backend.dll"]