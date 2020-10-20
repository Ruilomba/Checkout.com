namespace Presentation.Api.Setup
{
    using _3rdPartyBank;
    using _3rdPartyBank.Implementations;
    using Checkout.com.AcquiringBank.Client;
    using Checkout.com.AcquiringBank.Client.Implementations;
    using Checkout.com.Common.Configuration;
    using Checkout.com.Common.Encryption;
    using Checkout.com.Common.Encryption.Implementations;
    using Checkout.com.Common.Mongo.Implementations;
    using Checkout.com.PaymentGateway.Business.Adapters;
    using Checkout.com.PaymentGateway.Business.Adapters.Implementations;
    using Checkout.com.PaymentGateway.Business.DAL.Repositories;
    using Checkout.com.PaymentGateway.Business.DAL.Repositories.Implementations;
    using Checkout.com.PaymentGateway.Business.Services;
    using Microsoft.Extensions.DependencyInjection;
    using AcquiringBank = Checkout.com.AcquiringBank.Business.Services;
    using PaymentGateway = Checkout.com.PaymentGateway.Business.Services;

    public static class ServicesSetup
    {
        public static IServiceCollection SetupDependencies(this IServiceCollection services, ApplicationSettings applicationSettings)
        {
            return services
                    .AddTransient<IMerchantService, PaymentGateway.Implementations.MerchantService>()
                    .AddTransient<IPaymentService, PaymentGateway.Implementations.PaymentService>()
                    .AddTransient<IPaymentService, PaymentGateway.Implementations.PaymentService>()
                    .AddTransient<IPaymentProcessor, PaymentProcessor>()
                    .AddTransient<IAcquiringBankClient, AcquiringBankClient>()
                    .AddTransient<AcquiringBank.IPaymentProcessor, AcquiringBank.Implementations.PaymentProcessor>()
                    .AddTransient<I3rdPartyBankAPI, _3rdPartyBankAPI>()
                    .AddTransient<IPaymentRepository, PaymentRepository>()
                    .AddTransient<IEncryptionService, EncryptionService>()
                    .AddSingleton<IMongoDbConnection>(new MongoDbConnection(applicationSettings.ConnectionStrings["Payments"]));
        }
    }
}