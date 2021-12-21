using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdonetORMCommon
{
    public class ORMBase<ET, OT> : IORM<ET>
        where ET : class, new()
        where OT : class, new()
    {
        public bool Delete(ET Entity)
        {
            throw new NotImplementedException();
        }

        public bool Insert(ET Entity)
        {
            throw new NotImplementedException();
        }

        public List<ET> Select()
        {
            Type type = typeof(ET); //Entity Type
            string querySentence = "Select * from ";
            var attributes = type.GetCustomAttributes(typeof(Table), false);
            if (attributes != null && attributes.Any())
            {
                Table table = (Table)attributes[0];
                querySentence += table.TableName;
            }
            SqlCommand command= new SqlCommand(querySentence, Tools.MySqlDBConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            return dataTable.ToList<ET>();
        }

        public bool Update(ET Entity)
        {
            throw new NotImplementedException();
        }
    }
}
