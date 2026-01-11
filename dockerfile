
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG ConnectionStrings__DatabasePostgreSQL=${ConnectionStrings__DatabasePostgreSQL}
ENV ConnectionStrings__DatabasePostgreSQL=${ConnectionStrings__DatabasePostgreSQL}
ARG ConnectionStrings__RedisConnection=${ConnectionStrings__RedisConnection}
ENV ConnectionStrings__RedisConnection=${ConnectionStrings__RedisConnection}
ARG ConnectionStrings__HostUrl=${ConnectionStrings__HostUrl}
ENV ConnectionStrings__HostUrl=${ConnectionStrings__HostUrl}

ARG HangfireDashboard__Username=${HangfireDashboard__Username}
ENV HangfireDashboard__Username=${HangfireDashboard__Username}

ARG HangfireDashboard__Password=${HangfireDashboard__Password}
ENV HangfireDashboard__Password=${HangfireDashboard__Password}

ARG JwtOptions__Issuer=${JwtOptions__Issuer}
ENV JwtOptions__Issuer=${JwtOptions__Issuer}
ARG JwtOptions__Audience=${JwtOptions__Audience}
ENV JwtOptions__Audience=${JwtOptions__Audience}
ARG JwtOptions__SecurityKey=${JwtOptions__SecurityKey}
ENV JwtOptions__SecurityKey=${JwtOptions__SecurityKey}

ARG TelegramBot__Token=${TelegramBot__Token}
ENV TelegramBot__Token=${TelegramBot__Token}
ARG TelegramBot__ChatId=${TelegramBot__ChatId}
ENV TelegramBot__ChatId=${TelegramBot__ChatId}

ARG MinioSettings__Endpoint
ARG MinioSettings__AccessKey
ARG MinioSettings__SecretKey
ARG MinioSettings__UseSSL
ARG MinioSettings__DefaultBucketName

ENV MinioSettings__Endpoint=${MinioSettings__Endpoint}
ENV MinioSettings__AccessKey=${MinioSettings__AccessKey}
ENV MinioSettings__SecretKey=${MinioSettings__SecretKey}
ENV MinioSettings__UseSSL=${MinioSettings__UseSSL}
ENV MinioSettings__DefaultBucketName=${MinioSettings__DefaultBucketName}

WORKDIR /app/api

COPY *.sln ./

COPY bingo-api/*.csproj ./bingo-api/
COPY bingo-tests/*.csproj ./bingo-tests/

RUN dotnet clean
RUN dotnet restore

COPY . ./

RUN dotnet build --configuration Release --output /app/build

RUN dotnet publish --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

RUN apt-get update && \
    apt-get install -y locales tzdata

ENV TZ=America/Sao_Paulo

WORKDIR /app/api

COPY --from=build /app/publish .

RUN locale-gen en_US.UTF-8 en_GB.UTF-8 de_DE.UTF-8 es_ES.UTF-8 fr_FR.UTF-8 it_IT.UTF-8 km_KH sv_SE.UTF-8 fi_FI.UTF-8 pt_BR.UTF-8

ENV LANG=pt_BR.UTF-8 LC_ALL=pt_BR.UTF-8

ENTRYPOINT ["dotnet", "bingo-api.dll"]
