using _1_AuthorsBooks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_AuthorsBooks.Controllers
{
    public class GenreController : Controller
    {
        private readonly IODatabase _iODatabase;
        public GenreController(IODatabase iODatabase)
        {
            _iODatabase = iODatabase;
        }
        //Жанры
        public async Task<IActionResult> Index()
        {
            var genres = await _iODatabase.GetGenres();
            return View(genres);
        }
    }
}
