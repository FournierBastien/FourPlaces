using System.Collections.Generic;
using Newtonsoft.Json;

namespace FourPlaces.Model
{
	public class PlaceItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		
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
		
		[JsonProperty("comments")]
		public List<CommentItem> Comments { get; set; }

        public PlaceItem(int id, string title, string description, int imageId, double latitude, double longitude, List<CommentItem> comments)
        {
            Id = id;
            Title = title;
            Description = description;
            ImageId = imageId;
            Latitude = latitude;
            Longitude = longitude;
            Comments = comments;
        }
    }
}