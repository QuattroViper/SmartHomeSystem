using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataHandler
    {

        private string connectionStringSHS;
        private string providerName;
        DataTable mainSchema;

        /// <summary>
        /// 
        /// </summary>
        //private static DataHandler instance = new DataHandler();

        //public static DataHandler getInstance(DataHandlerHelper helper)
        //{

        //    return instance;
        //}

        public DataHandler(DataHandlerHelper helper)
        {
            if (helper.ConnectionToUse == DataHandlerHelper.Connection.ConnectionOne)
            {
                connectionStringSHS = helper.ConnectionStringOne;
            }
            else if (helper.ConnectionToUse == DataHandlerHelper.Connection.ConnecitonTwo)
            {
                connectionStringSHS = helper.ConnectionStringTwo;
            }
            this.providerName = helper.ProviderName;
        }

        public DataTable getSchema()
        {
            using (SqlConnection connection = new SqlConnection(connectionStringSHS))
            {
                 
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                connection.InfoMessage += new SqlInfoMessageEventHandler(onInfoMessage);
                connection.StateChange += new StateChangeEventHandler(OnStateChange);

                mainSchema = connection.GetSchema("Tables");
                //mainSchema = connection.GetSchema("Tables", new string[] { "tblProduct" });
                //mainSchema = connection.GetSchema(connection.Database, new string[] { null, null, null, "databases" });
            }
           

            return mainSchema;
        }

        DataTable dynamicQueries<T>(QueryType queryType, object objectClass, KeyValuePair<string, string[,]> table) {
            //return dalk n Datatable. As dit insert, delete, of update is, return null

            DataTable returnTable = null;

            try
            {
                StringBuilder queryString = new StringBuilder();
                queryString.Append("SELECT ");
               
                for (int i = 0; i < table.Value.GetLength(0); i++)
                {
                    if (i != table.Value.GetLength(0)-1)
                    {
                        queryString.Append(table.Value[i, 0] + ", ");
                    }
                    else
                    {
                        queryString.Append(table.Value[i, 0] + " ");
                    }
                }

                queryString.Append("FROM ");
                queryString.Append(table.Key);

                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.log(queryString.ToString());

                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connectionStringSHS;

                // Create the DbCommand.
                DbCommand command = factory.CreateCommand();
                command.CommandText = queryString.ToString();
                command.Connection = connection;

                DbDataAdapter adapter = factory.CreateDataAdapter();
                adapter.SelectCommand = command;

                if (queryType == QueryType.SELECT)
                {
                    // Fill the DataTable.
                    returnTable = new DataTable();
                    adapter.Fill(returnTable);

                    return returnTable;
                }

                if (queryType != QueryType.SELECT)
                {                  

                    // Create the DbCommandBuilder.
                    DbCommandBuilder builder = factory.CreateCommandBuilder();
                    builder.DataAdapter = adapter;

                    // Cast it to itself
                    T spesificClass = (T)Convert.ChangeType(objectClass, typeof(T));

                    if (queryType == QueryType.INSERT)
                    {
                        // Get the insert commands.
                        adapter.InsertCommand = builder.GetInsertCommand();
                        System.Diagnostics.Debug.WriteLine(adapter.InsertCommand.CommandText);

                        // Fill the DataTable.
                        returnTable = new DataTable();
                        adapter.Fill(returnTable);

                        // Fill the DataTable.
                        //returnTable = new DataTable();
                        //adapter.Fill(returnTable);

                        // Insert a new row.
                        DataRow newRow = returnTable.NewRow();
                        //  newRow["CustomerID"] = "XYZZZ";

                        for (int i = 0; i < table.Value.GetLength(0); i++)
                        {
                            //PropertyInfo info = spesificClass.GetType().GetProperty(table.Value[i,1]);
                            //newRow[table.Value[i, 0]] = info.GetValue(spesificClass);
                            string dbColumn = table.Value[i, 0];
                            string propName = table.Value[i, 1];
                            var value = spesificClass.GetType().GetProperty(propName).GetValue(spesificClass);
                            newRow[dbColumn] = value;
                        }

                        returnTable.Rows.Add(newRow);
                   
                        adapter.Update(returnTable);

                        return returnTable;
                    } else

                    if (queryType == QueryType.UPDATE)
                    {
                        // Get the update commands.
                        adapter.UpdateCommand = builder.GetUpdateCommand();
                        System.Diagnostics.Debug.WriteLine(adapter.UpdateCommand.CommandText);

                        // Fill the DataTable.
                        returnTable = new DataTable();
                        adapter.Fill(returnTable);

                        // Edit an existing row.
                        //DataRow[] editRow = returnTable.Select("CustomerID = 'XYZZZ'");
                        //editRow[0]["CompanyName"] = "XYZ Corporation";

                        // Update the row based on GUID
                        DataRow[] editRow = returnTable.Select( string.Format("guid = '{0}'", spesificClass.GetType().GetProperty("GUID").GetValue(spesificClass) ));
                        //editRow[0]["CompanyName"] = "XYZ Corporation";
                        for (int i = 0; i < table.Value.GetLength(0); i++)
                        {
                            editRow[0][table.Value[i, 0]] = spesificClass.GetType().GetProperty(table.Value[i, 1]).GetValue(spesificClass);
                        }

                        adapter.Update(returnTable);

                        return returnTable;

                    } else

                    if (queryType == QueryType.DELETE)
                    {
                        // Get the delete commands.
                        adapter.DeleteCommand = builder.GetDeleteCommand();
                        System.Diagnostics.Debug.WriteLine(adapter.DeleteCommand.CommandText);

                        // Fill the DataTable.
                        returnTable = new DataTable();
                        adapter.Fill(returnTable);

                        // Delete a row.
                        DataRow[] deleteRow = returnTable.Select(string.Format("guid = '{0}'", spesificClass.GetType().GetProperty("GUID").GetValue(spesificClass)) );
                        foreach (DataRow row in deleteRow)
                        {
                            row.Delete();
                        }

                        adapter.Update(returnTable);

                        return returnTable;
                    } 


                }

                
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(ex,true,false,ex.ToString());
            }


            return returnTable;
        }

        DataTable dynamicSelectQuery<T>(object objectClass, object[] selectParameters = null)
        {
            //return dalk n Datatable. As dit insert, delete, of update is, return null

            DataTable returnTable = new DataTable();     

            try
            {

                // Cast it to itself
                T spesificClass = (T)Convert.ChangeType(objectClass, typeof(T));
                string tableName = (string)spesificClass.GetType().GetProperty("TableName").GetValue(spesificClass);
                SortedDictionary<string, string> valuePair = (SortedDictionary<string, string>)spesificClass.GetType().GetProperty("TableDataInfo").GetValue(spesificClass);

                StringBuilder queryString = new StringBuilder();
                queryString.Append("SELECT ");


                for (int i = 0; i < valuePair.Count; i++)
                {
                    if (i != valuePair.Count - 1)
                    {
                        queryString.Append(valuePair.Keys.ElementAt(i) + ", ");
                    }
                    else
                    {
                        queryString.Append(valuePair.Keys.ElementAt(i) + " ");
                    }
                }

                queryString.Append("FROM ");
                queryString.Append(tableName);




                // providerName = "System.Data.SqlClient"
                // connectionStringSHS = "Data Source=192.168.2.222;Initial Catalog=SmartHomeSystem;User ID=applicationLogin;Password=BelgiumCampus123."


                //connectionStringSHS = ConfigurationManager.ConnectionStrings["default"].ConnectionString);
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.log(queryString.ToString());

                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connectionStringSHS;

                using (connection)
                {
                    // Create the DbCommand.
                    DbCommand command = factory.CreateCommand();
                    command.CommandText = queryString.ToString();
                    //command.CommandText = "SELECT * FROM tblProduct";
                    command.Connection = connection;
                    connection.Open();

                    DbDataAdapter adapter = factory.CreateDataAdapter();
                    adapter.SelectCommand = command;

                    // Fill the DataTable.

                    adapter.Fill(returnTable);
                }




                return returnTable;


            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(ex, true, false, ex.ToString());
            }


            return returnTable;
        }

        public DataTable dynamicInsertQuery<T>(object objectClass)
        {
            DataTable returnTable = new DataTable();

            try
            {

                // Cast it to itself
                T spesificClass = (T)Convert.ChangeType(objectClass, typeof(T));
                string tableName = (string)spesificClass.GetType().GetProperty("TableName").GetValue(spesificClass);
                SortedDictionary<string, string> valuePair = (SortedDictionary<string, string>)spesificClass.GetType().GetProperty("TableDataInfo").GetValue(spesificClass);          

                // Create the DbProviderFactory and DbConnection.
                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connectionStringSHS;

                using (connection)
                {
                    string simpleQuery = "SELECT * FROM " + tableName;
                    // Create the select command.
                    DbCommand command = factory.CreateCommand();
                    command.CommandText = simpleQuery;
                    command.Connection = connection;

                    // Create the DbDataAdapter.
                    DbDataAdapter adapter = factory.CreateDataAdapter();
                    adapter.SelectCommand = command;

                    // Create the DbCommandBuilder.
                    DbCommandBuilder builder = factory.CreateCommandBuilder();
                    builder.DataAdapter = adapter;

                    // Get the insert, update and delete commands.
                    adapter.InsertCommand = builder.GetInsertCommand();

                    // Fill the DataTable.
                    //DataTable table = new DataTable();
                    adapter.Fill(returnTable);

                    // Insert a new row.
                    DataRow newRow = returnTable.NewRow();

                    for (int i = 0; i < valuePair.Count; i++)
                    {
                        // Get the table columnName from the sorted dictionary where i, the counter, is at in the valuePair, which is the list of properties and values at that specific point.
                        //string ColumnName = valuePair.FirstOrDefault(x => x.Value == "string, value").Key;
                        string columnName = valuePair.Keys.ElementAt(i);
                        string propertyName = valuePair.Values.ElementAt(i);
                        string valueAtProperty = "";
                        if (columnName.Contains("guid"))
                        {
                            valueAtProperty = spesificClass.GetType().GetProperty(propertyName).GetValue(spesificClass).ToString();
                        }
                        else
                        {
                            valueAtProperty = spesificClass.GetType().GetProperty(propertyName).GetValue(spesificClass).ToString();
                        }
                        

                        newRow[columnName] = valueAtProperty;
                    }

                    returnTable.Rows.Add(newRow);

                    adapter.Update(returnTable);

                }


            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true, false, exception.ToString());
            }



            return returnTable;
        }

        public DataTable dynamicUpdateQuery<T>(object objectClass)
        {
            DataTable returnTable = new DataTable();

            try
            {
                // Cast it to itself
                T spesificClass = (T)Convert.ChangeType(objectClass, typeof(T));
                string tableName = (string)spesificClass.GetType().GetProperty("TableName").GetValue(spesificClass);
                SortedDictionary<string, string> valuePair = (SortedDictionary<string, string>)spesificClass.GetType().GetProperty("TableDataInfo").GetValue(spesificClass);          

                // Create the DbProviderFactory and DbConnection.
                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connectionStringSHS;

                using (connection)
                {
                    string simpleQuery = "SELECT * FROM " + tableName;
                    // Create the select command.
                    DbCommand command = factory.CreateCommand();
                    command.CommandText = simpleQuery;
                    command.Connection = connection;

                    // Create the DbDataAdapter.
                    DbDataAdapter adapter = factory.CreateDataAdapter();
                    adapter.SelectCommand = command;

                    // Create the DbCommandBuilder.
                    DbCommandBuilder builder = factory.CreateCommandBuilder();
                    builder.DataAdapter = adapter;

                    // Get the insert, update and delete commands.
                    adapter.InsertCommand = builder.GetUpdateCommand();

                    // Fill the DataTable.
                    //DataTable table = new DataTable();
                    adapter.Fill(returnTable);

                    // edit a row.
                    DataRow[] editRow = returnTable.Select("guid = '" + (string)spesificClass.GetType().GetProperty("GUID").GetValue(spesificClass).ToString() + "'");
                    for (int i = 0; i < valuePair.Count; i++)
                    {
                        // Get the table columnName from the sorted dictionary where i, the counter, is at in the valuePair, which is the list of properties and values at that specific point.
                        string columnName = valuePair.Keys.ElementAt(i);
                        string propertyName = valuePair.Values.ElementAt(i);
                        string valueAtProperty = spesificClass.GetType().GetProperty(propertyName).GetValue(spesificClass).ToString();

                        editRow[0][columnName] = valueAtProperty;
                    }

                    adapter.Update(returnTable);


                }
            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true, false, exception.ToString());
            }

            return returnTable;
        }

        public DataTable dynamicDeleteQuery<T>(object objectClass)
        {
            DataTable returnTable = new DataTable();

            try
            {
                // Cast it to itself
                T spesificClass = (T)Convert.ChangeType(objectClass, typeof(T));
                string tableName = (string)spesificClass.GetType().GetProperty("TableName").GetValue(spesificClass);
                SortedDictionary<string, string> valuePair = (SortedDictionary<string, string>)spesificClass.GetType().GetProperty("TableDataInfo").GetValue(spesificClass);

                // Create the DbProviderFactory and DbConnection.
                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connectionStringSHS;

                using (connection)
                {
                    string simpleQuery = "SELECT * FROM " + tableName;
                    // Create the select command.
                    DbCommand command = factory.CreateCommand();
                    command.CommandText = simpleQuery;
                    command.Connection = connection;

                    // Create the DbDataAdapter.
                    DbDataAdapter adapter = factory.CreateDataAdapter();
                    adapter.SelectCommand = command;

                    // Create the DbCommandBuilder.
                    DbCommandBuilder builder = factory.CreateCommandBuilder();
                    builder.DataAdapter = adapter;

                    // Get the insert, update and delete commands.
                    adapter.DeleteCommand = builder.GetDeleteCommand();

                    // Fill the DataTable.
                    //DataTable table = new DataTable();
                    adapter.Fill(returnTable);

                    // edit a row.
                    string guid = spesificClass.GetType().GetProperty("GUID").GetValue(spesificClass).ToString();
                    DataRow[] deleteRow = returnTable.Select("guid = '" + guid + "'");
                    foreach (DataRow row in deleteRow)
                    {
                        
                        row.Delete();
                    }                    

                    adapter.Update(returnTable);

                    //returnTable.Columns[1].DataType.UnderlyingSystemType


                }
            }
            catch (Exception exception)
            {
                ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                error.handle(exception, true, false, exception.ToString());
            }

            return returnTable;
        }

        public class DataConverter
        {
            public static void SetPropertyValue(object obj, string propertyName, object propertyvalue)
            {
                PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);

                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(obj, Convert.ChangeType(propertyvalue, propertyInfo.PropertyType),null);
                }
            }
        }

        public T getSingleObject<T>(object obj)
        {
            // Cast it to itself
            T spesificClass = (T)Convert.ChangeType(obj, typeof(T));

            DataTable result = dynamicSelectQuery<T>(spesificClass);

            PropertyInfo[] objectProperties = spesificClass.GetType().GetProperties();

            SortedDictionary<string, string> valuePair = (SortedDictionary<string, string>)spesificClass.GetType().GetProperty("TableDataInfo").GetValue(spesificClass);

            foreach (PropertyInfo proper in objectProperties)
            {
                string ColumnName = valuePair.FirstOrDefault(x => x.Value == proper.Name).Key;
                proper.SetValue(obj, Convert.ChangeType(result.Rows[0][ColumnName],proper.PropertyType),null);
            }

            return (T)obj;
        }


        // List Only
        public List<T> getMultipleObject<T>(object obj, object[] selectParameters = null)
        {
            // Cast it to itself
            T spesificClass = (T)Convert.ChangeType(obj, typeof(T));

            DataTable result = null;

            if (selectParameters == null)
            {
                result = dynamicSelectQuery<T>(spesificClass);
            } else
            {
                result = dynamicSelectQuery<T>(spesificClass, selectParameters);
            }

            List<T> items = new List<T>();
            SortedDictionary<string, string> valuePair = (SortedDictionary<string, string>)spesificClass.GetType().GetProperty("TableDataInfo").GetValue(spesificClass);
            PropertyInfo[] objectProperties = spesificClass.GetType().GetProperties();

            foreach (DataRow row in result.Rows)
            {
                Type newObjType = obj.GetType();
                object objNew = Activator.CreateInstance(newObjType);
                
                foreach (PropertyInfo proper in objectProperties)
                {
                    string ColumnName = valuePair.FirstOrDefault(x => x.Value == proper.Name).Key;
                    proper.SetValue(objNew, Convert.ChangeType(result.Rows[0][ColumnName], proper.PropertyType), null);
                }

                items.Add((T)objNew);
            }




            return items;
        }


        //public T Clone<T>(T what) where T : ICloneable
        //{
        //    return (T)what.Clone();
        //}

        protected static void onInfoMessage(object sender, SqlInfoMessageEventArgs args)
        {
            // Can give message here

            //foreach (SqlError err in args.Errors)
            //{
            //    Console.WriteLine(
            //  "The {0} has received a severity {1}, state {2} error number {3}\n" +
            //  "on line {4} of procedure {5} on server {6}:\n{7}",
            //   err.Source, err.Class, err.State, err.Number, err.LineNumber,
            //   err.Procedure, err.Server, err.Message);
            //}
        }

        protected static void OnStateChange(object sender, StateChangeEventArgs args)
        {
            // Notify on state Change

            //Console.WriteLine(
            //  "The current Connection state has changed from {0} to {1}.",
            //    args.OriginalState, args.CurrentState);
        }



   

        


    }
}
