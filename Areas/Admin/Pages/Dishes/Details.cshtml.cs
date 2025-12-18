using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Kolbasin_lab1.Data;
using Microsoft.AspNetCore.Authorization;

namespace Kolbasin_lab1.Areas.Admin.Pages.Dishes
{
     [Authorize(Policy = "admin")]
    public class DetailsModel : PageModel
    {
        private readonly Kolbasin_lab1.Data.AppDbContext _context;

        public DetailsModel(Kolbasin_lab1.Data.AppDbContext context)
        {
            _context = context;
        }

        public Dish Dish { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FirstOrDefaultAsync(m => m.Id == id);

            if (dish is not null)
            {
                Dish = dish;

                return Page();
            }

            return NotFound();
        }
    }
}
