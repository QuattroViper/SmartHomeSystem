using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IDatabaseHandler
    {

        // 
        bool nonQuery(string query, Dictionary<string, KeyValuePair<string, string>> parameters);

        //
        DataTable returnQuery(string query, Dictionary<string, string> parameters);

        //
        DataTable getFromStringQuery(string query);

        //
        bool insertQueryFromString(string query);

    }
}
