# PSRabbitMq.Consumer
This module aims to extend [PSRabbitMQ](https://github.com/RamblingCookieMonster/PSRabbitMq) and focuses on consumption of messages only. For configuring rabbitmq and publishing messages I recommend using PSRabbitMQ or other client library.

This module is built for dotnet core and powershell 7.0+ on windows and linux. 

## Usage
see [examples](./examples) here.

## Contributing
To build this module locally you will need:
- powershell 7 or later
- dotnet sdk 3.1 or later

```powershell
# compile module after making changes
# this will open a new powershell window and load the module for immediate use
Invoke-Build

# run unit tests locally
Invoke-Build UnitTests
```