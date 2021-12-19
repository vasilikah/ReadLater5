using Data;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private ReadLaterDataContext _ReadLaterDataContext;
        public CategoryService(ReadLaterDataContext readLaterDataContext)
        {
            _ReadLaterDataContext = readLaterDataContext;
        }

        public Category CreateCategory(Category category)
        {
            _ReadLaterDataContext.Add(category);
            _ReadLaterDataContext.SaveChanges();
            return category;
        }

        public void UpdateCategory(Category category)
        {
            _ReadLaterDataContext.Update(category);
            _ReadLaterDataContext.SaveChanges();
        }

        public List<Category> GetCategories()
        {
            return _ReadLaterDataContext.Categories.ToList();
        }

        public Category GetCategory(int Id)
        {
            var category = _ReadLaterDataContext.Categories
                .Include(x => x.Bookmarks)
                .Where(c => c.ID == Id).FirstOrDefault();
            if (category == null)
            {
                throw new NullReferenceException("Category is null");
            }
            return category;

        }

        public Category GetCategory(string Name)
        {
            return _ReadLaterDataContext.Categories.Where(c => c.Name == Name).FirstOrDefault();
        }

        public void DeleteCategory(Category category)
        {
            var bookmark = _ReadLaterDataContext.Bookmark.Where(b => b.CategoryId == category.ID).FirstOrDefault();
            if (bookmark == null)
            {
                _ReadLaterDataContext.Categories.Remove(category);
            }
            else
            {
                throw new Exception("The category cannot be deleted because is used in some bookmark");
            }
            _ReadLaterDataContext.SaveChanges();
        }
    }
}
