using AutoMapper;
using DemoApi.Models;
using DemoApi.Models.Dtos;
using DemoApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<ICollection<CategoryDto>> GetCategories()
    {
        var categories = _categoryRepository.GetCategories();
        var categoriesDto = _mapper.Map<ICollection<CategoryDto>>(categories);
        return Ok(categoriesDto);
    }
    
    
    [HttpGet("{id:int}", Name = "GetCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetCategory([FromRoute] int id)
    {
        var category = _categoryRepository.GetCategory(id);
        if(category == null)
            return NotFound();
        
        return Ok(_mapper.Map<CategoryDto>(category));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateCategory([FromBody] CreateCategoryDto category)
    {

        if (_categoryRepository.CategoryExists(category.Name))
        {
            ModelState.AddModelError(string.Empty, "Category with the same name already exists.");
            return BadRequest(ModelState);
        }

        var entity = _mapper.Map<Category>(category);

        var result = _categoryRepository.CreateCategory(entity);

        if (!result)
            return StatusCode(StatusCodes.Status500InternalServerError, "Category creation failed.");

        return CreatedAtRoute("GetCategory", new { id = entity.Id }, entity);
    }
    
    [HttpPatch("{id:int}", Name = "UpdateCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCategory([FromRoute] int id,[FromBody] CreateCategoryDto category)
    {

        if (!_categoryRepository.CategoryExists(id))
        {
            return NotFound();
        }

        if (_categoryRepository.CategoryExists(category.Name))
        {
            ModelState.AddModelError("Error", "Category with the same name already exists.");
            return BadRequest(ModelState);
        }

        var entity = _mapper.Map<Category>(category);
        entity.Id = id;
        

        var result = _categoryRepository.UpdateCategory(entity);

        if (!result)
            return StatusCode(StatusCodes.Status500InternalServerError, "Category update failed.");

        return NoContent();
    }
    
    [HttpDelete("{id:int}", Name = "DeleteCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteCategory([FromRoute] int id )
    {

        if (!_categoryRepository.CategoryExists(id))
        {
            return NotFound();
        }
        
        var result = _categoryRepository.DeleteCategory( _categoryRepository.GetCategory(id)! );

        if (!result)
            return StatusCode(StatusCodes.Status500InternalServerError, "Category delete failed.");

        return NoContent();
    }
    
    
}