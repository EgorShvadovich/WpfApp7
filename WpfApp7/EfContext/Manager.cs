using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp7.EfContext
{
    public class Manager
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = null!;
        public String Surname { get; set; } = null!;
        public String Secname { get; set; } = null!;
        public Guid IdMainDep { get; set; }
        public Guid? IdSecDep { get; set; }
        public Guid? IdChief { get; set; }

        public String ToShortString()
        {
            return $"{Surname} {Name} {Secname}";
        }
    }
}
