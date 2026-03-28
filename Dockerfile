FROM mcr.microsoft.com/dotnet/sdk:8.0

# Google Chrome のインストール
RUN apt-get update && apt-get install -y wget gnupg \
    && wget -q -O - https://dl.google.com/linux/linux_signing_key.pub \
       | gpg --dearmor -o /usr/share/keyrings/google-chrome-keyring.gpg \
    && echo "deb [arch=amd64 signed-by=/usr/share/keyrings/google-chrome-keyring.gpg] \
       https://dl.google.com/linux/chrome/deb/ stable main" \
       > /etc/apt/sources.list.d/google-chrome.list \
    && apt-get update && apt-get install -y google-chrome-stable \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY . .

RUN dotnet restore matome_phase1.sln

CMD ["dotnet", "test", "matome_phase1.Tests/matome_phase1.Tests.csproj", \
     "--no-restore", "--logger", "console;verbosity=normal"]
