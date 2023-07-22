using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Contexts;
using OnlineStore.Entities;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/category")]
public class CategoryController : Controller
{
    private readonly OnlineStoreContext _context;
    private readonly IConfigurationProvider _configurationProvider;

    public CategoryController(OnlineStoreContext context, IConfigurationProvider configurationProvider)
    {
        _context = context;
        _configurationProvider = configurationProvider;
    }

    [HttpGet("list/block")]
    public async Task<IActionResult> GetCategoryBlock(int start, int count)
    {
        int countSkip = start * count;

        var result = await _context.Categories
            .AsNoTracking()
            .Skip(countSkip)
            .Take(count)


            .ProjectTo<CategoryBlockDto>(_configurationProvider)
            .ToListAsync();

        return Json(result);
    }

    [HttpGet("list/hierarchy")]
    public async Task<IActionResult> GetListAsHierarchy()
    {
        var resultArray = await _context
            .Categories
            .AsNoTracking()
            .ProjectTo<CategoryHierarchyDto>(_configurationProvider)
            .ToListAsync();

        var parentCategories = resultArray
            .Where(c => c.ParentId == null)
            .ToArray();

        foreach (var category in resultArray)
        {
            category.Children = resultArray
                .Where(kid => kid.ParentId == category.Id)
                .ToArray();
        }

        return Json(parentCategories);
    }
    
    

    [HttpGet("list/linear")]
    public async Task<IActionResult> GetListAsLinear()
    {
        var result = await _context.Categories
            .AsNoTracking()
            .ProjectTo<CategoryLinearDto>(_configurationProvider)
            .ToListAsync();

        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .ProjectTo<CategoryInfoDto>(_configurationProvider)
            .FirstOrDefaultAsync();

        return category == null ? NotFound() : Json(category);
    }
}