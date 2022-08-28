using System.ComponentModel.DataAnnotations;

namespace MVCTestProject.DataModels
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Дата регистрации")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }
    }
}