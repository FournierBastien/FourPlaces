using Newtonsoft.Json;

namespace FourPlaces.Model
{
    public class UpdateProfileRequest
    {
        public UpdateProfileRequest(string firstName, string lastName, int? imageId)
        {
            FirstName = firstName;
            LastName = lastName;
            ImageId = imageId;
        }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        [JsonProperty("image_id")]
        public int? ImageId { get; set; }
    }
}