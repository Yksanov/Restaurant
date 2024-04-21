using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restoraunt.Models.ViewModels
{
    public class HomeVC
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
