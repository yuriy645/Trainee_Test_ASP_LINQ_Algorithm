using _1_AuthorsBooks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_AuthorsBooks.Controllers
{    public class BookController : Controller
    {
        private readonly IODatabase _iODatabase;
        private readonly IMemoryCache _memoryCache;
        public BookController(IODatabase iODatabase, IMemoryCache memoryCache)
        {
            _iODatabase = iODatabase;
            _memoryCache = memoryCache;
        }
        //Книги
        public async Task<IActionResult> Index()
        {
            var books = await _iODatabase.GetBooks();
            return View(books);
        }
    }
}
