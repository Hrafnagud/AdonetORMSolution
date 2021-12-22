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
    public class ORMBase<ET, OT> : IORM<ET>
        where ET : class, new()
        where OT : class, new()
    {
        public Type ETType
        {
            get
            {
                return typeof(ET);
            }
        }

        public Table TheTable
        {
            get
            {
                var attributes = ETType.GetCustomAttributes(typeof(Table), false);
                if (attributes != null && attributes.Any())
                {
                    Table theTable = (Table)attributes[0];  //Zero index is to receive TableName property from Table.cs
                    return theTable;
                }
                return null;
            }
        }

        private static OT _current;
        
        public static OT Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new OT();
                }
                return _current;
            }
        }

        public bool Delete(ET Entity)
        {
            int theID = 0;
            PropertyInfo[] properties = ETType.GetProperties();
            foreach (var item in properties)
            {
                if (item.Name == TheTable.PrimaryColumn)
                {
                    theID = (int)item.GetValue(Entity);
                }
            }


            string query = $"delete from {TheTable.TableName} where {TheTable.PrimaryColumn} = {theID}";
            //First Approach
            //SqlCommand command = new SqlCommand(query, Tools.MySqlDBConnection);
            //int affectedRows = command.ExecuteNonQuery();
            //Tools.MySqlDBConnection.Close();

            //Second Approach
            int affectedRows = 0;
            using (Tools.MySqlDBConnection)
            {
                SqlCommand command = new SqlCommand(query, Tools.MySqlDBConnection);
                Tools.MySqlDBConnection.Open();
                affectedRows = command.ExecuteNonQuery();
            }

            if (affectedRows > 0)
            {

                return true;
            }
            else
            {

                return false;
            }

            return true;
        }

        public bool Insert(ET Entity)
        {
            string query = "";
            string tableName = "";
            string properties = "";
            string values = "";

            PropertyInfo[] propertyArray = ETType.GetProperties();
            SqlCommand command = new SqlCommand();

            // Columns should be formatted for insert into query.
            foreach (var item in propertyArray)
            {
                if (item.Name == TheTable.IdentityColumn)
                {
                    continue;      //e.g bookId has an increment attribute. Thus we don't insert identity value for insert into operation for db tables.
                }
                else
                {
                    properties += item.Name + ",";  //BookName, Pages, IsPassive etc..
                }
            }   //insert into (properties)
            properties = properties.TrimEnd(',');   //Trim the comma at the end of the properties.

            //we need to handle values() part now
            foreach (var item in propertyArray)
            {
                if (item.Name !=  TheTable.IdentityColumn)
                {
                    if (item.GetValue(Entity) == null)
                    {
                        values += "null, ";
                    }
                    else if (item.PropertyType.Name.Contains("DateTime"))
                    {
                        DateTime dateTime;
                        DateTime.TryParse(item.GetValue(Entity).ToString(), out dateTime);
                        string dateTimeString = $"'{dateTime.ToString("yyyy-MM-dd HH:mm:ss")}'";
                        values += dateTimeString + ",";
                    }

                    else if (item.PropertyType.Name.Contains("Bool"))
                    {
                        bool value = (bool)item.GetValue(Entity);
                        string valueString = value ? "1" : "0";
                        values += valueString + ",";
                    }
                    else if (item.PropertyType.Name.Contains("String"))
                    {
                        //'Crime and Punishment',
                        values += $"'{item.GetValue(Entity)}',";
                    }
                    else
                    {
                        values += item.GetValue(Entity) + ",";
                    }
                }
            }
            values = values.TrimEnd(',');

            tableName = TheTable.TableName;
            query = $"insert into {tableName} ({properties}) values ({values})";

            command.CommandType = CommandType.Text;
            command.CommandText = query;
            command.Connection = Tools.MySqlDBConnection;
            Tools.MySqlDBConnection.Open();
            int affectedRows = command.ExecuteNonQuery();
            Tools.MySqlDBConnection.Close();

            if (affectedRows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            SqlCommand command = new SqlCommand(querySentence, Tools.MySqlDBConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            return dataTable.ToList<ET>();
        }

        public bool Update(ET Entity)
        {
            throw new NotImplementedException();
        }

        public ET SelectET(int etID)
        {
            string query = string.Empty;
            var attributes = ETType.GetCustomAttributes(typeof(Table), false);
            if (attributes != null && attributes.Any())
            {
                Table table = attributes[0] as Table;
                query = $"select * from {table.TableName} where {table.PrimaryColumn} = {etID}";
            }

            DataTable dataTable = new DataTable();

            using (Tools.MySqlDBConnection)
            {
                SqlCommand commadn = new SqlCommand(query, Tools.MySqlDBConnection);
                Tools.MySqlDBConnection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(commadn);
                adapter.Fill(dataTable);
            }
            return dataTable.ToET<ET>();
        }



    }
}
