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
export ConnectionStrings__DefaultConnection="Server=127.0.0.1,1433;Database=Yukimushi;Trusted_Connection=True;TrustServerCertificate=True;"

```

#### Windows

```bash
# change theese to your actual paths
$env:ASPNETCORE_URLS = "https://localhost:5000"
$env:ConnectionStrings__DefaultConnection = "Server=127.0.0.1,1433;Database=Yukimushi;Trusted_Connection=True;TrustServerCertificate=True;"

```

### Running

```bash
dotnet build
dotnet run --project src/Api/Api.csproj --launch-profile https

```
