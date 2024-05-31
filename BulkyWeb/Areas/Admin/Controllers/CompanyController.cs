using System.Collections.Generic;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            
            return View(objCompanyList);
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

            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        // Method that accepts the post request.
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            // Examines all the data annotations to validate user input.
            // Checks if all the validations are satisfied or not.
            if (ModelState.IsValid)
            {
                // If the company Id is zero, this means that no such product exists yet, so create.
                if (companyObj.Id == 0)
                {
                    // Adds the newly input company to the table.
                    _unitOfWork.Company.Add(companyObj);
                } 
                else
                {
                    // Updates the input company in the table.
                    _unitOfWork.Company.Update(companyObj);
                }
                
                // Saves all the changes in the database.
                _unitOfWork.Save();
                // This will display a notification on the page indicating success of action.
                TempData["success"] = "Company created successfully";

                // Reidrects user to the Index page after adding the new company.
                return RedirectToAction("Index");

            } else
            {
                return View(companyObj);
            }
        }

        

        #region API CALLS

        // Handles the API call from DataTables API.
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        #endregion

        #region API CALLS

        // Handles the API call from DataTables API.
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            // If the product retrieved with given id is null, it means there is no such record,
            // and that there is probably an error in retreiving the data.
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error whille deleting" });
            }

            // Removes the entire record from the table.
            _unitOfWork.Company.Remove(companyToBeDeleted);
            // Saves the changes made in the table.
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
