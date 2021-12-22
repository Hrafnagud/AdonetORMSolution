using AdonetORMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdonetORMEntities.Entities
{
    [Table(TableName = "LoanBook", PrimaryColumn = "OperationId", IdentityColumn = "OperationId")]
    public class LoanBook
    {
        public int OperationId { get; set; }
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanStarts { get; set; }
        public DateTime LoanEnds { get; set; }
        public bool IsReturnedool { get; set; }
    }
}
