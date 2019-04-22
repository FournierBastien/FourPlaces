using Newtonsoft.Json;

namespace FourPlaces.Model
{
	public class RegisterRequest
	{
		[JsonProperty("email")]
		public string Email { get; set; }
		
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		
		[JsonProperty("last_name")]
		public string LastName { get; set; }
		
		[JsonProperty("password")]
		public string Password { get; set; }

        public RegisterRequest(string email, string firstName, string lastName, string password)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }
    }
}