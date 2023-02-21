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
    /// <summary>
    /// Логика взаимодействия для EfWindow.xaml
    /// </summary>
    public partial class EfWindow : Window
    {
        public EfContext.DataContext dataContext;
        public EfWindow()
        {
            InitializeComponent();
            dataContext = new();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MonitorDepartments.Content = dataContext.Departaments.Count().ToString();
            MonitorManagers.Content = dataContext.Manager.Count().ToString();
            MonitorProducts.Content = dataContext.Products.Count().ToString();
            MonitorSales.Content = dataContext.Sales.Count().ToString();
            DailyStatisticks();
        }
        private void DailyStatisticks()
        {
            SalesCount.Content = dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date).Count();
            SalesTotal.Content = dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date).Sum(sale => sale.Count);
            SalesHRN.Content = dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date)
                .Join(dataContext.Products,
                sale => sale.IdProduct,
                prod => prod.Id,
                (sale, prod) => sale.Count * prod.Price).Sum().ToString("0.00");

            BestManager.Content = dataContext.Manager.GroupJoin(
                dataContext.Sales,
                man => man.Id,
                sale => sale.IdManager,
                (man, sales) => new { Manager = man, Total = sales.Where(sale => sale.Moment.Date == DateTime.Now.Date).Sum(s => s.Count) }).OrderByDescending(mix => mix.Total).Take(1).Select(mix => mix.Manager.ToShortString() + $"({mix.Total})").First();

            TopProduct.Content = dataContext.Products.GroupJoin(
               dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date),
               p => p.Id,
               sale => sale.IdProduct,
               (p, sales) => new { Product = p, Total = sales.Sum(s => s.Count) })
               .OrderByDescending(mix => mix.Total)
               .Take(1)
               .Select(mix => mix.Product.ToShortString() + $" ({mix.Total})")
               .First();

            TopDepartment.Content = dataContext.Departaments.GroupJoin(
               dataContext.Sales.Where(sale => sale.Moment.Date == DateTime.Now.Date).Join(dataContext.Manager,
               s => s.IdManager,
               m => m.Id,
               (s, m) => new { Sale = s, Manager = m }),
               d => d.Id,
               m => m.Manager.IdMainDep,
               (d, mix) => new { Department = d, Total = mix.FirstOrDefault().Sale })
               .OrderByDescending(mix => mix.Total)
               .Take(1)
               .Select(mix => mix.Department.Name)
               .First();

        }
        private void ButtonSalesAdd_Click(object sender, RoutedEventArgs e)
        {
            int managerCount = dataContext.Manager.Count();
            int productCount = dataContext.Products.Count();
            for (int i = 0; i < 100; i++)
            {
                dataContext.Sales.Add(new()
                {
                    Id = Guid.NewGuid(),
                    IdManager = dataContext.Manager.Skip(App.random.Next(managerCount)).First().Id,
                    IdProduct = dataContext.Products.Skip(App.random.Next(productCount)).First().Id,
                    Count = App.random.Next(1, 10),
                    Moment = DateTime.Now + TimeSpan.FromHours(App.random.Next(-24,24))
                });
            }
            
            dataContext.SaveChanges();
            MonitorSales.Content = dataContext.Sales.Count().ToString();
            DailyStatisticks();
        }
    }
}
