using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_Manager.Models;
using Project_Manager.Services;

namespace Project_Manager.Pages.Clients
{
    public class DeleteModel : PageModel
    {
        private readonly ClientService _clientService;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(ClientService clientService, ILogger<DeleteModel> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public ClientInfo? Client { get; set; }

        public IActionResult OnGet()
        {
            var clients = _clientService.GetClients();
            Client = clients.FirstOrDefault(c => c.Id == Id);

            if (Client == null)
            {
                return RedirectToPage("/Clients/Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                _clientService.DeleteClient(Id);
                return RedirectToPage("/Clients/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting client.");
                return Page();
            }
        }
    }
}
