using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Services
{
    public interface IBookmarkService
    {
        Bookmark CreateBookmark(Bookmark bookmark);
        void UpdateBookmark(Bookmark bookmark);
        void DeleteBookmar(Bookmark bookmark);
        List<Bookmark> GetBookmarks();
        Bookmark GetBookmarksById(int Id);
    }
}
