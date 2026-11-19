# Cold-Chain-Monitoring
Backend and Frontend for Cold Chain Monitoring System service

## Requirements

- dotnet

## Setup

### Cloning

```bash
# clone the repository
git clone https://github.com/faridfaharaj/Yukimushi-Cold-Chain-Monitoring-System.git

# cd into the project directory
cd Yukimushi-Cold-Chain-Monitoring-System
```

### Enviroment variables

#### Linux

```bash
# change theese to your actual paths
export ASPNETCORE_URLS="https://localhost:5000"
export Mqtt__Host="broker.hivemq.com"
export Mqtt__Port="1833"
export Mqtt__ClientId="Yukimushi-CCMS-App"

# if aplicable:
export Mqtt__UseTls="true"
export Mqtt__Username="username"
export Mqtt__Password="password"

```

#### Windows

```bash
# change theese to your actual paths
$env:ASPNETCORE_URLS = "https://localhost:5000"

```

### Running

```bash
dotnet build
dotnet run --project src/Api/Api.csproj --launch-profile https

```
