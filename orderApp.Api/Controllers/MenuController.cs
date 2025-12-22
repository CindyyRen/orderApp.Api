using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApp.Domain.Entities;
using OrderApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using OrderApp.Application.DTOs.Menu;


namespace OrderApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class MenuController(ApplicationDbContext dbContext) : ControllerBase
{

    // GET: api/MenuItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetAll()
    {
        var items = await dbContext.MenuItems.ToListAsync();
        return Ok(items);
    }

    // GET: api/MenuItems/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MenuItem>> GetById(Guid id)
    {
        var item = await dbContext.MenuItems.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    // POST: api/MenuItems
    [HttpPost]
    public async Task<ActionResult<MenuItem>> Create(MenuItemDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var menuItem = MenuItem.Create(dto.Name, dto.Price,dto.Category,  dto.Description);
        await dbContext.MenuItems.AddAsync(menuItem);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = menuItem.Id }, menuItem);
    }

    // PUT: api/MenuItems/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, MenuItemDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var item = await dbContext.MenuItems.FindAsync(id);
        if (item == null) return NotFound();

        item.UpdateName(dto.Name);
        item.UpdatePrice(dto.Price);
        item.UpdateDescription(dto.Description);

        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/MenuItems/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await dbContext.MenuItems.FindAsync(id);
        if (item == null) return NotFound();

        dbContext.MenuItems.Remove(item);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }


}
