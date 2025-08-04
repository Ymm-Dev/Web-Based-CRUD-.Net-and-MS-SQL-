using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_Manager.Models;
using Project_Manager.Services;

namespace Project_Manager.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly ProjectService _projectService;

        public IndexModel(ProjectService projectService)
        {
            _projectService = projectService;
        }

        public List<ProjectInfo> ProjectList { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public void OnGet()
        {
            ProjectList = _projectService.GetProjects(SearchTerm);
        }
    }
}
