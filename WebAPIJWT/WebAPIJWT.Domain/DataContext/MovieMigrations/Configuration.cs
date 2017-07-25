namespace WebAPIJWT.Domain.DataContext.MovieMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebAPIJWT.Domain.Db.Movie.MovieDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataContext\MovieMigrations";
        }

        protected override void Seed(WebAPIJWT.Domain.Db.Movie.MovieDb context)
        {
            context.MoviesDetails.AddOrUpdate(
                p => p.Title,
                new Business.Movie.Entities.MovieDetail {Title= "Atomic Blonde", Runtime=115 },
                new Business.Movie.Entities.MovieDetail { Title = "The Emoji Movie", Runtime = 86 },
                new Business.Movie.Entities.MovieDetail { Title = "Spider-Man: Homecoming", Runtime = 120 },
                new Business.Movie.Entities.MovieDetail { Title = "Brigsby Bear", Runtime = 97 },
                new Business.Movie.Entities.MovieDetail { Title = "The Dark Tower", Runtime = 120 },
                new Business.Movie.Entities.MovieDetail { Title = "Wish Upon", Runtime = 89 },
                new Business.Movie.Entities.MovieDetail { Title = "War for the Planet of the Apes", Runtime = 140 }
                );
        }
    }
}
