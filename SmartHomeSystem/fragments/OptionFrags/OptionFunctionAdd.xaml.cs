using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EventBus;

namespace SmartHomeSystem.fragments.OptionFrags
{
    /// <summary>
    /// Interaction logic for OptionFunctionAdd.xaml
    /// </summary>
    public partial class OptionFunctionAdd : Window, IDetachContent, IEventBus
    {

        ObservableCollection<string> functions;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            EventBus.EventBus.Instance.Unregister(this);
        }

        public OptionFunctionAdd()
        {
            InitializeComponent();
        }

        public OptionFunctionAdd(ObservableCollection<string> functionList)
        {
            InitializeComponent();
            functions = functionList;
            EventBus.EventBus.Instance.Register(this);
        }

        private void btnAddFunction_Click(object sender, RoutedEventArgs e)
        {
            if (txtOptionGuid.Text.Length == 0)
            {
                NavigationService.NavigateBack();
            } else
            {
                functions.Add(txtOptionGuid.Text);

                EventBus.EventBus.Instance.PostEvent(new CustomEvent("FunctionAdded"));

                NavigationService.NavigateBack();
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.NavigateBack();
        }

        private void txtOptionGuid_TextChanged(object sender, TextChangedEventArgs e)
        {
            //EventBus.EventBus.Instance.PostEvent(e);
        }

        public void OnEvent(CustomEvent customEvent)
        {

        }

        public void DetachContent()
        {
            RemoveLogicalChild(Content);
        }
    }
}
