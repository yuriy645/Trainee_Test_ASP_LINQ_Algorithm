using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_AuthorsBooks.Models
{
    public class IODatabase
    {
        public async Task<List<Author>> GetAuthors()
        {
            using (var db = new TestContext())
            {
                var books = await db.Books
                    .Include(p => p.Genre)
                    .ToListAsync();

                var authors = await db.Authors
                    .Include(p => p.FirstName)
                    .Include(p => p.Patronymic)
                    .Include(p => p.AuthorsBooks)
                    .ToListAsync();

                return authors;
            }
        }


        public string AddBook(int authorId, Book book, IMemoryCache _memoryCache)
        {

            if (!_memoryCache.TryGetValue("cacheContainer", out object cacheContainer)) //если нет кэша, то создаем, добавляем книгу и сохраняем
            {
                CacheContainer cacheContainerT = new();
                cacheContainerT.CacheBooks = new();
                cacheContainerT.CacheBooks.Add(new CacheBook() { AuthorId = authorId, BookName = book.BookName, GenreName = book.Genre.GenreName, Pages = book.Pages });

                _memoryCache.Set("cacheContainer", cacheContainerT,
                new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromSeconds(600)
                });
            }
            else                                                                        //если есть кэш- то только добавляем книгу и сохраняем
            {
                var cacheContainerT = cacheContainer as CacheContainer;

                cacheContainerT.CacheBooks.Add(new CacheBook() { AuthorId = authorId, BookName = book.BookName, GenreName = book.Genre.GenreName, Pages = book.Pages });

                _memoryCache.Set("cacheContainer", cacheContainerT,
                new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromSeconds(600)
                });
            }   

                

            return "The book has been added to the cache";
            
        }
        

        public async Task<List<Author>> UpdateTables(List<Author> inAuthors, IMemoryCache _memoryCache)
        {
                                     // СОХРАНЕНИЕ АВТОРОВ

            List<Author> outAuthors = null;

            using (var db = new TestContext())
            {
                var outBooks = await db.Books
                    .Include(p => p.Genre)
                    .ToListAsync();

                var outFirstNames = await db.FirstNames
                    .ToListAsync();

                var outPatronymics = await db.Patronymics
                    .ToListAsync();

                outAuthors = await db.Authors
                    //.Include(p => p.FirstName)
                    //.Include(p => p.Patronymic)
                    .Include(p => p.AuthorsBooks)
                    .ToListAsync();

                bool authorCganges = false;

                for (int i = 0; i < inAuthors.Count; i++)
                {
                    authorCganges = false;
                    //SECOND NAME
                    //если в старом списке не так, как в новом
                    if (outAuthors[i].AuthorSecondName != inAuthors[i].AuthorSecondName)
                    {
                        //то сделать в саром так, как в новом
                        outAuthors[i].AuthorSecondName = inAuthors[i].AuthorSecondName;
                        authorCganges = true;
                        //db.Authors.Attach(outAuthors[i]);
                        //await db.SaveChangesAsync();
                    }

                    //FIRST NAME
                    //если в старом списке не так, как в новом
                    if (outAuthors[i].FirstName.FirstName1 != inAuthors[i].FirstName.FirstName1)
                    {
                        //то сделать в саром так, как в новом

                        //если новое имя уже есть в отдельной таблице
                        var inFirstName = outFirstNames
                            .Where(p => p.FirstName1 == inAuthors[i].FirstName.FirstName1)
                            .FirstOrDefault();
                        if (inFirstName != null)
                        {
                            // то подставляем в автора Id имени из отдельной таблицы
                            outAuthors[i].FirstNameId = inFirstName.FirstNameId;
                        }
                        else
                        { // иначе добавляем новое имя в отдельную табл,
                          // а потом подставляем в автора Id нового имени

                            await db.FirstNames.AddAsync(new FirstName() { FirstName1 = inAuthors[i].FirstName.FirstName1 });
                            await db.SaveChangesAsync();

                            var outFirstName = await db.FirstNames
                                .Where(p => p.FirstName1 == inAuthors[i].FirstName.FirstName1)
                                .FirstOrDefaultAsync();
                            outAuthors[i].FirstNameId = outFirstName.FirstNameId;
                            authorCganges = true;
                            //db.Authors.Attach(outAuthors[i]);
                            //await db.SaveChangesAsync();
                        }
                    }

                    //THIRD NAME

                    //если в старом списке не так, как в новом
                    if (outAuthors[i].Patronymic.Patronymic1 != inAuthors[i].Patronymic.Patronymic1)
                    {
                        //то сделать в саром так, как в новом

                        //если новое имя уже есть в отдельной таблице
                        var inPatronymic = outPatronymics
                            .Where(p => p.Patronymic1 == inAuthors[i].Patronymic.Patronymic1)
                            .FirstOrDefault();
                        if (inPatronymic != null)
                        {
                            // то подставляем в автора Id имени из отдельной таблицы
                            outAuthors[i].PatronymicId = inPatronymic.PatronymicId;
                        }
                        else
                        { // иначе добавляем новое имя в отдельную табл,
                          // а потом подставляем в автора Id нового имени

                            await db.Patronymics.AddAsync(new Patronymic() { Patronymic1 = inAuthors[i].Patronymic.Patronymic1 });
                            await db.SaveChangesAsync();

                            var outPatronymic = await db.Patronymics
                                .Where(p => p.Patronymic1 == inAuthors[i].Patronymic.Patronymic1)
                                .FirstOrDefaultAsync();
                            outAuthors[i].PatronymicId = outPatronymic.PatronymicId;
                            authorCganges = true;
                            //db.Authors.Attach(outAuthors[i]);
                            //await db.SaveChangesAsync();

                        }
                    }

                    if (authorCganges)
                    {
                        db.Authors.Attach(outAuthors[i]);
                        await db.SaveChangesAsync();
                    }
                }



                                                                 // СОХРАНЕНИЕ КНИГ

                 Genre newGenre = null;
                 Book newBook = null;
                 AuthorsBook newAuthorsBook = null;
                 List<CacheBook> cacheBooks = null;

                 // Извлечение контекста из кэша
                 if (_memoryCache.TryGetValue("cacheContainer", out object cacheContainer)) //если нет кэша, то создаем, добавляем книгу и сохраняем
                 {
                     var cacheContainerT = cacheContainer as CacheContainer;

                     cacheBooks = cacheContainerT.CacheBooks;
                 }

                 foreach (var cacheBook in cacheBooks)
                 {

                     // можно сделать транзакцию, чтобы исключить появление в БД жанров и книг, которые нигде не используются
                     using (var transaction = db.Database.BeginTransaction())
                     {
                         try
                         {
                             // табл. Genres
                             var oldGenre = db.Genres
                                 .Where(b => b.GenreName == cacheBook.GenreName)
                                 .FirstOrDefault();

                             int newGenreId = 0;

                             if (oldGenre == null)  // Если нет такого жанра в табл жанров, то добавляем его
                                    {
                                        //определение GenreId новой записи
                                        //newGenreId = await db.Genres
                                        //                         .MaxAsync(p => p.GenreId)
                                        //                         + 1;

                                        newGenre = new Genre() { GenreName = cacheBook.GenreName };
                                        await db.Genres.AddAsync(newGenre);
                                        await db.SaveChangesAsync();

                                        newGenre = db.Genres
                                        .Where(b => b.GenreName == cacheBook.GenreName)
                                        .FirstOrDefault();
                                        newGenreId = newGenre.GenreId;
                                    }
                             else
                                    {
                                        newGenreId = oldGenre.GenreId;
                                    }

                             // табл. Books
                             var oldBook = db.Books
                                 .Where(b => b.BookName == cacheBook.BookName)
                                 .Where(b => b.Genre.GenreName == cacheBook.GenreName)
                                 .Where(b => b.Pages == cacheBook.Pages)
                                 .FirstOrDefault();

                             int newBookId = 0;
                             if (oldBook == null)  // Если нет такой книги в табл книг , то добавляем её
                                    {
                                        //определение BookId новой записи
                                        //newBookId = await db.Books
                                        //                      .MaxAsync(p => p.BookId)
                                        //                      + 1;
                                        // С этим способом получилась проблема : если транзакция откатывает изменения, то
                                        // фактический Id назначается как будто не было этих удалений.

                                        newBook = new Book() { BookName = cacheBook.BookName, GenreId = newGenreId, Pages = cacheBook.Pages };
                                        await db.Books.AddAsync(newBook); // в Books появляется после SaveChanges
                                        await db.SaveChangesAsync();

                                        newBook = db.Books
                                        .Where(b => b.BookName == cacheBook.BookName)
                                        .Where(b => b.GenreId == newGenreId)
                                        .Where(b => b.Pages == cacheBook.Pages)
                                        .FirstOrDefault();
                                        newBookId = newBook.BookId;
                                    }
                             else
                                    {
                                        newBookId = oldBook.BookId;
                                    }

                             //табл.AuthorsBooks
                             var oldAuthorsBook = db.AuthorsBooks             // поиск Id кникги у этого автора
                                .Where(b => b.AuthorId == cacheBook.AuthorId)
                                .Where(b => b.BookId == newBookId)
                                .FirstOrDefault();

                             if (oldAuthorsBook == null) // Если такая книга не приписана этому автору, то приписываем
                                    {
                                        newAuthorsBook = new AuthorsBook() { AuthorId = cacheBook.AuthorId, BookId = newBookId };
                                        await db.AuthorsBooks.AddAsync(newAuthorsBook);
                                        await db.SaveChangesAsync();
                                    }

                             
                             transaction.Commit();
                         }

                         catch (Exception ex)
                         {
                             transaction.Rollback();
                             //messages.Add($"Exception Message: {ex.Message}, {ex.InnerException}, {ex.Data} ");
                         }
                     }
                 }
                    


            }
            return outAuthors;
        }

        public async Task<string> AddAuthor(Author newAuthor)
        {
            string message = null;
            using(var db = new TestContext()) 
            {
                // табл. Authors
                var oldAuthor = await db.Authors
                    .Where(b => b.FirstName.FirstName1 == newAuthor.FirstName.FirstName1)
                    .Where(b => b.AuthorSecondName == newAuthor.AuthorSecondName)
                    .Where(b => b.Patronymic.Patronymic1 == newAuthor.Patronymic.Patronymic1)
                    .FirstOrDefaultAsync();
                if (oldAuthor != null)
                {
                    message = "This author is already in the database";
                    return message;
                }

                // табл. FirstNames
                var oldFirstName = await db.FirstNames
                    .Where(b => b.FirstName1 == newAuthor.FirstName.FirstName1)
                    .FirstOrDefaultAsync();

                int newFirstNameId = 0;

                if (oldFirstName == null)  // Если нет такого в табл, то добавляем
                {
                    await db.FirstNames.AddAsync(new FirstName() { FirstName1 = newAuthor.FirstName.FirstName1 });
                    await db.SaveChangesAsync();

                    var newFirstName = await db.FirstNames
                    .Where(b => b.FirstName1 == newAuthor.FirstName.FirstName1)
                    .FirstOrDefaultAsync();
                    newFirstNameId = newFirstName.FirstNameId;
                }
                else
                {
                    newFirstNameId = oldFirstName.FirstNameId;
                }


                // табл. Patronymics
                var oldPatronymic = await db.Patronymics
                    .Where(b => b.Patronymic1 == newAuthor.Patronymic.Patronymic1)
                    .FirstOrDefaultAsync();

                int newPatronymicId = 0;

                if (oldPatronymic == null)  // Если нет такого в табл, то добавляем
                {
                    await db.Patronymics.AddAsync(new Patronymic() { Patronymic1 = newAuthor.Patronymic.Patronymic1 });
                    await db.SaveChangesAsync();

                    var newPatronymic = await db.Patronymics
                    .Where(b => b.Patronymic1 == newAuthor.Patronymic.Patronymic1)
                    .FirstOrDefaultAsync();
                    newPatronymicId = newPatronymic.PatronymicId;
                }
                else
                {
                    newPatronymicId = oldPatronymic.PatronymicId;
                }

                // табл. Authors
                if (oldAuthor == null)  // Если нет такого сочетания ФИО в табл , то добавляем его
                {
                    db.Authors.Add(new Author() { AuthorSecondName = newAuthor.AuthorSecondName, FirstNameId = newFirstNameId, PatronymicId = newPatronymicId });
                    await db.SaveChangesAsync();

                    message = "The author has been added to the database. Refresh the form to see the new author.";

                }

                // табл. AuthorsBooks
                // заполняется при добавлении книг
            }
            return message;
        }

    }
}
