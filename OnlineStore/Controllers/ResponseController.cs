using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Contexts;
using OnlineStore.Entities;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/delivery")]
public class DeliveryMethodController : Controller
{
    private readonly OnlineStoreContext _context;


    public DeliveryMethodController(OnlineStoreContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDelivery()
    {
        var delivery = new DeliveryMethod
        {
            Name = Random.Shared.Next().ToString()
        };

        await _context.AddAsync(delivery);
        await _context.SaveChangesAsync();

        return Json(delivery);
    }


    [HttpGet("list")]
    public async Task<IActionResult> GetListDeliveryAsync()
    {
        var resultList = await _context.DeliveryMethod
            .Select(e => e.Name)
            .ToListAsync();

        return Json(resultList);
    }
}