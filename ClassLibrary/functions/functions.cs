using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Collections;

namespace ClassLibrary.functions
{
    public class functions
    {

        /// <summary>
        /// Is equal to PHP's var_dump function. It gives everything about the type. 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="recursion"></param>
        /// <returns></returns>
        public static string var_dump(object obj, int recursion)
        {
            StringBuilder result = new StringBuilder();

            // Protect the method against endless recursion
            if (recursion < 5)
            {
                // Determine object type
                Type t = obj.GetType();

                // Get array with properties for this object
                PropertyInfo[] properties = t.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        // Get the property value
                        object value = property.GetValue(obj, null);

                        // Create indenting string to put in front of properties of a deeper level
                        // We'll need this when we display the property name and value
                        string indent = String.Empty;
                        string spaces = "|   ";
                        string trail = "|...";

                        if (recursion > 0)
                        {
                            indent = new StringBuilder(trail).Insert(0, spaces, recursion - 1).ToString();
                        }

                        if (value != null)
                        {
                            // If the value is a string, add quotation marks
                            string displayValue = value.ToString();
                            if (value is string) displayValue = String.Concat('"', displayValue, '"');

                            // Add property name and value to return string
                            result.AppendFormat("{0}{1} = {2}\n", indent, property.Name, displayValue);

                            try
                            {
                                if (!(value is ICollection))
                                {
                                    // Call var_dump() again to list child properties
                                    // This throws an exception if the current property value
                                    // is of an unsupported type (eg. it has not properties)
                                    result.Append(var_dump(value, recursion + 1));
                                }
                                else
                                {
                                    // 2009-07-29: added support for collections
                                    // The value is a collection (eg. it's an arraylist or generic list)
                                    // so loop through its elements and dump their properties
                                    int elementCount = 0;
                                    foreach (object element in ((ICollection)value))
                                    {
                                        string elementName = String.Format("{0}[{1}]", property.Name, elementCount);
                                        indent = new StringBuilder(trail).Insert(0, spaces, recursion).ToString();

                                        // Display the collection element name and type
                                        result.AppendFormat("{0}{1} = {2}\n", indent, elementName, element.ToString());

                                        // Display the child properties
                                        result.Append(var_dump(element, recursion + 2));
                                        elementCount++;
                                    }

                                    result.Append(var_dump(value, recursion + 1));
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            // Add empty (null) property to return string
                            result.AppendFormat("{0}{1} = {2}\n", indent, property.Name, "null");
                        }
                    }
                    catch
                    {
                        // Some properties will throw an exception on property.GetValue()
                        // I don't know exactly why this happens, so for now i will ignore them...
                    }
                }
            }

            return result.ToString();
        }


        /// <summary>
        /// Given the ID in a string, it will be decoded into the date of born, gender, is SA and is authenticated
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Dictionary<string,object> decodeID(string id )
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            //if (GetControlDigit(id) == -1)
            //{
            //    dict.Add("date", DateTime.UtcNow);
            //    dict.Add("gender", "Unkown");
            //    dict.Add("Authenticated", "No");
            //    dict.Add("SA", "N/A");
            //    return dict;
            //}

            try
            {
                string date = id.Substring(0, 6);
                int month = Convert.ToInt32(id.Substring(2, 2));
                int day = Convert.ToInt32(id.Substring(4, 2));
                int dateCalc = 0;
                string gender = id.Substring(6, 1);
                int year = new DateTime().Year;
                if (Convert.ToInt32(date.Substring(0, 2)) > year)
                {
                    dateCalc += Convert.ToInt32("19" + date.Substring(0, 2));
                }
                else
                {
                    dateCalc += Convert.ToInt32("20" + date.Substring(0, 2));
                }

                DateTime dateValue = new DateTime(dateCalc, month, day);
                if (gender.Equals("0"))
                {
                    gender = "Female";
                }
                else
                {
                    gender = "Male";
                }

                dict.Add("date", dateValue);
                dict.Add("gender", gender);
                dict.Add("Authenticated", "Yes");
                dict.Add("SA", "Yes");
            }
            catch
            {
                dict.Add("date", DateTime.UtcNow);
                dict.Add("gender", "Unkown");
                dict.Add("Authenticated", "No");
                dict.Add("SA", "N/A");
                return dict;
            }



            return dict;

        }

        public static int GetControlDigit(string parsedIdString)
        {
            int d = -1;
            try
            {
                int a = 0;
                for (int i = 0; i < 6; i++)
                {
                    a += int.Parse(parsedIdString[2 * i].ToString());
                }
                int b = 0;
                for (int i = 0; i < 6; i++)
                {
                    b = b * 10 + int.Parse(parsedIdString[2 * i + 1].ToString());
                }
                b *= 2;
                int c = 0;
                do
                {
                    c += b % 10;
                    b = b / 10;
                }
                while (b > 0);
                c += a;
                d = 10 - (c % 10);
                if (d == 10) d = 0;
            }
            catch {/*ignore*/}
            return d;
        }



    }
}
