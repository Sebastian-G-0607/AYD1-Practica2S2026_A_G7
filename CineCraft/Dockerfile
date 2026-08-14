# ---------- Build stage ----------
ARG DOTNET_VERSION=10.0
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-noble AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy solution and project files first to leverage Docker layer caching.
# This way, NuGet restore is only re-run when project dependencies change.
COPY CineCraft.slnx ./
COPY src/CineCraft.Api/CineCraft.Api.csproj                 src/CineCraft.Api/
COPY src/CineCraft.ServiceDefaults/CineCraft.ServiceDefaults.csproj src/CineCraft.ServiceDefaults/
COPY src/CineCraft.AppHost/CineCraft.AppHost.csproj         src/CineCraft.AppHost/

# Restore dependencies for the API project (and its transitive deps).
RUN dotnet restore "src/CineCraft.Api/CineCraft.Api.csproj"

# Copy the full source tree and publish a Release build.
# UseAppHost=true (default) produces a native apphost binary so the runtime
# container doesn't need the `dotnet` CLI to launch the app.
COPY . .
RUN dotnet publish "src/CineCraft.Api/CineCraft.Api.csproj" \
    --configuration ${BUILD_CONFIGURATION} \
    --no-restore \
    --output /app/publish

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-noble AS runtime

WORKDIR /app

# Ensure the published files are owned by the non-root user.
COPY --from=build --chown=app:app /app/publish .

# Expose HTTP traffic. Port 8080 is the container standard and works for
# non-root users (unprivileged ports are >= 1024).
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080

# Health probing:
#   The chiseled image has no shell/curl/wget, so HEALTHCHECK is omitted here.
#   Probe the ServiceDefaults endpoints from the orchestrator (Docker Compose,
#   Kubernetes, etc.) instead:
#     /health/ready  -> readiness
#     /health/alive  -> liveness
#   Example for docker-compose:
#     healthcheck:
#       test: ["CMD-SHELL", "wget -qO- http://127.0.0.1:8080/health/alive || exit 1"]
#
# Graceful shutdown on SIGTERM (default k8s/Docker behavior).
ENTRYPOINT ["/app/CineCraft.Api"]

# Switch to the built-in non-root user (UID 65532) for least privilege.
USER app

# Expose HTTP traffic. Port 8080 is the container standard and works for
# non-root users (unprivileged ports are >= 1024).
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080

# Health probing:
#   The chiseled image has no shell/curl/wget, so HEALTHCHECK is omitted here.
#   Probe the ServiceDefaults endpoints from the orchestrator (Docker Compose,
#   Kubernetes, etc.) instead:
#     /health/ready  -> readiness
#     /health/alive  -> liveness
#   Example for docker-compose:
#     healthcheck:
#       test: ["CMD-SHELL", "wget -qO- http://127.0.0.1:8080/health/alive || exit 1"]
#
# Graceful shutdown on SIGTERM (default k8s/Docker behavior).
ENTRYPOINT ["/app/CineCraft.Api"]
