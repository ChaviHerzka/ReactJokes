using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReactJokes.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using ReactJokes.Web.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;

namespace ReactJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        private readonly string _connectionString;
        public JokeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpGet]
        [Route("getjoke")]
        public Joke GetJoke()
        {
            var client = new HttpClient();
            string json = client.GetStringAsync("https://api.chucknorris.io/jokes/random").Result;
            var apiJoke = JsonConvert.DeserializeObject<ApiJoke>(json);

            var repo = new JokesRepository(_connectionString);
            var newJoke = new Joke
            {
                ExternalId = apiJoke.Id,
                Content = apiJoke.Value
            };
            var joke = repo.GetJoke(newJoke);
            if (joke == null)
            {
                repo.AddJoke(newJoke);
                joke = newJoke;
            }
            return joke;

        }

        [HttpGet]
        [Route("getalljokes")]
        public List<Joke> GetAllJokes()
        {
            var repo = new JokesRepository(_connectionString);
            return repo.GetAllJokes();
        }

        [HttpGet]
        [Route("getupdatedjoke/{jokeId}")]
        public Joke GetUpdatedJoke(int jokeId)
        {
            var repo = new JokesRepository(_connectionString);
            return repo.GetJokeById(jokeId);
        }

        [HttpPost]
        [Route("addlike")]
        public Joke AddLike(UserLikedJoke likedJoke)
        {
            var like = new UserLikedJoke
            {
                UserId = likedJoke.UserId,
                JokeId = likedJoke.JokeId,
                Liked = likedJoke.Liked,
                Time = System.DateTime.Now
            };
            var repo = new JokesRepository(_connectionString);
            return repo.AddLike(like);
        }

        [HttpPost]
        [Route("updatelike")]
        public Joke UpdateLike(UserLikedJoke likedJoke)
        {
            var like = new UserLikedJoke
            {
                UserId = likedJoke.UserId,
                JokeId = likedJoke.JokeId,
                Liked = likedJoke.Liked,
                Time = System.DateTime.Now
            };
            var repo = new JokesRepository(_connectionString);
           var returnObject =  repo.UpdateLike(like);
            return returnObject;
        }
        [HttpGet]
        [Route("getlikescount/{jokeid}")]
        public object GetLikesCount(int jokeId)
        {
            var repo = new JokesRepository(_connectionString);
            var userLikeJoke = repo.GetWithLikes(jokeId);
            return new
            {
                likes = userLikeJoke.Count(u => u.Liked),
                dislikes = userLikeJoke.Count(u => !u.Liked)
            };
        }
        [HttpGet]
        [Route("IsBeforeOneMinute")]
        public bool IsBeforeOneMinute(DateTime date)
        {
            var repo = new JokesRepository(_connectionString);
            return repo.IsBeforeOneMinute(date);
        }
    };
}
