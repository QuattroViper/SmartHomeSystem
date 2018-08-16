using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorHandle
{
    public class CustomException : Exception
    {
        public CustomException()
        {

        }

        public CustomException(string message) : base(message)
        {

        }

        public CustomException(string message, Exception inner) : base(message, inner)
        {

        }
    }

    public class CustomExceptionForUI : Exception
    {
        public CustomExceptionForUI()
        {

        }

        public CustomExceptionForUI(string message) : base(message)
        {

        }

        public CustomExceptionForUI(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
