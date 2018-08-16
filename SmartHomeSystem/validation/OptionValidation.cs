using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SmartHomeSystem.validation
{
    public class OptionValidation<Y>
    {
        public ValidationResult validate<T>(object target)
        {
            T spesificClass = (T)Convert.ChangeType(target, typeof(T));
            Type type = spesificClass.GetType();

            string strValue = Convert.ToString(target);

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Input should not be empty"); 
            bool canConvert = false;

            switch (type.Name)
            {

                case "Boolean":
                    bool boolVal = false;
                    canConvert = bool.TryParse(strValue, out boolVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of boolean");
                case "Int32":
                    int intVal = 0;
                    canConvert = int.TryParse(strValue, out intVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int32");
                case "Double":
                    double doubleVal = 0;
                    canConvert = double.TryParse(strValue, out doubleVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Double");
                case "Int64":
                    long longVal = 0;
                    canConvert = long.TryParse(strValue, out longVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int64");
                case "Decimal":
                    decimal decVal = 0;
                    canConvert = decimal.TryParse(strValue, out decVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int64");
                default:
                    return new ValidationResult(false, $"Input not supported"); 
            }
        }
    }
}
