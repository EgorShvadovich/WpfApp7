﻿using Sales.Enities;
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
                using SqlCommand cmd3 = new("SELECT Id, Name, Surname, Secname, Id_main_dep, Id_sec_dep, Id_chief FROM Managers", connection);
                {
                    using var reader = cmd3.ExecuteReader();
                    while (reader.Read())
                    {
                        Managers.Add(new() //изменения коллекции автоматически изменяет и изменения на ListView 
                        {
                            Id = reader.GetGuid(0),
                            Surname = reader.GetString(1),
                            Name = reader.GetString(2),
                            Secname = reader.GetString(3),
                            IdMainDep = reader.GetGuid(4),
                            IdSecDep = reader[5] == DBNull.Value ? null : reader.GetGuid(5),
                            IdChief = reader[6] == DBNull.Value ? null : reader.GetGuid(6),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item) // item = sender as ListViewItem
            {
                // Обратная связь (view->model) через item.Content 
                if (item.Content is Department department)
                {
                    Sales.CRUD.CrudDepartmentWindow window = new()
                    {
                        Department = department
                    };
                    int index = Departments.IndexOf(department);
                    Departments.Remove(department); // удалеям из колекции и передаём на редактирование
                    if (window.ShowDialog().GetValueOrDefault())
                    {
                        using SqlConnection connection = new(App.StringConnection);
                        try
                        {
                            connection.Open();
                            using SqlCommand cmd = new() { Connection = connection };
                            if (window.Department is null) //удаление
                            {
                                cmd.CommandText = $"DELETE FROM Departments WHERE Id = '{department.Id}' ";
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Данные удалены!");
                            }
                            else //возвращаем но изменённый.
                            {
                                cmd.CommandText = $"UPDATE Departments SET Name = N'{department.Name}' WHERE Id = '{department.Id}'";
                                Departments.Insert(index, department);
                            }
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Задание выполнено успешно!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    else//отмена - возвращение в окно
                    {
                        Departments.Insert(index, department);
                    }
                    //MessageBox.Show(department.ToShortString());
                }

            }
        }

        private void ListViewItem_MouseDoubleClick2(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Sales.Enities.Product product)
                    MessageBox.Show(product.ToShortString());
            }

        }

        private void ListViewItem_MouseDoubleClick3(object sender, MouseButtonEventArgs e)
        {

        }
        private void AddDepartment_Click(object sender, RoutedEventArgs e)
        {
            var window = new Sales.CRUD.CrudDepartmentWindow();

            if (window.ShowDialog().GetValueOrDefault())
            {
                MessageBox.Show(window.Department?.ToShortString());
                using SqlConnection connection = new(App.StringConnection);
                try
                {
                    connection.Open();
                    using SqlCommand cmd = new(
                        $"INSERT INTO Departments(Id, Name) VALUES( @id, @name )",
                        connection);
                    cmd.Parameters.AddWithValue("@id", window.Department.Id);
                    cmd.Parameters.AddWithValue("@name", window.Department.Name);
                    cmd.ExecuteNonQuery();

                    Departments.Add(window.Department);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Cancel");
            }
        }
        private void AddManager_Click(object sender, RoutedEventArgs e)
        {
            WpfApp7.CRUD.CrudManagerWindow managerWindow = new() { Owner = this };
            managerWindow.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var man = Managers[3];
           
            var dep = Departments.FirstOrDefault(d => d.Id == man.IdMainDep);
            var dep1 = Departments.FirstOrDefault(d => d.Id == man.IdSecDep);


            textBlock1.Text += man.Name + " " + dep.Name + " " + (dep1?.Name ?? "--") + "\n";
        }
    }
}
