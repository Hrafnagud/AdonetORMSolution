using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdonetORMCommon;
namespace AdonetORMEntities.Entities
{
    [Table(TableName = "Genre", IdentityColumn = "GenreId", PrimaryColumn = "GenreId")]
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
