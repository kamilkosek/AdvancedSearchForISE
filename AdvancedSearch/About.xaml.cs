using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace AdvancedSearch
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/kamilkosek");
        }

        private void TextBlock_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://www.nt-guys.com/AdvancedSearchISEAddon");
        }

        private void TextBlock_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://twitter.com/kamilkosek");
        }
    }
}
