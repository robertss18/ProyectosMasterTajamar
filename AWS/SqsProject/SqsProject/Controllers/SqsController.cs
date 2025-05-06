using Microsoft.AspNetCore.Mvc;
using SqsProject.Service;

namespace SqsProject.Controllers
{
    public class SqsController : Controller
    {
        private readonly SqsService _sqsService;
        public SqsController(SqsService sqsService)
        {
            _sqsService = sqsService;
        }

        public async Task<IActionResult> Index()
        {
            var mensajes = await _sqsService.ReceiveMessageAsync(); 
            var totalMensajes = await _sqsService.GetApproximateMessagesInQueueAsync();

            ViewData["TotalMensajes"] = totalMensajes;
            return View(mensajes);

        }

        public async Task<IActionResult> Delete(string receiptHandle)
        {
            await _sqsService.DeleteMessageAsync(receiptHandle);
            return RedirectToAction("Index");
        }

        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("messageBody", "El mensaje no puede estar vacío.");
                return View();
            }
            await _sqsService.SendMessageAsync(message);
            return RedirectToAction("Index");
        }

    }
}
