using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
	// This can be used in case you want to populate/make available all the properties
	// to the different handlers.
	//[BindProperties]
	public class EditModel : PageModel
	{

		private readonly ApplicationDbContext _db;

		// Makes sure that this property is avaliable to be used in the POST handler.
		[BindProperty]
		public Category Category { get; set; }

		public EditModel(ApplicationDbContext db)
		{
			_db = db;
		}
		public void OnGet(int? id)
		{
			if (id != null && id != 0)
			{
				Category = _db.Categories.Find(id);
			}
		}


		public IActionResult OnPost()
		{
			if (ModelState.IsValid)
			{
				_db.Categories.Update(Category);
				_db.SaveChanges();
				// This will display a notification on the page indicating success of action.
				TempData["success"] = "Category updated successfully";
				return RedirectToPage("Index");
			}
			return Page();
		}
	}
}
