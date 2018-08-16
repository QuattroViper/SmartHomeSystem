using System;
using ErrorHandler;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using System.Reflection;
using System.Linq;

namespace DataAccess
{
    public class DatabaseHandler : IDatabaseHandler
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;
        private string connectionStringSHS;

        private const string connectionString = "";

        /// <summary>
        /// 
        /// </summary>
        private static DatabaseHandler instance = new DatabaseHandler(new DataHandlerHelper());

        public static DatabaseHandler getInstance()
        {
            return instance;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        private DatabaseHandler(string connectionString = "default") {
            connectionStringSHS = connectionString.Equals("default") ? ConfigurationManager.ConnectionStrings[connectionString].ConnectionString : connectionString;            
        }

        private DatabaseHandler(DataHandlerHelper dataHelper)
        {
            //connectionStringSHS = connectionString.Equals("default") ? ConfigurationManager.ConnectionStrings[connectionString].ConnectionString : connectionString;
            connectionStringSHS = dataHelper.ConnectionToUse == DataHandlerHelper.Connection.ConnectionOne ? dataHelper.ConnectionStringOne : dataHelper.ConnectionStringTwo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        public bool nonQuery(string query, Dictionary<string, KeyValuePair<string, string>> parameters) 
        {
            connection = null;

            using (SqlConnection connection = new SqlConnection(connectionStringSHS))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    
                    command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, KeyValuePair<string,string>> parameter in parameters)
                    {
                        switch (parameter.Value.Value)
                        {
                            case "GUID":
                                string value = (parameter.Value.Key); command.Parameters.Add(new SqlParameter(parameter.Key, new Guid(value) ));
                                break;
                            case "DOUBLE":
                                command.Parameters.Add(new SqlParameter(parameter.Key, Convert.ToDouble(parameter.Value.Key)));
                                break;
                            case "STRING":
                                command.Parameters.Add(new SqlParameter(parameter.Key, SqlDbType.VarChar,1699)); command.Parameters[parameter.Key].Value = parameter.Value.Key;
                                break;
                            case "INT":
                                command.Parameters.Add(new SqlParameter(parameter.Key, Convert.ToInt32(parameter.Value.Key)));
                                break;
                            case "DATETIME":
                                command.Parameters.Add(new SqlParameter(parameter.Key, Convert.ToDateTime(parameter.Value.Key)));
                                break;
                            case "BOOL":
                                command.Parameters.Add(new SqlParameter(parameter.Key, Convert.ToBoolean(parameter.Value.Key)));
                                break;
                            case "DECIMAL":
                                command.Parameters.Add(new SqlParameter(parameter.Key, Convert.ToDecimal(parameter.Value.Key)));
                                break;
                            case "MONEY":
                                command.Parameters.Add(new SqlParameter(parameter.Key, SqlDbType.SmallMoney)); command.Parameters[parameter.Key].Value = parameter.Value.Key;
                                break;
                            default:
                                command.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value.Key));
                                break;
                        }
                        
                    }


                    command.ExecuteNonQuery();

                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true);
                    return false;
                }
                
            }

            return true;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable returnQuery(string query, Dictionary<string, string> parameters)
        {
            dataTable = new DataTable();
            connection = null;
            using (connection = new SqlConnection(connectionStringSHS))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, string> item in parameters)
                    {
                        command.Parameters.Add(new SqlParameter(item.Key, item.Value));
                    }


                    dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);
                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true);
                }

                
                
            }

            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable getFromStringQuery(string query)
        {
            dataTable = new DataTable();
            connection = null;

            using (connection = new SqlConnection(connectionStringSHS))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    command = new SqlCommand(query, connection);


                    dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);
                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception, true, true);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool insertQueryFromString(string query)
        {
            connection = null;
            using (connection = new SqlConnection(connectionStringSHS))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    command = new SqlCommand(query, connection);
                }
                catch (Exception exception)
                {
                    ErrorHandler.ErrorHandle error = ErrorHandler.ErrorHandle.getInstance();
                    error.handle(exception,true);
                    return false ;
                }
            }

            return true;
                
        }

        public static Type GetClrType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(long?);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool?);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    return typeof(DateTime?);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return typeof(decimal?);

                case SqlDbType.Float:
                    return typeof(double?);

                case SqlDbType.Int:
                    return typeof(int?);

                case SqlDbType.Real:
                    return typeof(float?);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid?);

                case SqlDbType.SmallInt:
                    return typeof(short?);

                case SqlDbType.TinyInt:
                    return typeof(byte?);

                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    return typeof(object);

                case SqlDbType.Structured:
                    return typeof(DataTable);

                case SqlDbType.DateTimeOffset:
                    return typeof(DateTimeOffset?);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }

        //private static object Convert(string value, Type type)
        //{
        //    object result;

        //    if (string.IsNullOrWhiteSpace(value))
        //    {
        //        return null;
        //    }

        //    try
        //    {
        //        var converter = TypeDescriptor.GetConverter(type);
        //        result = converter.ConvertFromString(value);
        //        return result;
        //    }
        //    catch (Exception exception)
        //    {
        //        // Log this exception if required.
        //        throw new InvalidCastException();
        //    }
        //}

    }
}
