using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_Manager.Models;
using Project_Manager.Services;

namespace Project_Manager.Pages.Projects
{
    public class EditModel : PageModel
    {
        private readonly ProjectService _projectService;
        private readonly ClientService _clientService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(ProjectService projectService, ClientService clientService, ILogger<EditModel> logger)
        {
            _projectService = projectService;
            _clientService = clientService;
            _logger = logger;
        }

        [BindProperty]
        public ProjectInfo Project { get; set; } = new();

        public List<SelectListItem> Clients { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var project = _projectService.GetProjectById(id);

            if (project == null)
            {
                TempData["ErrorMessage"] = "Project not found.";
                return RedirectToPage("/Projects/Index");
            }

            Project = project;
            LoadClients();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadClients();
                ModelState.AddModelError(string.Empty, "Please fix the errors and try again.");
                return Page();
            }

            try
            {
                _projectService.UpdateProject(Project);
                TempData["SuccessMessage"] = "Project updated successfully!";
                return RedirectToPage("/Projects/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project.");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the project.");
                LoadClients();
                return Page();
            }
        }

        private void LoadClients()
        {
            Clients = _clientService
                .GetClients()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Fname} {c.Lname}"
                })
                .ToList();
        }
    }
}
