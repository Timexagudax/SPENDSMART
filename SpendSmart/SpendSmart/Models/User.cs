using System.ComponentModel.DataAnnotations;

namespace SpendSmart.Models
{
    public class User
    {
    [Key]

    public int UserId { get; set; }
        [Required(ErrorMessage ="User Name is required")]
        [StringLength(100)]

        public string Name { get; set; }
        [Required]
        [Range(10, 120, ErrorMessage ="Age must be between 10 and 120")]

        public int Age { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(200)]

        public string Address { get; set; }

        public string profession { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
    }
}
