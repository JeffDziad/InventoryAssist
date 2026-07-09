using JeffDziad.InventoryAssist.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JeffDziad.InventoryAssist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DeviceManager deviceManager = new();

        private bool isRunning = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isRunning = !isRunning;
            Button b = (Button)sender;
            if (isRunning)
            {
                b.Content = "STOP";
                b.Background = Brushes.Red;
            }
            else
            {
                b.Content = "START";
                b.Background = Brushes.Green;
            }
        }

    }
}