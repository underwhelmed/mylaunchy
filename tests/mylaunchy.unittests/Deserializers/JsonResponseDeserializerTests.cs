using FluentAssertions;
using mylaunchy.repository.spacexapi.Deserializers;
using System.Linq;
using Xunit;

namespace mylaunchy.unittests.Deserializers
{
    public class JsonResponseDeserializerTests
    {

        [Fact]
        void DeserializeLaunchpadsResponse_ReturnsSuccess()
        {
            var json = @"[
{""id"":""dfajsd_38432"",""full_name"": ""a full name of sorts"",""status"":""active""},
{""id"":""929232_38432"",""full_name"": ""another full name of sorts"",""status"":""retired""}
]";
            var result = new JsonResponseDeserializer().DeserializeLaunchpadCollectionResponse(json);
            result.Should().NotBeNull();
            result.ToList().Count().Should().Be(2);
        }

        [Fact]
        void DeserializeLaunchpadsResponse_EmptyArrayJson_ReturnsEmptyList()
        {
            var json = @"[]";
            var result = new JsonResponseDeserializer().DeserializeLaunchpadCollectionResponse(json);
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        void DeserializeLaunchpadsResponse_EmptyString_ReturnsEmptyList()
        {
            var json = string.Empty;
            var result = new JsonResponseDeserializer().DeserializeLaunchpadCollectionResponse(json);
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        void DeserializeLaunchpadsResponse_NullString_ReturnsEmptyList()
        {
            string json = null;
            var result = new JsonResponseDeserializer().DeserializeLaunchpadCollectionResponse(json);
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
        
        [Fact]
        void DeserializeLaunchpadResponse_ReturnsSuccess()
        {
            var json = @"{""id"":""dfajsd_38432"",""full_name"": ""a full name of sorts"",""status"":""active""}";
            var result = new JsonResponseDeserializer().DeserializeLaunchpadResponse(json);
            result.Should().NotBeNull();
            result.Id.Should().Be("dfajsd_38432");
            result.Name.Should().Be("a full name of sorts");
            result.Status.Should().Be("active");
        }

        [Fact]
        void DeserializeLaunchpadResponse_EmptyJson_ReturnsNull()
        {
            var json = @"{}";
            var result = new JsonResponseDeserializer().DeserializeLaunchpadResponse(json);
            result.Should().BeNull();            
        }

        [Fact]
        void DeserializeLaunchpadResponse_EmptyString_ReturnsNull()
        {
            var json = string.Empty;
            var result = new JsonResponseDeserializer().DeserializeLaunchpadResponse(json);
            result.Should().BeNull();
        }

        [Fact]
        void DeserializeLaunchpadResponse_NullString_ReturnsNull()
        {
            string json = null;
            var result = new JsonResponseDeserializer().DeserializeLaunchpadResponse(json);
            result.Should().BeNull();            
        }
    }
}
