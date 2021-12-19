using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class BookmarkService : IBookmarkService
    {
        private ReadLaterDataContext _ReadLaterDataContext;

        public BookmarkService(ReadLaterDataContext readLaterDataContext)
        {
            _ReadLaterDataContext = readLaterDataContext;
        }

        public Bookmark CreateBookmark(Bookmark bookmark)
        {
            bookmark.CreateDate = DateTime.Now;
            if (!string.IsNullOrEmpty(bookmark.Category.Name))
            {
                var category = _ReadLaterDataContext.Categories
                    .FirstOrDefault(x => x.Name.ToLower().Equals(bookmark.Category.Name.ToLower()));
                if(category != null)
                {
                    bookmark.CategoryId = category.ID;
                    bookmark.Category = category;
                }
            }
            _ReadLaterDataContext.Add(bookmark);
            _ReadLaterDataContext.SaveChanges();
            return bookmark;
        }
        public void UpdateBookmark(Bookmark bookmark)
        {
            _ReadLaterDataContext.Update(bookmark);
            _ReadLaterDataContext.SaveChanges();
        }

        public List<Bookmark> GetBookmarks()
        {
            return _ReadLaterDataContext.Bookmark
                .OrderByDescending(x => x.CreateDate).ToList();
        }
        public Bookmark GetBookmarksById(int Id)
        {
            return _ReadLaterDataContext.Bookmark
                .Include(x => x.Category)
                .Where(b => b.ID == Id).FirstOrDefault();
        }

        public void DeleteBookmar(Bookmark bookmark)
        {
            _ReadLaterDataContext.Bookmark.Remove(bookmark);
            _ReadLaterDataContext.SaveChanges();
        }

    }


}
