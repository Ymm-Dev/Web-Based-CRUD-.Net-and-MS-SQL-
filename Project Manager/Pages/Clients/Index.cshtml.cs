using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Project_Manager.Models;
using Project_Manager.Services;

namespace Project_Manager.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly ClientService _clientService;

        public IndexModel(ClientService clientService)
        {
            _clientService = clientService;
        }

        public List<ClientInfo> clientList = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public void OnGet()
        {
            clientList = _clientService.GetClients(SearchTerm);
        }
    }
}
