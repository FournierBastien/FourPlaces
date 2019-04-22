using Newtonsoft.Json;

namespace FourPlaces.Model
{
    public class UpdatePasswordRequest
    {
        [JsonProperty("old_password")]
        public string OldPassword { get; set; }
        
        [JsonProperty("new_password")]
        public string NewPassword { get; set; }

        public UpdatePasswordRequest(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}