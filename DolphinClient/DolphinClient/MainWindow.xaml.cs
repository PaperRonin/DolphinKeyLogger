using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DolphinClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogBox.Text = Client.SendCommand("screen");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LogBox.Text = Client.SendCommand("logs");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LogBox.Text = Client.SendCommand("quit");
            Close();
        }

        private void TextChanger(string str)
        {
            LogBox.Text += "\n" + str;
        }

    }
}
