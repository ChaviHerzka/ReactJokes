using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReactJokes.Data
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public List<UserLikedJoke> Liked { get; set; }
    }
    public class Joke
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Content { get; set; }
       
        public List<UserLikedJoke> Liked { get; set; }
    }
    public class ApiJoke
    {
        public string Id { get; set; }
        public string Value { get; set; }

    }
    public class UserLikedJoke
    { 
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int JokeId { get; set; }
        [JsonIgnore]
        public Joke Joke { get; set; }
        public DateTime Time { get; set; }
        public bool Liked { get; set; }
    }
}
