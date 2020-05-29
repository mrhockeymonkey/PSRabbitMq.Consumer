FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic

RUN wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb &&\
    dpkg -i packages-microsoft-prod.deb &&\
    apt-get update &&\
    apt-get install -y powershell=7.0.1-1.ubuntu.18.04

RUN pwsh -Command " \
    Install-Module InvokeBuild -RequiredVersion 5.6.0 -Force; \
    Install-Module PSRabbitMq -RequiredVersion 0.3.3 -Force; \
    "

WORKDIR /tmp/build
ENTRYPOINT ["pwsh", "-Command"]


