using System;
using Newtonsoft.Json;

namespace FourPlaces.Model
{
	public class CommentItem
	{
		[JsonProperty("date")]
		public DateTime Date { get; set; }
		
		[JsonProperty("author")]
		public UserItem Author { get; set; }
		
		[JsonProperty("text")]
		public string Text { get; set; }

        public CommentItem(DateTime date, UserItem author, string text)
        {
            Date = date;
            Author = author;
            Text = text;
        }
    }
}