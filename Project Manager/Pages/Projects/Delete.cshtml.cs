using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_Manager.Models;
using Project_Manager.Services;

namespace Project_Manager.Pages.Projects
{
    public class DeleteModel : PageModel
    {
        private readonly ProjectService _projectService;

        public DeleteModel(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [BindProperty]
        public ProjectInfo Project { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var project = _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }

            Project = project;
            return Page();
        }

        public IActionResult OnPost()
        {
            _projectService.DeleteProject(Project.Id);
            return RedirectToPage("/Projects/Index");
        }
    }
}
