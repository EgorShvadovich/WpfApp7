using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp7.EfContext
{
    public class Departament
    {
        public Guid Id { get; set; }                 // Набор свойств сущности в точности
        public String Name { get; set; } = null!;    // повторяет структуру таблицы

        public String ToShortString()
        {
            return Id.ToString()[..4] + "... " + Name;
        }
    }
}
