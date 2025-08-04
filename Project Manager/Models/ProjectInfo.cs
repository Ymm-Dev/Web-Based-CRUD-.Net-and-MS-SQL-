using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Models
{
    public class ProjectInfo
    {
        public int Id { get; set; }

        [Display(Name = "Project Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Deadline")]
        public DateTime? Deadline { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Client")]
        public int ClientId { get; set; }

        public string? ClientName { get; set; }
    }
}