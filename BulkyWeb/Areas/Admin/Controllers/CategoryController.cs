using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    // Indicates which area the CategoryController is a part of. Since it allows us to 
    // perform general Admin functions like add, delete, update, it is in the Admin area. 
    [Area("Admin")]
    // This will enable authorization of the role of the person, i.e., a person will only 
    // be able to access this webpage if they are of role ADMIN. This applies it globally
    // on the entire Category Controller.
    //[Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var obnjCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(obnjCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        // Method that accepts the post request.
        [HttpPost]
        public IActionResult Create(Category obj)
        {

            // Checks if the Display order input value is the same as the Name value.
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                // Will return custom error message below the Name input box.
                ModelState.AddModelError("Name", "The Display Order cannot exactly match the name");
            }

            /*// Checks if the input for Name is the word test and if it is not null. If yes, then error.
			if (obj.Name != null && obj.Name.ToLower() == "test")
			{
				// Will return custom error message. Here, "" means this error is not displayed
                // below any of the input fields but is directly displayed in the summary.
				ModelState.AddModelError("", "Test is an invalid value");
			}*/

            // Examines all the data annotations to validate user input.
            // Checks if all the validations are satisfied or not.
            if (ModelState.IsValid)
            {
                // Adds the newly input category to the table.
                _unitOfWork.Category.Add(obj);
                // Saves all the changes in the database.
                _unitOfWork.Save();
                // This will display a notification on the page indicating success of action.
                TempData["success"] = "Category created successfully";

                // Reidrects user to the Index page after adding the new category.
                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            // Checks if the dispay order (Id) exists or if it is zero.
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Checks if such an id exists in the database.
            // Only works with the primary key of the data in the table.
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            // Whether there is any record will be checked first. Works with any type of column data.
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            // Allows us to do more checks on the data being retrieved.
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            // Not found.
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            // If it is non-zero, then show in View.
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                // This will display a notification on the page indicating success of action.
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }

            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            // Not found.
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            // If it is non-zero, then show in View.
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            // This will display a notification on the page indicating success of action.
            TempData["success"] = "Category deletd successfully";
            return RedirectToAction("Index", "Category");
        }
    }
}
