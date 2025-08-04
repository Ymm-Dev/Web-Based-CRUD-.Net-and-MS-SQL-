using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Models
{
    public class ClientInfo
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string Fname { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string Lname { get; set; } = string.Empty;

        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string? PhoneNum { get; set; }

        [Display(Name = "Company Name")]
        public string? Company { get; set; }
    }
}