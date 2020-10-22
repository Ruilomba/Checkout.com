namespace checkout.com.Tests.Utils
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