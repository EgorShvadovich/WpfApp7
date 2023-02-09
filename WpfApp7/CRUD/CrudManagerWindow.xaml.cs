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

namespace WpfApp7.CRUD
{
    /// <summary>
    /// Логика взаимодействия для CrudManagerWindow.xaml
    /// </summary>
    public partial class CrudManagerWindow : Window
    {
        public CrudManagerWindow()
        {
            InitializeComponent();
            DataContext = Owner;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = (Owner as disconnect);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (DepartamentsCombo.SelectedValue is Sales.Enities.Department deparment)
            {
                Guid id = deparment.Id;
                MessageBox.Show(
                    $"{ManagerSurname.Text} - {ManagerName.Text} - {ManagerSecname.Text} - {id}"
                    );
            }
            else
            {
                MessageBox.Show("Выберете отдел!");
            }
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
