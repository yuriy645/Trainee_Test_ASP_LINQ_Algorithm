using _1_AuthorsBooks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_AuthorsBooks.Controllers
{
    public class AuthorsBookController : Controller
    {        
        private readonly IODatabase _iODatabase;        
        public AuthorsBookController(IODatabase iODatabase)
        {
            _iODatabase = iODatabase;
        }        
        //Авторы c книгами
        public async Task<IActionResult> Index()
        {
            var authorsBooks = await _iODatabase.GetAuthorsBooks();        
            return View(authorsBooks);
        }        
    }
}
