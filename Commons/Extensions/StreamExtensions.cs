namespace aspnetcore.Commons.Extensions
{
    public static class StreamExtensions
    {
            public static async Task<string> ReadAsStringAsync(this Stream requestBody, bool leaveOpen = false)
            {
                using StreamReader reader = new(requestBody, leaveOpen: leaveOpen);
                var bodyAsString = await reader.ReadToEndAsync();
                requestBody.Position = 0;
                return bodyAsString;
            } 
    }
}
