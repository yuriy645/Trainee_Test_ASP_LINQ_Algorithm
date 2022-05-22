using _1_AuthorsBooks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_AuthorsBooks.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IODatabase _iODatabase;
        private readonly IMemoryCache _memoryCache;
        public AuthorController(IODatabase iODatabase, IMemoryCache memoryCache)
        {
            _iODatabase = iODatabase;
            _memoryCache = memoryCache;
        }
        //Авторы
        public async Task<IActionResult> Index()
        {
            var authors = await _iODatabase.GetAuthors();

            return View(authors);
        }
        //Смотреть автора первый раз
        public async Task<IActionResult> ViewAuthor(int authorId)
        {
            var author = await _iODatabase.ViewAuthorr(authorId);

            return View(author);
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
        public async Task<IActionResult> UpdateAuthorWithBooks(Author inAuthor)
        {
            Author newAuthor = null;
            if (ModelState.IsValid)
            {
                newAuthor = await _iODatabase.UpdateAuthorWithBooks(inAuthor, _memoryCache);
                return View("ViewAuthor", newAuthor);
            }
            else
            {
                return View("ViewAuthor", inAuthor);
            }
        }
        // Добавить автора
        [HttpPost]
        public async Task<IActionResult> AddAuthor(Author newAuthor)
        {
            //var authors = await _iODatabase.AddAuthor(newAuthor);
            var message = await _iODatabase.AddAuthor(newAuthor);
            //return View("NoIndex", authors); В таком варианте представление не вызывается
            return Json(new
            {
                Message = message
            });
        }
        //Удалить автора
        public async Task<IActionResult> DelAuthor(int authorId)
        {
            var authors = await _iODatabase.DelAuthorr(authorId);

            return View("Index", authors);
        }
    }
}
