using System.Buffers;
using System.Text.Json;

namespace HQPlus.Data.Extensions {
    public static class JsonExtensions {
        
        public static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null) {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using var writer = new Utf8JsonWriter(bufferWriter);
            element.WriteTo(writer);
            writer.Flush();
            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }
    }
}