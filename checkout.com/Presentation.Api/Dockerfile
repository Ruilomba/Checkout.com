FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["Presentation.Api/Presentation.Api.csproj", "Presentation.Api/"]
COPY ["Checkout.com.PaymentGateway/Checkout.com.PaymentGateway.csproj", "Checkout.com.PaymentGateway/"]
COPY ["Checkout.com.PaymentGateway.DTO/Checkout.com.PaymentGateway.DTO.csproj", "Checkout.com.PaymentGateway.DTO/"]
COPY ["Checkout.com.Common/Checkout.com.Common.csproj", "Checkout.com.Common/"]
COPY ["Checkout.com.AcquiringBank.Client/Checkout.com.AcquiringBank.Client.csproj", "Checkout.com.AcquiringBank.Client/"]
COPY ["Checkout.com.AcquiringBank.DTO/Checkout.com.AcquiringBank.DTO.csproj", "Checkout.com.AcquiringBank.DTO/"]
RUN dotnet restore "Presentation.Api/Presentation.Api.csproj"
COPY . .
WORKDIR "/src/Presentation.Api"
RUN dotnet build "Presentation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.Api.dll"]