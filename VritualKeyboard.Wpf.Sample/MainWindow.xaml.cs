using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using VirtualKeyboard.Wpf;
using VirtualKeyboard.Wpf.Types;

namespace VritualKeyboard.Wpf.Sample
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            VirtualKeyboard.Wpf.VKeyboard.ShowDiscardButton = true;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Label.Content = await VirtualKeyboard.Wpf.VKeyboard.OpenAsync(type:KeyboardType.Alphabet);
        }
    }
}
