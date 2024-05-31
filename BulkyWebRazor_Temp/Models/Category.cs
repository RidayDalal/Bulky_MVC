using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyWebRazor_Temp.Models
{
    public class Category
    {
        // The Id property is the primary key of the table. However, in this case, since the 
        // name is Id, we don't need data annotation.
        [Key]
        public int Id { get; set; }

        // This data annotation will make the Name column of "NOT NULL" type in the table.
        [Required]
        [DisplayName("Category Name")]
        public String Name { get; set; }

        // Data annotation that defines text that will
        // be displayed to describe this property to the user on screen.
        [DisplayName("Display Order")]
        // Data annotation that represents min, max range of values.
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
