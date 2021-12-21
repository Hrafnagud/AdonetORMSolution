using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdonetORMCommon
{
    public static class Tools
    {
        private static SqlConnection _mySqlDBConnection;
        public static SqlConnection MySqlDBConnection {
            get
            {
                if (_mySqlDBConnection == null)
                {
                    _mySqlDBConnection = new SqlConnection("Server=DESKTOP-TUMHS1A; Database=NORTHWND; Trusted_Connection=True;");
                }
                return _mySqlDBConnection;
            }

            set
            {
                _mySqlDBConnection = value;
            }
        }
        public static List<ET> ToList<ET>(this DataTable dt) where ET : class, new()
        {
            Type type = typeof(ET);
            List<ET> list = new List<ET>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (DataRow rowitem in dt.Rows)
            {
                ET myET = new ET();

                foreach (PropertyInfo propertyitem in propertyInfos)
                {
                    object o = rowitem[propertyitem.Name];
                    if (o != null)
                    {
                        propertyitem.SetValue(myET, o);
                    }
                }

                list.Add(myET);

            }


            return list;
        }
    }
}
