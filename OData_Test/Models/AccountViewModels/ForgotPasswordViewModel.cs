using System.ComponentModel.DataAnnotations;

namespace OData_Test.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}