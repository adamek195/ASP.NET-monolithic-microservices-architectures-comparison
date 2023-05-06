namespace Multimedia.Web
{
    public static class SD
    {
        public static string ImagesAPIBase { get; set; }

        public static string VideosAPIBase { get; set; }

        public static string UsersAPIBase { get; set; }

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
