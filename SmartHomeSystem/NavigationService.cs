using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartHomeSystem
{
    public static class NavigationService
    {
        private static readonly Stack<Window> NavigationStack = new Stack<Window>();
        private static Window mainWindow = null;

        static NavigationService()
        {
            mainWindow = Application.Current.MainWindow;
            NavigationStack.Push(mainWindow);
            
        }

        public static void NavigateTo(Window win)
        {
            
            if (NavigationStack.Count > 0)
                NavigationStack.Peek().Hide();

            NavigationStack.Push(win);
            mainWindow.Hide();
            win.Show();

           
        }

        public static void NavigateToWithoutHide(Window win)
        {
            //if (NavigationStack.Count > 0)
            //    NavigationStack.Peek().Hide();

            NavigationStack.Push(win);
            win.Show();
        }

        public static bool NavigateBack()
        {
            if (NavigationStack.Count <= 1)
                return false;

            //NavigationStack.Pop().Close();
            NavigationStack.Pop().Hide();
            NavigationStack.Peek().Show();
            return true;
        }

        public static bool CanNavigateBack()
        {
            return NavigationStack.Count > 1;
        }
    }
}
