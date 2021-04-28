FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env
WORKDIR /app

# Copy everything else and build
COPY ./src ./
RUN dotnet restore && dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

COPY ./assets/docker_entrypoint.sh ./
COPY ./certs /certs

EXPOSE 5000

ENV ASPNETCORE_URLS=http://0.0.0.0:5000 \
    OIDC_KEYPAIR_PASS=asdlkj \
    OIDC_KEYPAIR_PFX_FILE=/certs/oidc_certificate.pfx \
    OIDC_CLIENT_NAME=NtkAS

ENTRYPOINT ["/app/docker_entrypoint.sh"]
