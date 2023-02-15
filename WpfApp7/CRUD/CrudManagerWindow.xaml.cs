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
        public Sales.Enities.Managers? Manager { get; set; }
        public CrudManagerWindow()
        {
            InitializeComponent();
            DataContext = Owner;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = (Owner as disconnect);
            if (Manager is null)  // режим "C" - добавление отдела
            {
                Manager = new Sales.Enities.Managers()
                {
                    Id = Guid.NewGuid()
                };
                ButtonDelete.IsEnabled = false;
            }
            else  // Режимы "UD" - есть переданный отдел для изменения/удаления
            {
                ButtonDelete.IsEnabled = true;
            }
            ManagerId.Text = Manager.Id.ToString();
            ManagerSurname.Text = Manager.Surname;
            ManagerName.Text = Manager.Name;
            ManagerSecname.Text = Manager.Secname;
            DepartamentsCombo.SelectedItem = Manager.IdMainDep.ToString();
           
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
