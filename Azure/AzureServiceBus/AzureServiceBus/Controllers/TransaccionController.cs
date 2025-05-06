using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureServiceBus.Data;
using AzureServiceBus.Models;
using AzureServiceBus.Service;
using System.Text.Json;

namespace AzureServiceBus.Controllers
{
    public class TransaccionController : Controller
    {
        private readonly DataDbContext _context;
        private readonly IServiceBusService _serviceBusService;


        public TransaccionController(DataDbContext context, IServiceBusService serviceBusService)
        {
            _context = context;
            _serviceBusService = serviceBusService;
        }

        // GET: Transaccion
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transacciones.ToListAsync());
        }

        // GET: Transaccion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccion = await _context.Transacciones
                .FirstOrDefaultAsync(m => m.TransaccionId == id);
            if (transaccion == null)
            {
                return NotFound();
            }

            return View(transaccion);
        }

        // GET: Transaccion/Create
        public IActionResult Create()
        {
            ViewBag.TiposTransacciones = Enum.GetValues(typeof(TipoTransaccion))
            .Cast<TipoTransaccion>()
            .Select(t => new SelectListItem
            {
                Value = t.ToString(),
                Text = t.ToString()
            })
            .ToList();

            ViewBag.EstadosTransacciones = Enum.GetValues(typeof(EstadoTransaccion))
                .Cast<EstadoTransaccion>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();

            return View();
        }

        // POST: Transaccion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Monto,TipoTransaccion,CuentaDestino,DetallesAdicionales")] Transaccion transaccion)
        {
            try
            {
                transaccion.FechaCreacion = DateTime.UtcNow;
                transaccion.Estado = EstadoTransaccion.Pendiente;

                _context.Transacciones.Add(transaccion);
                await _context.SaveChangesAsync();

                // 👉 Enviar correo al cliente (correo temporal aquí, podrías pedirlo en el formulario)
                var emailDestino = "roberts200018@gmail.com"; // O transaccion.EmailCliente si lo agregas

                string mensajeCorreo = $"Tienes una nueva transacción pendiente por aceptar. " +
                    $"Monto: {transaccion.Monto}, Tipo: {transaccion.TiposTransacciones}. " +
                    $"Haz clic aquí para aceptarla: https://tusitio.com/Transaccions/Aceptar/%7Btransaccion.TransaccionId%7D";

                await EmailService.SendEmailAsync(emailDestino, "Nueva Transacción Pendiente", mensajeCorreo);

                // Enviar a la cola
                var mensaje = JsonSerializer.Serialize(transaccion);
                await _serviceBusService.SendMessageToQueueAsync(mensaje);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear la transacción: " + ex.Message);
                return View(transaccion);
            }
        }


        // GET: Transaccion/Edit/5 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
            {
                return NotFound();
            }
            return View(transaccion);
        }

        // POST: Transaccion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransaccionId,Monto,TiposTransacciones,CuentaDestino,DetallesAdicionales,Estado,FechaCreacion,FechaProcesamiento,FechaNotificacion")] Transaccion transaccion)
        {
            if (id != transaccion.TransaccionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaccionExists(transaccion.TransaccionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaccion);
        }

        // GET: Transaccion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccion = await _context.Transacciones
                .FirstOrDefaultAsync(m => m.TransaccionId == id);
            if (transaccion == null)
            {
                return NotFound();
            }

            return View(transaccion);
        }

        // POST: Transaccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion != null)
            {
                _context.Transacciones.Remove(transaccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // MÉTODOS PARA LA REALIZACIÓN DE LAS COLAS Y SUSCRIPCIONES

        // POST: Transaccions/SendMessageToQueue
        // Envía manualmente un mensaje a la cola sin guardar una transacción.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessageToQueue(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                TempData["QueueStatus"] = "El mensaje para la cola no puede estar vacío.";
                return RedirectToAction(nameof(Create));
            }

            try
            {
                await _serviceBusService.SendMessageToQueueAsync(message);
                TempData["QueueStatus"] = "Mensaje enviado a la cola correctamente.";
            }
            catch (Exception ex)
            {
                TempData["QueueStatus"] = "Error al enviar el mensaje: " + ex.Message;
            }
            return RedirectToAction(nameof(Create));
        }

        // POST: Transaccions/ReceiveMessagesFromQueue
        // Recibe un mensaje de la cola y lo devuelve como resultado (para mostrarse vía AJAX).
        [HttpPost]
        public async Task<IActionResult> ReceiveMessagesFromQueue()
        {
            try
            {
                string message = await _serviceBusService.ReceiveMessageFromQueueAsync();
                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine($"📥 Mensaje recibido de la cola: {message}");
                    // Deserializar el mensaje para obtener los detalles de la transacción
                    var transaccion = JsonSerializer.Deserialize<Transaccion>(message);
                    // Buscar la transacción en la base de datos
                    var transaccionDb = await _context.Transacciones
                                                      .FirstOrDefaultAsync(t => t.TransaccionId == transaccion.TransaccionId);
                    if (transaccionDb != null)
                    {
                        // Actualizar el estado y las fechas de la transacción
                        transaccionDb.Estado = EstadoTransaccion.Exitosa;
                        transaccionDb.FechaProcesamiento = DateTime.UtcNow;
                        transaccionDb.FechaNotificacion = DateTime.UtcNow;

                        // Guardar los cambios en la base de datos
                        await _context.SaveChangesAsync();
                        Console.WriteLine($"✔️ Transacción con ID {transaccionDb.TransaccionId} actualizada.");
                        return Content($"📥 Mensaje recibido y transacción actualizada:<br/>{message}");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ No se encontró la transacción con ID {transaccion.TransaccionId}.");
                        return Content($"⚠️ No se encontró la transacción con ID {transaccion.TransaccionId}.");
                    }
                }
                else
                {
                    Console.WriteLine("⚠️ No hay mensajes en la cola.");
                    return Content("⚠️ No hay mensajes en la cola.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al recibir mensajes de la cola: {ex.Message}");
                return Content($"⚠️ Error al recibir mensajes de la cola: {ex.Message}");
            }


        }

        // POST: Transaccions/SendMessageToSubscription
        // Envía manualmente un mensaje a la suscripción (Topic).
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessageToSubscription(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                TempData["SubscriptionStatus"] = "El mensaje para la suscripción no puede estar vacío.";
                return RedirectToAction(nameof(Create));
            }

            try
            {
                await _serviceBusService.SendMessageToTopicAsync(message);
                TempData["SubscriptionStatus"] = "Mensaje enviado a la suscripción correctamente.";
            }
            catch (Exception ex)
            {
                TempData["SubscriptionStatus"] = "Error al enviar el mensaje a la suscripción: " + ex.Message;
            }
            return RedirectToAction(nameof(Create));
        }

        // POST: Transaccions/ReceiveMessagesFromSubscription
        // Recibe un mensaje de la suscripción y lo devuelve (para mostrarse vía AJAX).
        [HttpPost]
        public async Task<IActionResult> ReceiveMessagesFromSubscription()
        {
            try
            {
                string message = await _serviceBusService.ReceiveMessageFromSubscriptionAsync();
                if (!string.IsNullOrEmpty(message))
                {
                    return Content($"Mensaje recibido de la suscripción:<br/>{message}");
                }
                return Content("No hay mensajes en la suscripción.");
            }
            catch (Exception ex)
            {
                return Content($"Error al recibir mensajes de la suscripción: {ex.Message}");
            }
        }



        private bool TransaccionExists(int id)
        {
            return _context.Transacciones.Any(e => e.TransaccionId == id);
        }
    }
}
