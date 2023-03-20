namespace EcommerceApi.Middlewares
{
    public static class HttpRequestExtensions
    {
        public static Guid GetHeaderValue(this HttpRequest request, string headerName)
        {
            if (request.Headers.TryGetValue(headerName, out var values) && Guid.TryParse(values.FirstOrDefault(), out var guidValue))
            {
                return guidValue;
            }

            return Guid.Empty;
        }
    }
}
