using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdonetORMCommon;
namespace AdonetORMEntities.Entities
{
    [Table(TableName = "Books", PrimaryColumn = "BookId", IdentityColumn = "BookId")]
    public class Book
    {
        public int BookId { get; set; }
        public DateTime RegisterDate { get; set; }
        public string BookName { get; set; }
        public int Pages { get; set; }
        public bool IsPassive { get; set; }
        public int? GenreId { get; set; }
        public int AuthorId { get; set; }
        public int Stock { get; set; }
    }
}
