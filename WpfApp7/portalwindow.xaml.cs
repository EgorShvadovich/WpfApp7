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
using System.Windows.Shapes;

namespace WpfApp7
{
   
    public partial class portalwindow : Window
    {
        public portalwindow()
        {
            InitializeComponent();
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new MainWindow().ShowDialog();
            this.Show();
        }
        private void ButtonClick2(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new disconnect().ShowDialog();
            this.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new LinqWindow().ShowDialog();
            this.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new EfWindow().ShowDialog();
            this.Show();
        }
    }
}
