using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAzure.Data;
using ProyectoAzure.Models;
using ProyectoAzure.Services;

namespace ProyectoAzure.Controllers
{
    public class TransaccionsController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IServiceBusService _serviceBusService;
        private static decimal SaldoDisponible = 1000000; // Monto inicial

        public TransaccionsController(AplicationDbContext context, IServiceBusService serviceBusService)
        {
            _context = context;
            _serviceBusService = serviceBusService;
        }

        // GET: Transaccions
        public async Task<IActionResult> Index()
        {
            var transacciones = await _context.Transacciones.ToListAsync();
            return View(transacciones);
        }

        public IActionResult Saldo()
        {
            ViewBag.SaldoDisponible = SaldoDisponible;
            return View();
        }

        public async Task<IActionResult> Notificaciones()
        {
            var notificaciones = await _context.Notificaciones.ToListAsync();
            return View(notificaciones);
        }

        // GET: Transacciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccion = await _context.Transacciones.FirstOrDefaultAsync(m => m.TransaccionId == id);
            if (transaccion == null)
            {
                return NotFound();
            }

            return View(transaccion);
        }

        // GET: Transaccions/Create
        public IActionResult Create()
        {
            ViewBag.SaldoDisponible = SaldoDisponible;
            return View();
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Monto,TipoTransaccion,CuentaDestino,DetallesAdicionales")] Transaccion transaccion)
        {
            var emailDestino = "roberts200018@gmail.com";

            try
            {
                transaccion.FechaCreacion = DateTime.UtcNow;

                if (transaccion.Monto > SaldoDisponible)
                {
                    // Saldo insuficiente → marcar como fallida
                    transaccion.Estado = "Fallida";
                }
                else
                {
                    // Saldo suficiente → marcar como pendiente
                    transaccion.Estado = "Pendiente";
                    SaldoDisponible -= transaccion.Monto; // Se descuenta solo si es válida
                }

                _context.Transacciones.Add(transaccion);
                await _context.SaveChangesAsync();

                // Si fue fallida, registrar el evento
                if (transaccion.Estado == "Fallida")
                {
                    var eventoTransaccion = new EventoTransaccion
                    {
                        TransaccionId = transaccion.TransaccionId,
                        TipoEvento = "Transacción Fallida",
                        Descripcion = $"La transacción con monto {transaccion.Monto:C} fue fallida debido a saldo insuficiente.",
                        FechaEvento = DateTime.UtcNow,
                        Transaccion = transaccion
                    };                    

                    _context.EventosTransaccion.Add(eventoTransaccion);
                    
                    await _context.SaveChangesAsync();                    

                    string mensajeCorreo = $"La transacción ha sido fallida" + " " +
                        $"Monto: {transaccion.Monto}, Tipo: {transaccion.TipoTransaccion}. ";

                    await EmailService.SendEmailAsync(emailDestino, "Nueva  aviso de Transacción Fallida", mensajeCorreo);

                    var mensaje = JsonSerializer.Serialize(transaccion);
                    await _serviceBusService.SendMessageToQueueAsyncNotification(mensaje);

                }
                else if (transaccion.Estado == "Pendiente")
                {
                    var eventoTransaccion = new EventoTransaccion
                    {
                        TransaccionId = transaccion.TransaccionId,
                        TipoEvento = "Transacción Exitosa",
                        Descripcion = $"La transacción con monto {transaccion.Monto:C} fue fallida debido a saldo insuficiente.",
                        FechaEvento = DateTime.UtcNow,
                        Transaccion = transaccion
                    };                   

                    _context.EventosTransaccion.Add(eventoTransaccion);
                   

                    await _context.SaveChangesAsync();

                    // Enviar correo para aceptar la transacción                    

                    string mensajeCorreo = $"Tienes una nueva transacción pendiente por aceptar. " +
                        $"Monto: {transaccion.Monto}, Tipo: {transaccion.TipoTransaccion}. " +
                        $"Haz clic aquí para aceptarla: https://practicaroberts18-gcg2byaxgbb4esab.canadacentral-01.azurewebsites.net/Transaccions/Aceptar/{transaccion.TransaccionId}";

                    await EmailService.SendEmailAsync(emailDestino, "Nueva Transacción Pendiente", mensajeCorreo);

                    // Enviar a la cola
                    var mensaje = JsonSerializer.Serialize(transaccion);
                    await _serviceBusService.SendMessageToQueueAsync(mensaje);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al procesar la transacción: " + ex.Message);
                return View(transaccion);
            }
        }


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
                        transaccionDb.Estado = "Realizado";
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

        [HttpPost]
        public async Task<IActionResult> ReceiveMessagesFromSubscription()
        {
            try
            {
                string message = await _serviceBusService.ReceiveMessageFromSubscriptionAsync();
                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine($"📥 Mensaje recibido de la suscripción: {message}");
                    return Content($"📥 Mensaje recibido de la suscripción:<br/>{message}");
                }
                else
                {
                    Console.WriteLine("⚠️ No hay mensajes en la suscripción.");
                    return Content("⚠️ No hay mensajes en la suscripción.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al recibir mensajes de la suscripción: {ex.Message}");
                return Content($"⚠️ Error al recibir mensajes de la suscripción: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Aceptar(int id, Notificacion notificacion)
        {
            var emailDestino = "roberts200018@gmail.com";

            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion == null)
                return NotFound();

            var notificaciones = new Notificacion
            {
                NotificacionId = notificacion.NotificacionId,
                TransaccionId = transaccion.TransaccionId,
                EmailCliente = emailDestino,
                EstadoNotificacion = "Exitosa",
                FechaEnvio = DateTime.UtcNow,
            };

            _context.Notificaciones.Add(notificaciones);

            if (transaccion.Estado != "Pendiente")
                return Content($"⚠️ La transacción con ID {id} no está pendiente. Estado actual: {transaccion.Estado}");

            transaccion.Estado = "Exitosa";
            transaccion.FechaProcesamiento = DateTime.UtcNow;

            // Guardar cambios
            await _context.SaveChangesAsync();         
            

            string mensajeCorreo = $"✅ Tu transacción con ID {transaccion.TransaccionId}, " +
                $"monto {transaccion.Monto:C}, tipo {transaccion.TipoTransaccion} ha sido *aceptada* correctamente.";

            await EmailService.SendEmailAsync(emailDestino, "Transacción Aceptada", mensajeCorreo);

            return Content($"✔️ Transacción con ID {id} fue aceptada y se ha enviado confirmación por correo.");
        }


        // GET: Transacciones/Edit/5
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

        // POST: Transacciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransaccionId,Monto,TipoTransaccion,CuentaDestino,DetallesAdicionales,Estado,FechaCreacion,FechaProcesamiento,FechaNotificacion")] Transaccion transaccion)
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

        // GET: Transacciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaccion = await _context.Transacciones.FirstOrDefaultAsync(m => m.TransaccionId == id);
            if (transaccion == null)
            {
                return NotFound();
            }

            return View(transaccion);
        }

        // POST: Transacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);
            if (transaccion != null)
            {
                _context.Transacciones.Remove(transaccion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TransaccionExists(int id)
        {
            return _context.Transacciones.Any(e => e.TransaccionId == id);
        }

    }
}