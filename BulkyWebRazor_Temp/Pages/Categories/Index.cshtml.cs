using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public List<Category> CategoryList { get; set; }
        
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        } 

        // This functionality is similar to what you would implement in the Controller in an MVC project.
        public void OnGet()
        {
            CategoryList = _db.Categories.ToList();
        }
    }
}
