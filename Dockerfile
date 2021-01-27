FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env
WORKDIR /app

# Copy everything else and build
COPY ./src ./
RUN dotnet restore && dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

# required for healthcheck
RUN apk add --no-cache curl && rm -rf /var/cache/apk/*

COPY ./assets/docker_entrypoint.sh ./

# HEALTHCHECK CMD curl --fail http://localhost:5000/health || exit 1
EXPOSE 5000

ENV ASPNETCORE_URLS http://0.0.0.0:5000

ARG GIT_DESCRIBE=
ARG GIT_COMMIT=
ARG BUILD_DATE=

ENV GIT_DESCRIBE $GIT_DESCRIBE
ENV GIT_COMMIT $GIT_COMMIT

LABEL com.notakey.vcs-ref=$GIT_COMMIT \
    com.notakey.vcs-desc=$GIT_DESCRIBE \
    com.notakey.build-date=$BUILD_DATE

ENTRYPOINT ["/app/docker_entrypoint.sh"]
