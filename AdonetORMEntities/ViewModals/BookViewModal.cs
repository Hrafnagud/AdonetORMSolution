using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdonetORMEntities.ViewModals
{
    public class BookViewModal
    {
        public int BookId { get; set; }
        public DateTime RegisterDate { get; set; }
        public string BookName { get; set; }
        public int Pages { get; set; }
        public bool IsPassive { get; set; }
        public int? GenreId { get; set; }
        public int AuthorId { get; set; }
        public int Stock { get; set; }

        public string AuthorFullName { get; set; }
        public string GenreName { get; set; }
    }
}
