using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Models.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Models.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public SelectList Genere { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MovieGenere { get; set; }

        public async Task OnGetAsync()
        {
            // Use LINQ to get list of genres.
            IQueryable<String> genereQuary = from entity in _context.Movie
                                             orderby entity.Genere
                                             select entity.Genere;

            var movies = from entity in _context.Movie
                         select entity;

            if (!String.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s =>
                s.Title.Contains(SearchString));
            }
            if (!String.IsNullOrEmpty(MovieGenere))
            {
                movies = movies.Where(s =>
                s.Genere == MovieGenere);
            }

            Genere = new SelectList(await genereQuary.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
