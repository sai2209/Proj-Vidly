using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private CustomerContext _context;

        public MoviesController()
        {
            _context = new CustomerContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Movies
        //public ActionResult Random()
        //{

        //    var movie = new Movie { Name = "Sherk!" };
        //    //How movie obj goes to model which can be accessed in the view
        //    //var viewResult = new ViewResult();
        //    //viewResult.ViewData.Model;

        //    //Creating Customer objects
        //    var Customers = new List<Customer>
        //    {
        //        new Customer{Name="Customer 1"},
        //        new Customer{Name="Customer 2"}
        //    };

        //    var ViewModel = new RandomMovieViewModel
        //    {
        //        Movie = movie,
        //        Customers = Customers

        //    };
        //    return View(ViewModel);
        //}
        
        //[Route("movies/released/{year:regex(\\d{4})}/{month:regex(\\d{2}):range(1,12)}")]
        //public ActionResult ByReleaseDate(int year,int month)
        //{
        //    return Content(year+"/"+month);
        //}

        public ActionResult GetMoviesList()
        {
            //var movies = _context.Movies.Include(c=>c.Genres).ToList();
            //return View(movies);
            return View();
        }

        public ActionResult GetMovieDetails(int id)
        {
            var name = _context.Movies.Include(c=>c.Genres).SingleOrDefault(c => c.Id == id);
            if (name != null)
            {
                var movie = new Movie
                {

                    Name = name.Name,
                    Genres = name.Genres,
                    ReleaseDate = name.ReleaseDate,
                    DateAdded = name.DateAdded,
                    Stock = name.Stock,
                    GenreId = name.GenreId,
                    Id = name.Id
                    
                    
                };
                return View(movie);
            }
            else
            {
                return HttpNotFound();
            }
        }

        //public IEnumerable<Movie> GetMovies()
        //{
        //    return new List<Movie>
        //    {
        //        new Movie{Name="Shrek",Id=1},
        //        new Movie{Name="Wall-e",Id=2}
        //    };
        //}

        public ActionResult NewMovie()
        {
            var Generes = _context.Genres.ToList();
            var viewModel = new NewMovieViewModel
            {
                Genres = Generes
            };
            ViewBag.Mode = "New Movie";
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult  Save(NewMovieViewModel movie)
        {
            if (movie.Movies != null && movie.Movies.Id == null)
                movie.Movies.Id = 0;
            if(!ModelState.IsValid)
            {
                var viewModel = new NewMovieViewModel
                {
                    Movies = movie.Movies,
                    Genres = _context.Genres.ToList()
                };
                return View("NewMovie", viewModel);
            }
            movie.Movies.DateAdded = DateTime.Now;
            if (movie.Movies.Id == 0)
            {
                _context.Movies.Add(movie.Movies);
            }
            else
            {
                var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == movie.Movies.Id);
                movieInDb.Name = movie.Movies.Name;
                movieInDb.ReleaseDate = movie.Movies.ReleaseDate;
                movieInDb.GenreId = movie.Movies.GenreId;
                movieInDb.DateAdded = movie.Movies.DateAdded;
                movieInDb.Stock = movie.Movies.Stock;
            }
            _context.SaveChanges();
            return RedirectToAction("GetMoviesList", "Movies");
        }

        public ActionResult EditMovieDetails(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);
            if(movie==null)
            {
                return HttpNotFound();
            }
            var movieViewModel = new NewMovieViewModel
            {
                Movies = movie,
                Genres = _context.Genres.ToList()
            };
            ViewBag.Mode = "Edit Movie";
            return View("NewMovie",movieViewModel);
        }
    }
}