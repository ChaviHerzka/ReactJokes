using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace ReactJokes.Data
{
    public class JokesRepository
    {
        private readonly string _connectionString;
        public JokesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Joke GetJokeById(int jokeId)
        {
            using var ctx = new JokesDbContext(_connectionString);
            return ctx.Jokes.Include(j=> j.Liked).FirstOrDefault(ulj => ulj.Id == jokeId); ;
        }
        public Joke GetJoke(Joke joke)
        {
            using var ctx = new JokesDbContext(_connectionString);
            return ctx.Jokes.Include(j => j.Liked).FirstOrDefault(j => j.Content == joke.Content);
        }

        public void AddJoke(Joke joke)
        {
            using var ctx = new JokesDbContext(_connectionString);
                ctx.Jokes.Add(joke);
                ctx.SaveChanges();
        }
        public List<Joke>GetAllJokes()
        {
            var ctx = new JokesDbContext(_connectionString);
            return ctx.Jokes.Include(j => j.Liked).ToList();
        }
        public Joke AddLike(UserLikedJoke like)
        {
            var ctx = new JokesDbContext(_connectionString);
            ctx.Liked.Add(like);
            ctx.SaveChanges();
            return ctx.Jokes.FirstOrDefault(j => j.Id == like.JokeId);
        }
        public  List<UserLikedJoke> GetWithLikes(int jokeId)
        {
            var ctx = new JokesDbContext(_connectionString);
            return ctx.Liked.Where(u => u.JokeId == jokeId).ToList();
        } 
        public Joke UpdateLike(UserLikedJoke like)
        {
            var ctx = new JokesDbContext(_connectionString);
            ctx.Database.ExecuteSqlInterpolated($"Update Liked set Liked = {like.Liked} where JokeId = {like.JokeId}");
            //ctx.Liked.Attach(like);
            //ctx.Entry(like).State = EntityState.Modified;
            ctx.SaveChanges();
            return ctx.Jokes.Include(j=> j.Liked).FirstOrDefault(j => j.Id == like.JokeId);
        }
        public bool IsBeforeOneMinute(DateTime date)
        {
            DateTime currentDate = DateTime.Now;
            TimeSpan timeSpan = currentDate.Subtract(date);
            return timeSpan.TotalMinutes < 1;
        }

    }
}
