using Sales.Enities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
    /// <summary>
    /// Логика взаимодействия для disconnect.xaml
    /// </summary>
    public partial class disconnect : Window
    {
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Managers> Managers { get; set; }
        public disconnect()
        {
            InitializeComponent();
            DataContext = this;
            using SqlConnection connection = new(App.StringConnection);
            try
            {
                connection.Open();
                Departments = new();
                using SqlCommand cmd = new SqlCommand("SELECT Id,Name FROM Departments", connection);
                {
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Departments.Add(new()
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1)
                        });

                    }
                }
                Products = new();
                using SqlCommand cmdProd = new SqlCommand("SELECT Id,Name, Price FROM Products", connection);
                {
                    using var readerProd = cmdProd.ExecuteReader();
                    while (readerProd.Read())
                    {
                        Products.Add(new()
                        {
                            Id = readerProd.GetGuid(0),
                            Name = readerProd.GetString(1),
                            Price = readerProd.GetDouble(2)
                        });

                    }
                }
                Managers = new();
                using SqlCommand cmdManag = new SqlCommand("SELECT Id,Name FROM Managers", connection);
                {
                    using var readerManag = cmdManag.ExecuteReader();
                    while (readerManag.Read())
                    {
                        Managers.Add(new()
                        {
                            Id = readerManag.GetGuid(0),
                            Name = readerManag.GetString(1),
                        });

                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Sales.Enities.Department department)
                    MessageBox.Show(department.ToShortString());
            }

        }
        
        private void ListViewItem_MouseDoubleClick2(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if(item.Content is Sales.Enities.Product product)
                    MessageBox.Show(product.ToShortString());
            }

        }

        private void ListViewItem_MouseDoubleClick3(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Sales.Enities.Managers manager)
                    MessageBox.Show(manager.ToShortString());
            }
        }
    }
}
