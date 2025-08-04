using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_Manager.Models;
using Project_Manager.Services;

namespace Project_Manager.Pages.Clients
{
    public class EditModel : PageModel
    {
        private readonly ClientService _clientService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(ClientService clientService, ILogger<EditModel> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [BindProperty]
        public ClientInfo Client { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var client = _clientService.GetClientById(id);

            if (client == null)
            {
                TempData["ErrorMessage"] = "Client not found.";
                return RedirectToPage("/Clients/Index");
            }

            Client = client;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please correct the errors in the form.");
                return Page();
            }

            try
            {
                _clientService.UpdateClient(Client);
                TempData["SuccessMessage"] = "Client updated successfully!";
                return RedirectToPage("/Clients/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating client.");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the client.");
                return Page();
            }
        }
    }
}
