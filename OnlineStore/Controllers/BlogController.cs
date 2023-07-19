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

    [HttpGet("block-list")]
    public async Task<IActionResult> GetBlockList(int start = 0, int count = 5)
    {
        int countSkip = count * start;

        var result = await _context
            .Blogs
            .AsNoTracking()
            .Skip(countSkip)
            .Take(count)
            .ProjectTo<BlogBlockDto>(_provider)
            .ToListAsync();

        return Json(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBlog(int id)
    {
        var blog = _context.Blogs
            .AsNoTracking()
            .Where(b => b.Id == id)
            .ProjectTo<BlogInfoDto>(_provider)
            .FirstOrDefault();

        return blog == null
            ? NotFound()
            : Json(blog);
    }

}