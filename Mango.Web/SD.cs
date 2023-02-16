namespace Mango.Web
{
    // create this as static class
    public static class SD
    {
        public static string ProductAPIBase { get; set; }
        public static string CartAPIBase { get; set; }

        // whenever we make an API call better to use enum
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
