using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeSystem
{
    public interface IEventBuss
    {
        void OnEvent(CustomEvent customEvent);
    }
}
