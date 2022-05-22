using _1_AuthorsBooks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_AuthorsBooks.Controllers
{
    public class ThirdNameController : Controller
    {
        private readonly IODatabase _iODatabase;
        public ThirdNameController(IODatabase iODatabase)
        {
            _iODatabase = iODatabase;
        }
        //Книги
        public async Task<IActionResult> Index()
        {
            var thirdNames = await _iODatabase.GetThirdNames();
            return View(thirdNames);
        }
    }
}
