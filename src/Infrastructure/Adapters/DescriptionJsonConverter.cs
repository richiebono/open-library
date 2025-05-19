using System.Text.Json;
using System.Text.Json.Serialization;
using LaunchQ.TakeHomeProject.Domain.Models;

namespace LaunchQ.TakeHomeProject.Infrastructure.Adapters
{
    /// <summary>
    /// JSON converter for Description domain model
    /// </summary>
    public class DescriptionJsonConverter : JsonConverter<Description>
    {
        public override Description? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new Description
                {
                    Value = reader.GetString() ?? string.Empty,
                    Type = "/type/text"
                };
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                using var doc = JsonDocument.ParseValue(ref reader);
                var root = doc.RootElement;
                
                var value = root.TryGetProperty("value", out var valueProperty)
                    ? valueProperty.GetString() ?? string.Empty
                    : string.Empty;
                    
                var type = root.TryGetProperty("type", out var typeProperty)
                    ? typeProperty.GetString() ?? "/type/text"
                    : "/type/text";
                
                return new Description
                {
                    Value = value,
                    Type = type
                };
            }
            
            throw new JsonException("Invalid description format");
        }

        public override void Write(Utf8JsonWriter writer, Description value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("type", value.Type);
            writer.WriteString("value", value.Value);
            writer.WriteEndObject();
        }
    }
}
