using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restoraunt.Models.ViewModels
{
    public class ProductVM
    {
        public Models.Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public IEnumerable<SelectListItem> ApplicationTypeSelectList { get; set; }
    }
}
