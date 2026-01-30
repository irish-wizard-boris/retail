using System.Text.Json.Serialization;

namespace API.Response
{
    public class DomainErrorResponse
    {
		[JsonPropertyName("errorName")]
		public string ErrorName { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }
	}
}
