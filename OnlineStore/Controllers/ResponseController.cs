using Microsoft.AspNetCore.Mvc;
using OnlineStore.Contexts;
using OnlineStore.Entities;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/response")]
public class ResponseController: Controller
{
    private readonly OnlineStoreContext _context;

    public ResponseController(OnlineStoreContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDelivery()
    {
        var delivery = new DeliveryMethod()
        {
            Name = Random.Shared.Next().ToString()
        };

        await _context.AddAsync(delivery);
        await _context.SaveChangesAsync();

        return Json(delivery);
    }
}