using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace DataAccess
{
    //public enum Tables
    //{
    //    [Display(Name = "Person")]
    //    tblPerson,
    //    [Display(Name = "Client")]
    //    tblClient,
    //    [Display(Name = "Technician")]
    //    tblTechincian
    //}

    //public class TableStrings
    //{
    //    public static string Person = "tblPerson";

    //    public static string Product = "tblproduct";

    //    public static string Client = "tblClient";

    //    public static string Technician = "tblTechnician";

    //}

    public class Database
    {

        /// <summary>
        /// List of all the tables in human form.
        /// The index number of the enum must allign with the database SortedDictionary
        /// </summary>
        public enum Tables
        {
            Person = 0,
            Product,
            Client
        }     

        /// <summary>
        /// Perfect database vs Class reflection should happen here!!
        /// </summary>
        public static SortedDictionary<string, string[,]> database = new SortedDictionary<string, string[,]> {
            {
                "tblPerson",
                new string[,] {
                    { "guid", "GUID" },
                    { "identification_number", "ID" },
                    { "name", "Name" },
                    { "surname", "Surname" },
                    { "date_of_birth", "DateOfBirth" },
                    { "gender", "Gender" }
                }
            },
            {
                "tblProduct",
                new string[,] {
                    { "guid", "GUID"},
                    { "product_code", "ProductCode" },
                    { "name", "Name" },
                    { "description", "Description" },
                    { "price", "Price" },
                    { "type", "Type" },
                    { "warrenty_period", "WarrentyPeriod" },
                    { "in_stock", "InStock" },
                    { "arrival_date", "ArrivalDate" },
                }
            }
        };
    }

    
}
