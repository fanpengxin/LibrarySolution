using LibraryWebApi.Dtos;
using LibraryWebApi.Entities;

namespace LibraryWebApi.Repositories
{
    public interface IBookRepository
    {
        Task<Book> CreateAsync(Book book);
        Task<Book?> DeleteAsync(int bookId);
        Task<BookDto?> GetByIdAsync(int bookId);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> UpdateAsync(int id, Book book);
    }
}
