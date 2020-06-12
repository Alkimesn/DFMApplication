using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigFiltersModel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller;
        IFilterController filterController;
        ISignalController signalController;
        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller();
            controller.OnReload += (s, e) =>ReDraw();
            filterController = new FilterFileController();
            signalController = new SignalFileController();
        }
        void ReDraw()
        {
            DFMDrawer.Draw(cvSignalGraph, controller.InputSignalAsArray, controller.OutputSignalAsArray);
            DFMDrawer.Draw(cvSignalFreqGraph, controller.InputSignalFTAsArray, controller.OutputSignalFTAsArray);
            SetSignalInfo();
            SetFilterInfo();
        }
        void SetSignalInfo()
        {
            lbSignalName.Content = controller.CurSignal.Name;
            lbSignalLength.Content = controller.CurSignal.Length();
        }
        void SetFilterInfo()
        {
            lbFilterName.Content = controller.CurFilter.Name;
            lbFilterOrder.Content = controller.CurFilter.Order;
        }
        private void cvSignalGraph_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void cvSignalGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ReDraw();
        }

        private void btnLoadFilterFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                filterController.LoadFilter(openFileDialog.FileName, controller);
        }

        private void btnLoadSignal_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                signalController.LoadSignal(openFileDialog.FileName, controller);
        }

        private void tbLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(tbLength.Text, out int res))
                if (res > 0)
                    controller.MaxLength = res;
        }

        private void btnSaveSignal_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
