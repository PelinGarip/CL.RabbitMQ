using CL.RabbitMQ.Core.Enums;
using CL.RabbitMQ.Services.Abstract;
using CL.RabbitMQ.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CL.RabbitMQ.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INotificationService _notificationService;
        public IndexModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [BindProperty]
        public NotificationModel NotificationModel { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = _notificationService.Send(NotificationTypes.Email, NotificationModel);
            return RedirectToPage("./Index");
        }
    }
}
