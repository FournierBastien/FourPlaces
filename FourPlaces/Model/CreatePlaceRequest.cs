using Newtonsoft.Json;

namespace FourPlaces.Model
{
	public class CreatePlaceRequest
	{
		[JsonProperty("title")]
		public string Title { get; set; }
		
		[JsonProperty("description")]
		public string Description { get; set; }
		
		[JsonProperty("image_id")]
		public int ImageId { get; set; }
		
		[JsonProperty("latitude")]
		public double Latitude { get; set; }
		
		[JsonProperty("longitude")]
		public double Longitude { get; set; }

        public CreatePlaceRequest(string title, string description, int imageId, double latitude, double longitude)
        {
            Title = title;
            Description = description;
            ImageId = imageId;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}