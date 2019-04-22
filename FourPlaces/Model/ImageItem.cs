using Newtonsoft.Json;

namespace FourPlaces.Model
{
	public class ImageItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }

        public string Url { get; set; }

        public ImageItem(int id, string url)
        {
            Id = id;
            Url = url;
        }

        public ImageItem(int id)
        {
            Id = id;
        }
    }


}