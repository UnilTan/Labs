# Utility container for running EF Core tools or quick database checks.
# Usage example (from repo root):
#   docker build -t milkproducts-tools -f docker/Dockerfile .
#   docker run --rm -it -v ${PWD}:/workspace -w /workspace milkproducts-tools dotnet ef --help

FROM mcr.microsoft.com/dotnet/sdk:8.0

ENV PATH="/root/.dotnet/tools:${PATH}"
WORKDIR /workspace

RUN dotnet tool install --global dotnet-ef

CMD ["dotnet", "ef"]
