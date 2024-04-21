using Restoraunt.Areas.Identity.Data;
using System.Collections.Generic;

namespace Restoraunt.Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Product>();
        }

        public RestorauntUser RestorauntUser { get; set; }
        public IList<Product> ProductList { get; set; }
    }
}
