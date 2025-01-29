using System;
using System.Text.Json;

namespace TodoApi.Models
{
    public class JsonSerializationHelper
    {
        string SerializeObject<T>(T obj)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                    MaxDepth = 64 // Optional: Increase depth limit if needed
                };

                return JsonSerializer.Serialize(obj, options);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Serialization error: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
