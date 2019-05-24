ARG runtime_tag
ARG sdk_tag
FROM microsoft/dotnet:${sdk_tag} AS build-env
ARG version=1.0.0.0
WORKDIR /app/mylaunchy.services.launchpad

# copy entire repo (minus ignored) to /app
COPY . /app/

# restore and publish
RUN dotnet restore
RUN dotnet publish -c Release -o out /p:AssemblyVersion=$version

# build runtime image
FROM microsoft/dotnet:${runtime_tag} as runtime
WORKDIR /app
COPY --from=build-env /app/mylaunchy.services.launchpad/out .
ENTRYPOINT ["dotnet", "mylaunchy.services.launchpad.dll"]
