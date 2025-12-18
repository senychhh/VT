using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Kolbasin_lab1.Data;

namespace Kolbasin_lab1.Areas.Admin.Pages.Dishes
{
    public class IndexModel : PageModel
    {
        private readonly Kolbasin_lab1.Data.AppDbContext _context;

        public IndexModel(Kolbasin_lab1.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Dish> Dish { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Dish = await _context.Dishes
                .Include(d => d.Category).ToListAsync();
        }
    }
}
