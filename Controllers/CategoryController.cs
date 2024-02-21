using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices] BlogDataContext context)
    {
        try
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch 
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("ERRO-00 Falha interna no servidor"));
        }
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] int id)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<List<Category>>("ERRO-09 Conteúdo não encontrado"));

            return Ok(category);
        }
        catch
        {
            return StatusCode(404, new ResultViewModel<List<Category>>("ERRO-00 Falha interna no servidor"));
        }
    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context)
    {

        if(!ModelState.IsValid) 
            return BadRequest(ModelState.Values); 

        try
        {
            var category = new Category
            {
                Id = 0,
                Posts = [],
                Name = model.Name,
                Slug = model.Slug
            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", category);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "ERRO-01 Não foi possível incluir a categoria");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "ERRO-02 Falha interna no servidor");
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return Ok(category);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "ERRO-03 Não foi possível alterar a categoria");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "ERRO-04 Falha interna no servidor");
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> Delete(
        [FromRoute] int id,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "ERRO-05 Não foi possível excluir a categoria");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "ERRO-06 Falha interna no servidor");
        }
    }
}
