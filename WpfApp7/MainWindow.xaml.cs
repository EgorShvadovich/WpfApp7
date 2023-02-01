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
using System.Data;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace WpfApp7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Sales.Enities.Managers>? _managers;
        private List<Sales.Enities.Product>? _products;
        private List<Sales.Enities.TodaySales>? _todaysales;
        private SqlConnection _connection;
        
        public MainWindow()
        {
            InitializeComponent();

            string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HP\Desktop\C++ дз шаг\WpfApp7\WpfApp7\Database1.mdf;Integrated Security=True";
            _connection = new(App.StringConnection);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _connection.Open();
                MonitorConnection.Content = "Установлено";
                MonitorConnection.Foreground = Brushes.Green;
            }
            catch (SqlException ex)
            {
                MonitorConnection.Content = "Закріто";
                MonitorConnection.Foreground = Brushes.Red;
                MessageBox.Show(ex.Message);
                this.Close();
            }
            ShowDepartmentsCount();
            ShowManagersCount();
            ShowProductsCount();
            ShowSalesCount();
            ShowDepartments();
            ShowManagersORM();
            ShowDailyStatistics();
            ShowProductsORM();
            ShowTodaySalesORM();


        }
        #region Show Monitor

        private void ShowDepartmentsCount()
        {
            String sql = "SELECT COUNT(*)FROM Departments";
            using var cmd = new SqlCommand(sql, _connection);
            cmd.ExecuteScalar();
            MonitorDepartments.Content = Convert.ToString(cmd.ExecuteScalar());
        }
        private void ShowManagersCount()
        {
            String sql = "SELECT COUNT(*)FROM Managers";
            using var cmd = new SqlCommand(sql, _connection);
            cmd.ExecuteScalar();
            MonitorManagers.Content = Convert.ToString(cmd.ExecuteScalar());
        }
        private void ShowProductsCount()
        {
            String sql = "SELECT COUNT(*)FROM Products";
            using var cmd = new SqlCommand(sql, _connection);
            cmd.ExecuteScalar();
            MonitorProducts.Content = Convert.ToString(cmd.ExecuteScalar());
        }
        private void ShowSalesCount()
        {
            String sql = "SELECT COUNT(*)FROM Sales";
            using var cmd = new SqlCommand(sql, _connection);
            cmd.ExecuteScalar();
            MonitorSales.Content = Convert.ToString(cmd.ExecuteScalar());
        }
        #endregion
        private void ShowDailyStatistics()
        {
            SqlCommand cmd = new();
            cmd.Connection = _connection;
            String date = $"2022-{DateTime.Now.Month}-{DateTime.Now.Day}";
            cmd.CommandText = $"SELECT COUNT(*)FROM Sales S WHERE CAST(S.Moment AS DATE) = '{date}'";
            StatTotalSales.Content = Convert.ToString(cmd.ExecuteScalar());

            cmd.CommandText = $"SELECT SUM(S.Cnt) FROM Sales S WHERE CAST(S.Moment AS DATE) = '{date}'";
            StatTotalProducts.Content = Convert.ToString(cmd.ExecuteScalar());

            cmd.CommandText = $"SELECT ROUND( SUM( S.Cnt * P.Price ), 2 ) FROM Sales S JOIN Products P ON ID_product = P.Id WHERE CAST(S.Moment AS DATE) = '{date}'";
            StatTotalValue.Content = Convert.ToString(cmd.ExecuteScalar());

            cmd.CommandText = $"SELECT TOP 1 M.name,(S.Cnt* P.Price ) FROM Sales S JOIN Products P ON ID_product = P.Id JOIN Managers M ON S.ID_manager = M.id WHERE CAST(S.Moment AS DATE) = '{date}' order by 2 desc";
            StatTotalManager.Content = Convert.ToString(cmd.ExecuteScalar());

            cmd.CommandText = $"SELECT TOP 1 D.name,S.Cnt FROM Sales S JOIN Products P ON ID_product = P.Id JOIN Managers M ON S.ID_manager = M.id JOIN Departments D ON M.id_main_dep = D.id WHERE CAST(S.Moment AS DATE) = '{date}' order by 2 desc";
            StatTotalDepartment.Content = Convert.ToString(cmd.ExecuteScalar());

            cmd.CommandText = $"SELECT TOP 1 P.name,S.Cnt FROM Sales S JOIN Products P ON ID_product = P.Id WHERE CAST(S.Moment AS DATE) = '{date}' order by 2 desc";
            StatPopularProducts.Content = Convert.ToString(cmd.ExecuteScalar());
            cmd.Dispose();
        }

        private void ShowManagersORM()
        {
            if (_managers is null) // 1) обращение - заполняем коллекцию  
            {
                using SqlCommand cmd = new("SELECT M.Id, M.Surname, M.Name, M.Secname FROM Managers M", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _managers = new();
                    while (reader.Read())
                    {
                        _managers.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString("Name"),
                            Surname = reader.GetString("Surname"),
                            Secname = reader.GetString("Secname")
                        });
                    }
                }
                catch (SqlException ex)
                {

                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            ManagersCell.Text = "";
            foreach (var managers in _managers)
            {
                ManagersCell.Text += managers.ToShortString() + "\n";
            }
        }
        private void ShowProductsORM()
        {
            if (_products is null) // 1) обращение - заполняем коллекцию  
            {
                using SqlCommand cmd = new("SELECT P.Id, P.Name, P.Price FROM Products P", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _products = new();
                    while (reader.Read())
                    {
                        _products.Add(new()
                        {
                            Id = reader.GetGuid("id"),
                            Name = reader.GetString(1),
                            Price = reader.GetDouble(2)
                        });
                    }
                }
                catch (SqlException ex)
                {

                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            ProductsCell.Text = "";
            foreach (var product in _products)
            {
                ProductsCell.Text += product.ToShortString() + "\n";
            }
        }
        private void ShowDepartments()
        {
            using SqlCommand cmd = new("SELECT * FROM Departments", _connection);
            SqlDataReader reader = cmd.ExecuteReader();      // Табличный запрос - возвращает SqlDataReader
            DepartmentCell.Text = "";                        // Передача данных происходит "построчно" - по одной строке выборки (результата)
            while (reader.Read())                            // Вызов ExecuteReader не читает данные, только создает reader
            {                                                // Команда reader.Read() заполняет reader
                Guid id = reader.GetGuid("id");              // данными (строкой-выборкой) - "самозаполняется"
                String name = (String)reader[1];            // !! возврат Read() - статус (успех/конец)
                // double price = (double) reader["Price"];  // После чтения данные доступны
                DepartmentCell.Text += $"{id} {name} \n";    // а) через Get-теры (GetGuid/GetString,...) с индексом
            }                                                // б) через Get-теры с именем поля (using System.Data)
                                                             // в) через индексатор [] c числом - индексом поля
                                                             // г) через индексатор [] со строкой - названием поля
                                                             // Значение индекса начинается с 0 и соответствует
                                                             // порядку данных в строке-результате (в таблице)
                                                             // Поскольку обращение к данным идет по индексам, крайне НЕ рекомендуется
                                                             // оформлять запрос как SELECT * - в нем не видно порядок полей
                                                             // Лучше перечислять поля SELECT id, name FROM Departments
                                                             // reader обязательно нужно закрывать, если останется открытым, то не будут
                                                             // выполняться другие команды
            reader.Dispose();
        }
        private void ShowTodaySalesORM()
        {
            String date = $"2022-{DateTime.Now.Month}-{DateTime.Now.Day}";
            if (_todaysales is null)
            {
                using SqlCommand cmd = new($"SELECT s.ID, p.Name , s.Cnt, p.price FROM Sales s JOIN Products p ON s.ID_product = p.id WHERE CAST(s.Moment AS DATE) = '{date}'", _connection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    _todaysales = new();
                    while (reader.Read())
                    {
                        _todaysales.Add(new()
                        {
                            Id = reader.GetGuid("ID"),
                            Name = reader.GetString("Name"),
                            Count = reader.GetInt32("Cnt"),
                            Sum = reader.GetDouble("Price") * reader.GetInt32("Cnt")
                        });
                    }
                }
                catch (SqlException ex)
                {

                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            TodaySales.Text = "";
            foreach (var todaySales in _todaysales)
            {
                TodaySales.Text += todaySales.ToShortString() + "\n";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_connection?.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }




}
