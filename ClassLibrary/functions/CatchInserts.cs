using ClassLibrary.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.functions
{
    public static class CatchInserts
    {
        static CatchInserts()
        {

        }

        public static void notifyOfNewInsert()
        {

        }

        public static void notifyOfNewInsert(string classInserted)
        {
            Guid? personGuid = Global.ADUser.GUID;
            object locker = new object();
            Task.Run(() => {

                lock (locker)
                {

                }

            });
        }

        public static void addChangeToDatabase(string classInserted)
        {
            Guid? personGuid = Global.ADUser.GUID;
            object locker = new object();
            Task.Run(() => {

                lock(locker)
                {

                }

            });

           
        }
    }
}
