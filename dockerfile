
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG DatabasePostgreSQL=${DatabasePostgreSQL}
ENV RedisConnection=${RedisConnection}
WORKDIR /app/api

COPY *.sln ./

COPY bingo-api/*.csproj ./bingo-api/
COPY bingo-tests/*.csproj ./bingo-tests/

RUN dotnet clean
RUN dotnet restore

COPY . ./

RUN dotnet build --configuration Release --output /app/build

RUN dotnet publish --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

RUN apt-get update && \
    apt-get install -y locales tzdata

ENV TZ=America/Sao_Paulo

WORKDIR /app/api

COPY --from=build /app/publish .

RUN locale-gen en_US.UTF-8 en_GB.UTF-8 de_DE.UTF-8 es_ES.UTF-8 fr_FR.UTF-8 it_IT.UTF-8 km_KH sv_SE.UTF-8 fi_FI.UTF-8 pt_BR.UTF-8

ENV LANG=pt_BR.UTF-8 LC_ALL=pt_BR.UTF-8

ENTRYPOINT ["dotnet", "bingo-api.dll"]
