using Newtonsoft.Json;

namespace FourPlaces.Model
{
	public class CreateCommentRequest
	{
		[JsonProperty("text")]
		public string Text { get; set; }

        public CreateCommentRequest(string text)
        {
            Text = text;
        }
    }
}