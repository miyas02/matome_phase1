FROM mcr.microsoft.com/dotnet/sdk:8.0

# Chromium のインストール（amd64/arm64 両対応）
RUN apt-get update && apt-get install -y \
    chromium \
    chromium-driver \
    && rm -rf /var/lib/apt/lists/* \
    && ln -s /usr/bin/chromium /usr/bin/google-chrome

WORKDIR /app
COPY . .

RUN dotnet restore matome_phase1.Tests/matome_phase1.Tests.csproj -p:DOCKER_BUILD=true

CMD ["dotnet", "test", "matome_phase1.Tests/matome_phase1.Tests.csproj", \
     "--no-restore", "--logger", "console;verbosity=normal", \
     "-p:DOCKER_BUILD=true"]
