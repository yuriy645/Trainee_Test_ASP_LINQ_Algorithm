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
        public async Task<List<Genre>> GetGenres()
        {
            using (var db = new TestContext())
            {
                var genres = await db.Genres
                    .ToListAsync();
                return genres;
            }
        }
        public async Task<List<Book>> GetBooks()
        {
            using (var db = new TestContext())
            {
                var books = await db.Books
                    .ToListAsync();
                return books;
            }
        }
        public async Task<List<Patronymic>> GetThirdNames()
        {
            using (var db = new TestContext())
            {
                var thirdNames = await db.Patronymics
                    .ToListAsync();
                return thirdNames;
            }
        }
        public async Task<List<FirstName>> GetFirstNames()
        {
            using (var db = new TestContext())
            {
                var firstNames = await db.FirstNames
                    .ToListAsync();
                return firstNames;
            }
        }

        public async Task<List<AuthorsBook>> GetAuthorsBooks()
        {
            using (var db = new TestContext())
            {
                var authorsBooks = await db.AuthorsBooks
                    .ToListAsync();
                var books = await db.Books
                    .ToListAsync();
                var authors = await db.Authors
                    .ToListAsync();
                return authorsBooks;
            }
        }
        public async Task<Author> ViewAuthorr(int authorId)
        {
            using (var db = new TestContext())
            {
                // Загрузить автора
                var author = await db.Authors
                    .Include(p => p.FirstName)
                    .Include(p => p.Patronymic)
                    .Include(p => p.AuthorsBooks)
                    .Where(p => p.AuthorId == authorId)
                    .FirstOrDefaultAsync();
                // Загрузить все книги этого автора
                var authorsBooks = await db.AuthorsBooks
                    .Include(p => p.Book)
                    .Include(p => p.Book.Genre)
                    .Where(p => p.AuthorId == authorId)
                   .ToListAsync();
                return author;
            }
        }
        public async Task<List<Author>> DelAuthorr(int authorId)
        {
            using (var db = new TestContext())
            {
                // Список книг удаляемого автора
                var assumeToDeleteAuthorsBooks = await db.AuthorsBooks
                                    .Include(p => p.Book)
                                    //.Include(p => p.Book.Genre)
                                    .Where(p => p.AuthorId == authorId)
                                    .ToListAsync();
                //// Список книг удаляемого автора
                var toDeleteAuthorsBooks = await db.AuthorsBooks
                                    .Where(p => p.AuthorId == authorId)
                                    .ToListAsync();
                // Список всех книг у авторов
                var allAuthorsBooks = await db.AuthorsBooks
                                    .Include(p => p.Book)
                                    //.Include(p => p.Book.Genre)
                                   .ToListAsync();
                // Список всех книг
                var allBooks = await db.Books
                    .ToListAsync();
                // Список всех жанров
                var allGenres = await db.Genres
                    .ToListAsync();
                // Удаляем соответствующие записи из табл. AuthorsBooks в любом случае
                db.AuthorsBooks.RemoveRange(toDeleteAuthorsBooks);
                await db.SaveChangesAsync();
                //Перебор книг, которые собираемся удалять
                foreach (var assumeToDeleteAuthorsBook in assumeToDeleteAuthorsBooks)
                {
                    // если нет той же книги с другим автором в общем списке
                    var existsList = allAuthorsBooks
                        .Where(p => p.BookId == assumeToDeleteAuthorsBook.BookId)
                        .Where(p => p.AuthorId != authorId)
                        .ToList();
                    if (existsList.Count == 0)
                    { 
                        // то удалить эту книгу
                        // GenreId to delete
                        int assumeToDeleteGenreId = assumeToDeleteAuthorsBook.Book.GenreId;
                        db.Books.Remove(assumeToDeleteAuthorsBook.Book); //После SaveChangesAsync уменьшилось в Books
                        await db.SaveChangesAsync();
                        // Если не нашлось других книг с таким жанром, то и жанр удаляем
                        var existsBooksList = allBooks
                             .Where(p => p.GenreId == assumeToDeleteGenreId);
                        if (existsBooksList.Count() == 1)
                        {
                            var genreToDelete = allGenres
                                 .Where(p => p.GenreId == assumeToDeleteGenreId)
                                 .FirstOrDefault();
                            db.Genres.Remove(genreToDelete);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                // Удаляемый автор
                var toDeleteAuthor = await db.Authors
                          .Where(p => p.AuthorId == authorId)
                          .FirstOrDefaultAsync();
                // Все авторы
                var allAuthors = await db.Authors
                          .ToListAsync();
                // FirstNameId to delete
                int assumeToDeleteFirstNameId = toDeleteAuthor.FirstNameId;
                // PatronymicId to delete
                int assumeToDeletePatronymicId = toDeleteAuthor.PatronymicId;
                // Удаляем соответствующую запись из табл. Authors в любом случае
                db.Authors.Remove(toDeleteAuthor);
                await db.SaveChangesAsync();
                // Если в табл. авторов нашелся только один автор с таким именем (которого мы удаляем),
                // то удаляем и в табл имён
                var withFirstNameAuthors = allAuthors
                    .Where(p => p.FirstNameId == assumeToDeleteFirstNameId);
                if (withFirstNameAuthors.Count() == 1)
                {
                    var toDeleteFirstName = await db.FirstNames
                        .Where(p => p.FirstNameId == assumeToDeleteFirstNameId)
                        .FirstOrDefaultAsync();
                    db.FirstNames.Remove(toDeleteFirstName);
                    await db.SaveChangesAsync();
                }
                // Если в табл. авторов нашелся только один автор с таким отчеством (которого мы удаляем),
                // то удаляем и в табл отчеств
                var withPatronymicAuthors = allAuthors
                    .Where(p => p.PatronymicId == assumeToDeletePatronymicId);
                if (withPatronymicAuthors.Count() == 1)
                {
                    var toDeletePatronymic = await db.Patronymics
                        .Where(p => p.PatronymicId == assumeToDeletePatronymicId)
                        .FirstOrDefaultAsync();
                    db.Patronymics.Remove(toDeletePatronymic);
                    await db.SaveChangesAsync();
                }
                // Загрузить автора
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
        public async Task<Author> UpdateAuthorWithBooks(Author inAuthor, IMemoryCache _memoryCache)
        {
                                     // СОХРАНЕНИЕ АВТОРA
            Author outAuthor = null;
            using (var db = new TestContext())
            {
                var outFirstNames = await db.FirstNames
                    .ToListAsync();
                var outPatronymics = await db.Patronymics
                    .ToListAsync();
                var authorsBooks = await db.AuthorsBooks
                    .Include(p => p.Book)
                    .Include(p => p.Book.Genre)
                    .Where(p => p.AuthorId == inAuthor.AuthorId)
                    .ToListAsync();
                outAuthor = await db.Authors
                    .Where(p => p.AuthorId == inAuthor.AuthorId)
                    .FirstOrDefaultAsync();
                bool authorCganges = false;               
                    authorCganges = false;
                    //SECOND NAME
                    //если в старом списке не так, как в новом
                    if (outAuthor.AuthorSecondName != inAuthor.AuthorSecondName)
                    {
                        //то сделать в саром так, как в новом
                        outAuthor.AuthorSecondName = inAuthor.AuthorSecondName;
                        authorCganges = true;
                        //db.Authors.Attach(outAuthors[i]);
                        //await db.SaveChangesAsync();
                    }

                    //FIRST NAME
                    //если в старом списке не так, как в новом
                    if (outAuthor.FirstName.FirstName1 != inAuthor.FirstName.FirstName1)
                    {
                        //то сделать в саром так, как в новом
                        //если новое имя уже есть в таблице имён
                        var inFirstName = outFirstNames
                            .Where(p => p.FirstName1 == inAuthor.FirstName.FirstName1)
                            .FirstOrDefault();
                        if (inFirstName != null)
                        {
                            // то подставляем в автора Id имени из таблицы имён
                            outAuthor.FirstNameId = inFirstName.FirstNameId;
                        }
                        else
                        { // иначе добавляем новое имя в отдельную табл,
                          // а потом подставляем в автора Id нового имени
                            await db.FirstNames.AddAsync(new FirstName() { FirstName1 = inAuthor.FirstName.FirstName1 });
                            await db.SaveChangesAsync();
                            var outFirstName = await db.FirstNames
                                .Where(p => p.FirstName1 == inAuthor.FirstName.FirstName1)
                                .FirstOrDefaultAsync();
                            outAuthor.FirstNameId = outFirstName.FirstNameId;
                            authorCganges = true;
                            //db.Authors.Attach(outAuthors[i]);
                            //await db.SaveChangesAsync();
                        }
                    }
                    //THIRD NAME
                    //если в старом списке не так, как в новом
                    if (outAuthor.Patronymic.Patronymic1 != inAuthor.Patronymic.Patronymic1)
                    {
                        //то сделать в саром так, как в новом
                        //если новое имя уже есть в отдельной таблице
                        var inPatronymic = outPatronymics
                            .Where(p => p.Patronymic1 == inAuthor.Patronymic.Patronymic1)
                            .FirstOrDefault();
                        if (inPatronymic != null)
                        {
                            // то подставляем в автора Id имени из отдельной таблицы
                            outAuthor.PatronymicId = inPatronymic.PatronymicId;
                        }
                        else
                        { // иначе добавляем новое имя в отдельную табл,
                          // а потом подставляем в автора Id нового имени
                            await db.Patronymics.AddAsync(new Patronymic() { Patronymic1 = inAuthor.Patronymic.Patronymic1 });
                            await db.SaveChangesAsync();
                            var outPatronymic = await db.Patronymics
                                .Where(p => p.Patronymic1 == inAuthor.Patronymic.Patronymic1)
                                .FirstOrDefaultAsync();
                            outAuthor.PatronymicId = outPatronymic.PatronymicId;
                            authorCganges = true;
                            //db.Authors.Attach(outAuthors[i]);
                            //await db.SaveChangesAsync();
                        }
                    }
                    if (authorCganges)
                    {
                        db.Authors.Attach(outAuthor);
                        await db.SaveChangesAsync();
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
                if (cacheBooks != null)
                {
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
                                var oldAuthorsBook = await db.AuthorsBooks             // поиск Id кникги у этого автора
                                   .Where(b => b.AuthorId == cacheBook.AuthorId)
                                   .Where(b => b.BookId == newBookId)
                                   .FirstOrDefaultAsync();

                                if (oldAuthorsBook == null) // Если такая книга не приписана этому автору, то приписываем
                                {
                                    await db.AuthorsBooks.FirstOrDefaultAsync();
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
            }
            return outAuthor;
        }
        public async Task</*List<Author>*/string> AddAuthor(Author newAuthor)
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
                    //return message;
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
                    message = "The author has been added to the database. Refresh the page to see the new author.";
                }
                // табл. AuthorsBooks
                // заполняется при добавлении книг
                // Вариант с возвращением списка авторов
                //var newAuthors = await db.Authors
                //    .Include(p => p.FirstName)
                //    .Include(p => p.Patronymic)
                //    .Include(p => p.AuthorsBooks)
                //    .ToListAsync();
                //return newAuthors;
            }
            return message;
        }

    }
}
