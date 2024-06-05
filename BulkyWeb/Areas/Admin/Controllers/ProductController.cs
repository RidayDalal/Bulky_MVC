using System.Collections.Generic;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    // Indicates which area the CategoryController is a part of. Since it allows us to 
    // perform general Admin functions like add, delete, update, it is in the Admin area. 
    [Area("Admin")]
    // This will enable authorization of the role of the person, i.e., a person will only 
    // be able to access this webpage if they are of role ADMIN. This applies it globally
    // on the entire Category Controller.
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            // Temporary data to be passed to the View. This data is NOT present in the
            // model, so here, we will be using a ViewBag for returning data.
            // First pass data as IEnumerable.
            
            // Key-value pair, Key is .CategoryList, and value is the
            // CategoryList that we will return to View().
            //ViewBag.CategoryList = CategoryList;

            // In case of ViewData, it is of type Dictionary. So here, we will
            // pass the key-value pair like how we do for a dictionary, dict[key] = value.
            // For this, in Create View, we will explicitely convert its type.
            //ViewData["CategoryList"] = CategoryList;

            //
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product(),
            };

            if (id == null || id == 0)
            {
                // Create
                return View(productVM);
            }
            else
            {
                // Update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        // Method that accepts the post request.
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            // Examines all the data annotations to validate user input.
            // Checks if all the validations are satisfied or not.
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    // Firstly, we configure the location where we will be storing the images.
                    // Renames the file to some other random name.
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    // Navigates to the product path by combining and creating a new path.
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // Delete the old image.
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        // Checks if there is still an image on this file path.
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            // If yes, then delete the image on this file path.
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    // Here, we actually store the images to the path we constructed above.
                    using ( var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create) )
                    {
                        // Copies the file to the new location configured above.
                        file.CopyTo(fileStream);
                    }
                    // Saving the image in the Product ImageUrl.
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                // If the product Id is zero, this means that no such product exists yet, so create.
                if (productVM.Product.Id == 0)
                {
                    // Adds the newly input category to the table.
                    _unitOfWork.Product.Add(productVM.Product);
                } 
                else
                {
                    // Updates the input category in the table.
                    _unitOfWork.Product.Update(productVM.Product);
                }
                
                // Saves all the changes in the database.
                _unitOfWork.Save();
                // This will display a notification on the page indicating success of action.
                TempData["success"] = "Product created successfully";

                // Reidrects user to the Index page after adding the new category.
                return RedirectToAction("Index", "Product");
            } else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        /*public IActionResult Edit(int? id)
        {
            // Checks if the dispay order (Id) exists or if it is zero.
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Checks if such an id exists in the database.
            // Only works with the primary key of the data in the table.
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            // Whether there is any record will be checked first. Works with any type of column data.
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            // Allows us to do more checks on the data being retrieved.
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            // Not found.
            if (productFromDb == null)
            {
                return NotFound();
            }
            // If it is non-zero, then show in View.
            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                // This will display a notification on the page indicating success of action.
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
            }

            return View();

        }*/

        #region API CALLS

        // Handles the API call from DataTables API.
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        #endregion

        #region API CALLS

        // Handles the API call from DataTables API.
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            // If the product retrieved with given id is null, it means there is no such record,
            // and that there is probably an error in retreiving the data.
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error whille deleting" });
            }

            // Delete the old image.
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            // Checks if there is still an image on this file path.
            if (System.IO.File.Exists(oldImagePath))
            {
                // If yes, then delete the image on this file path.
                System.IO.File.Delete(oldImagePath);
            }

            // Removes the entire record from the table.
            _unitOfWork.Product.Remove(productToBeDeleted);
            // Saves the changes made in the table.
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
