using _1_AuthorsBooks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace _1_AuthorsBooks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IODatabase _iODatabase;
        private readonly IMemoryCache _memoryCache;

        public HomeController(ILogger<HomeController> logger, IODatabase iODatabase, IMemoryCache memoryCache)
        {
            _logger = logger;
            _iODatabase = iODatabase;
            _memoryCache = memoryCache;
        }

        //первая загрузка страницы
        public async Task<IActionResult> Index()
        {
            var authors = await _iODatabase.GetAuthors();

            return View(authors);
        }


        // Загрузка страницы с данными из формы
        [HttpPost]
        public async Task<IActionResult> Index(List<Author> inAuthors)
        {
            List<Author> newAuthors = null;

            if (ModelState.IsValid)
            {
                newAuthors = await _iODatabase.UpdateTables(inAuthors, _memoryCache);
                return View(newAuthors);
            }
            else 
            {
                return View(inAuthors);
            }
                              
        }

        
        // Вызывается скриптом
        [HttpPost]
        public IActionResult AddBook(int authorId, Book book)
        {
            var message = _iODatabase.AddBook(authorId, book, _memoryCache);

            return Json(new
            {
                Message = message
            });

        }


        [HttpPost]
        public async Task<IActionResult> AddAuthor(Author newAuthor)
        {

            var message = await _iODatabase.AddAuthor(newAuthor);


            return Json(new
                   {
                       Message = message
                   });
        }
        






        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
