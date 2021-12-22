using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdonetORMCommon;
namespace AdonetORMEntities.Entities
{
    [Table(TableName = "Authors", PrimaryColumn = "AuthorId", IdentityColumn = "AuthorId")]
    public class Author
    {
        public int AuthorId { get; set; }
        public DateTime RegisterDate { get; set; }
        public string AuthorFullName { get; set; }
        public bool IsPassive{ get; set; }
    }
}
