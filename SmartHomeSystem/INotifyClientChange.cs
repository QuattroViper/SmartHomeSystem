using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeSystem
{
    public interface INotifyClientChange
    {
        void updateView(Guid guid);
    }

    public interface INotifyProductChange
    {
        void updateView(Guid guid);
    }
}
