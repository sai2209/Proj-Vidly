using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Models;
using Vidly.Dtos;
using AutoMapper;
using System.Data.Entity;

namespace Vidly.Controllers.Api
{
    public class MoviesDtoController : ApiController
    {
        private CustomerContext _context;
        public MoviesDtoController()
        {
            _context = new CustomerContext();
        }

        //GET  /api/Movies
        public IEnumerable<MovieDto> GetMovie(string query=null)
        {
            var moviesQuery = _context.Movies
                .Include(c => c.Genres).Where(c => c.NumberAvailable > 0);
            if (!string.IsNullOrWhiteSpace(query))
                moviesQuery = moviesQuery.Where(c => c.Name.Contains(query));
            var moviesDtos = moviesQuery
                .ToList()
                .Select(Mapper.Map<Movie,MovieDto>);

            return moviesDtos;
        }

        //GET  /api/Movies/1

        public MovieDto GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return Mapper.Map<Movie,MovieDto>(movie);
        }

        //POST  /api/Movies
        [HttpPost]
        public MovieDto CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var movie = Mapper.Map<MovieDto, Movie>(movieDto);
            _context.Movies.Add(movie);
            _context.SaveChanges();

            movie.Id = movieDto.Id;
            return movieDto;
        }

        //PUT /api/Movies/1
        [HttpPut]
        public void UpdateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == movieDto.Id);
            if (movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map<MovieDto, Movie>(movieDto, movieInDb);
            //movieInDb.Name = movie.Name;
            //movieInDb.ReleaseDate = movie.ReleaseDate;
            //movieInDb.Stock = movie.Stock;
            //movieInDb.GenreId = movie.GenreId;
            //movieInDb.DateAdded = movie.DateAdded;
            _context.SaveChanges();
        }

        //DELETE /api/Movies/1
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _context.Movies.Remove(movieInDb);
            _context.SaveChanges();
        }
    }
}
