using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.interfaces
{
    interface IId { }

    public struct Id<T>: IId
    {
        private readonly string value;

        public Id(string value)
        {
            this.value = value;
        }

        public static implicit operator string(Id<T> id)
        {
            return id.value;
        }

        public static explicit operator Id<T>(string value)
        {
            return new Id<T>(value);
        }
    }

    public struct Person { }
    public struct Product { }
    public struct Location { }
    public struct Client { }
    public struct Employee { }
    public struct Address { }
}
