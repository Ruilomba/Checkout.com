namespace checkout.com.IntegrationTests.Utils
{
    using Newtonsoft.Json;

    public static class Utils
    {
        public static T GetData<T>(dynamic data)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
        }
    }
}