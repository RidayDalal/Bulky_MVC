using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bulky.Models
{
    public class Product
    {
        // The Id property is the primary key of the table. However, in this case, since the 
        // name is Id, we don't need data annotation.
        [Key]
        public int Id { get; set; }

        // This data annotation will make the Name column of "NOT NULL" type in the table.
        [Required]
        public string Title { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price for 50-100")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        // Foreign Key
        public int? CategoryId { get; set; }

        // Tells us that CategoryId is the foreign key property.
        // Category here is a navigation property that helps direct
        // us to the foreign key.
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        // Data Annotation that tells compiler to skip validation of this property
        // when compiling the web app.
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
