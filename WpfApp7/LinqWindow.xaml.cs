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
    /// Логика взаимодействия для LinqWindow.xaml
    /// </summary>
    public partial class LinqWindow : Window
    {
        private LinqContext.DataContext context;
        public LinqWindow()
        {
            InitializeComponent();
            try
            {
                context = new(App.StringConnection);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Simple_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.OrderBy(p => p.Price);
            textBlock1.Text = "";
            foreach(var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
        }
        private void Simple2_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.OrderByDescending(p => p.Price);
            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
        }
        private void Simple3_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.Where(p => p.Price < 200);
            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
        }
        
        private void Simple4_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.Where(p => p.Price < 200).OrderBy(p => p.Price);
            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
        }
        private void Simple5_Click(object sender, RoutedEventArgs e)
        {

            var query = context.Products.Where(p => p.Name.Contains("ов"));
            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            textBlock1.Text += "\n" + "всего " + query.Count();

        }

        private void Simple6_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.Where(p => p.Name.Contains("Г"));
            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Price + " " + item.Name + "\n";
            }
            textBlock1.Text += "\n" + "всего " + query.Count();

        }

        private void Simple7_Click(object sender, RoutedEventArgs e)
        {
            var query2 = from m in context.Managers
                         join d in context.Departments on m.IdMainDep equals d.Id
                         select new
                         {
                             Managers = m.Surname + " " + m.Name,
                             Department = " отдел " + d.Name
                         };
            var query = context.Managers.Join(  
                context.Departments,            
                m => m.IdMainDep,               
                d => d.Id,                      
                (m, d) => new { Managers = m.Surname + " " + m.Name, Department = d.Name }); 

            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Managers + " " + item.Department + "\n";
            }
            textBlock1.Text += "\n" + "всего " + query.Count();
        }

        private void Simple8_Click(object sender, RoutedEventArgs e)
        {
            var query2 = from m in context.Managers
                         join c in context.Managers on m.IdChief equals c.Id
                         select new
                         {
                             Managers = m.Surname + " " + m.Name,
                             Managers2 = " начальник " + c.Surname + " " + c.Name
                         };
            var query = context.Managers.Join( context.Managers,m => m.IdChief, c => c.Id, (m, c) => new { Managers = m.Surname + " " + m.Name, Managers2 = " Начальник - " + c.Surname + " " + c.Name }); 

            textBlock1.Text = "";
            foreach (var item in query)
            {
                textBlock1.Text += item.Managers + " " + item.Managers2 + "\n";
            }
            textBlock1.Text += "\n" + "всего " + query.Count();
        }

        private void Simple9_Click(object sender, RoutedEventArgs e)
        {
            var query2 = from m in context.Managers
                         join d in context.Departments on m.IdMainDep equals d.Id
                         join c in context.Managers on m.IdChief equals c.Id
                         select new
                         {
                             Managers = m.Surname + " " + m.Name,
                             Managers2 = " начальник " + c.Surname + " " + c.Name,
                             Department = " отдел " + d.Name
                         };

            textBlock1.Text = "";
            foreach (var item in query2)
            {
                textBlock1.Text += item.Managers + " " + item.Department + " " + item.Managers2 + "\n";
            }
            textBlock1.Text += "\n" + "всего " + query2.Count();
        }
        private void Join_Click(object sender, RoutedEventArgs e)
        {
            var query2 = from m in context.Managers
                        join d in context.Departments on m.IdMainDep equals d.Id
                        select new { Manager = m.Surname + " " + m.Name,
                        Department = d.Name
                        };
            textBlock1.Text = "";
            var query = context.Managers.Join(context.Departments, m => m.IdMainDep, d => d.Id, (m, d) => new { Manager = m.Surname + " " + m.Name, Department = d.Name});
            foreach (var item in query)
            {
                textBlock1.Text += item.Manager + " - " + item.Department + "\n";
            }
            textBlock1.Text += "\n" + "всего " + query.Count();
        }
    }
}
