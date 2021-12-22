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
                    _mySqlDBConnection = new SqlConnection("Server=DESKTOP-TUMHS1A; Database=SCHOOLLIBRARY; Trusted_Connection=True;");
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
                    object theObject = rowitem[propertyitem.Name];
                    if (theObject != null && theObject.ToString().Length > 0)
                    {
                        propertyitem.SetValue(myET, theObject);
                    }
                }

                list.Add(myET);

            }


            return list;
        }

        public static ET ToET<ET>(this DataTable dataTable) where ET : class, new()
        {
            Type theType = typeof(ET);
            ET entity = new ET();
            PropertyInfo[] propertyInfos = theType.GetProperties();

            foreach (DataRow rowItem in dataTable.Rows)
            {
                foreach (var propertyItem in propertyInfos)
                {
                    object theObject = rowItem[propertyItem.Name];
                    if (theObject != null && theObject.ToString().Length > 0)
                    {
                        propertyItem.SetValue(entity, theObject);
                    }
                }
            }

            return entity;
        }
    }
}
