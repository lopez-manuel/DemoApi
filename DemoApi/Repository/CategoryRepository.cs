using DemoApi.Data;
using DemoApi.Models;
using DemoApi.Repository.IRepository;

namespace DemoApi.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly DemoApiDbContext _context;

    public CategoryRepository(DemoApiDbContext context)
    {
        _context = context;
    }
    
    public ICollection<Category> GetCategories()
    {
        return _context.Categories.OrderBy( c => c.Name ).ToList();
    }

    public Category? GetCategory(int id)
    {
        return _context.Categories.FirstOrDefault( c => c.Id == id);
    }

    public bool CategoryExists(int id)
    {
        return _context.Categories.Any(c => c.Id == id);
    }

    public bool CategoryExists(string name)
    {
        return _context.Categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public bool CreateCategory(Category category)
    {
        category.CreationDate = DateTime.Now;
        _context.Categories.Add(category);
        return SaveChanges();
    }

    public bool UpdateCategory(Category category)
    {
        category.LastUpdateDate = DateTime.Now;
        _context.Categories.Update(category);
        return SaveChanges();
    }
    
    public bool DeleteCategory(Category category)
    {
        _context.Categories.Remove(category);
        return SaveChanges();
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}