using _1_AuthorsBooks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_AuthorsBooks.Controllers
{
    public class FirstNameController : Controller
    {
        private readonly IODatabase _iODatabase;
        public FirstNameController(IODatabase iODatabase)
        {
            _iODatabase = iODatabase;
        }
        //Книги
        public async Task<IActionResult> Index()
        {
            var firstNames = await _iODatabase.GetFirstNames();
            return View(firstNames);
        }
    }
}
