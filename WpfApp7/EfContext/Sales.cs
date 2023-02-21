using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp7.EfContext
{
    public class Sales
    {
        public Guid Id { get; set; }
        public Guid IdManager { get; set; }
        public Guid IdProduct { get; set; }
        public int Count { get; set; }
        public DateTime Moment { get; set; }

        public String ToShortString()
        {
            return $"{IdProduct.ToString()[..4]} ({Count} шт) {Moment}";

        }
    }
}
