using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_Manager.Models;
using Project_Manager.Services;
using Microsoft.Extensions.Logging;

namespace Project_Manager.Pages.Projects
{
    public class CreateModel : PageModel
    {
        private readonly ProjectService _projectService;
        private readonly ClientService _clientService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ProjectService projectService, ClientService clientService, ILogger<CreateModel> logger)
        {
            _projectService = projectService;
            _clientService = clientService;
            _logger = logger;
        }

        [BindProperty]
        public ProjectInfo Project { get; set; } = new();

        public List<SelectListItem> Clients { get; set; } = new();

        public void OnGet()
        {
            LoadClients();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadClients();
                return Page();
            }

            try
            {
                _projectService.AddProject(Project);
                TempData["SuccessMessage"] = "Project added successfully!";
                return RedirectToPage("/Projects/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new project.");
                LoadClients();
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again later.");
                return Page();
            }
        }

        private void LoadClients()
        {
            Clients = _clientService.GetClients()
                        .Select(c => new SelectListItem
                        {
                            Value = c.Id.ToString(),
                            Text = $"{c.Fname} {c.Lname}"
                        }).ToList();
        }
    }
}
