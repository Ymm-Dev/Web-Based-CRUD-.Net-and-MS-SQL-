using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Project_Manager.Models;
using Project_Manager.Services;

namespace Project_Manager.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly ClientService _clientService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ClientService clientService, ILogger<CreateModel> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [BindProperty]
        public ClientInfo NewClient { get; set; } = new();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _clientService.AddClient(NewClient);
                TempData["SuccessMessage"] = "Client added successfully!";
                return RedirectToPage("/Clients/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new client.");
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again later.");
                return Page();
            }
        }
    }
}
