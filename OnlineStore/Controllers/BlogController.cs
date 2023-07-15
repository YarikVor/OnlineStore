using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Contexts;
using OnlineStore.Dtos;
using OnlineStore.Entities;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/blog")]
public class BlogController : Controller
{
    private readonly IConfigurationProvider _provider;
    private readonly OnlineStoreContext _context;
    
    public BlogController(IConfigurationProvider provider, OnlineStoreContext context)
    {
        _provider = provider;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog()
    {
        var blog = new Blog()
        {
            Title = "Test",
            UserId = 2,
            Description = "Desc",
            PhotoUri = "http:\\\\333.com",
            TimeWriting = DateTimeOffset.Now
        };

        await _context.AddAsync(blog);

        await _context.SaveChangesAsync();

        return Json(blog);
    }

    [HttpGet("list/block")]
    public async Task<IActionResult> GetBlockList(int count = 5)
    {
        var result = await _context.Blog
            .ProjectTo<BlogBlockDto>(_provider)
            .ToListAsync();

        return Json(result);

    }
}