using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdonetORMCommon;
using AdonetORMEntities.Entities;
using AdonetORMEntities.ViewModals;

namespace AdonetORM_BLL
{
    public class BooksORM : ORMBase<Book, BooksORM>
    {
        GenreORM myGenreORM = new GenreORM();
        AuthorsORM myAuthorsORM = new AuthorsORM();

        public List<BookViewModal> BringBooksWithViewModal()
        {
            try
            {
                List<BookViewModal> returnList = new List<BookViewModal>();

                List<Book> books = this.Select();   //All
                var authors = myAuthorsORM.Select();    //approaches ar
                List<Genre> genre = myGenreORM.Select();    //valid.

                foreach (Book item in books)
                {
                    BookViewModal book = new BookViewModal()
                    {
                        BookId = item.BookId,
                        BookName = item.BookName,
                        RegisterDate = item.RegisterDate,
                        Pages = item.Pages,
                        IsPassive = item.IsPassive,
                        Stock = item.Stock,
                        AuthorId = item.AuthorId
                    };

                    book.AuthorFullName = authors.Find(x => x.AuthorId == item.AuthorId)?.AuthorFullName;
                    book.GenreId = genre.Find(x => x.GenreId == item.GenreId)?.GenreId;
                    book.GenreName = genre.Find(x => x.GenreId == item.GenreId)?.GenreName;
                    returnList.Add(book);
                }

                return returnList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
