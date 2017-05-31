﻿using System;
using System.Collections.Generic;
using System.Linq;
using fletnix.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace fletnix.Models
{
    public class MovieRepository : IMovieRepsitory
    {
        private fletnixContext _context;

        public MovieRepository(fletnixContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetMoviesByName(string movieName, int movieId)
        {
            return _context.Movie.AsNoTracking().Where(m => m.Title.Contains(movieName) && movieId != m.MovieId);
        }

        public bool UpdateGenre(List<MovieGenre> genres, int movieId)
        {
            var listOfItemsToBeRemoved = _context.MovieGenre.AsNoTracking().Where(genre => genre.MovieId == movieId)
                .ToList();
            foreach (var l in listOfItemsToBeRemoved)
            {
                _context.MovieGenre.Remove(l);
            }

            _context.SaveChanges();

            _context.MovieGenre.AddRange(genres);
            
            _context.SaveChanges();

            return true;
        }

        public object GetPersonByName(string personName)
        {
            return _context.Person.AsNoTracking().Where(m => m.Firstname.Contains(personName) || m.Lastname.Contains(personName));
        }

        public bool AddDirector(MovieDirector newDirector)
        {
            _context.MovieDirector.Add(newDirector);
            _context.SaveChanges();
            return true;
        }
    }
}