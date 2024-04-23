using LibraryWebApi.Entities;

namespace LibraryWebApi.Repositories
{
    public interface IBorrowBooksRepository
    {
        Task<BorrowBooks> CreateAsync(BorrowBooks borrowBooks);
        Task<BorrowBooks?> DeleteAsync(int borrowBooksId);
        Task<BorrowBooks?> GetByIdAsync(int borrowBooksId);
        Task<IEnumerable<BorrowBooks>> GetAllBorrowBooksAsync();
        Task<BorrowBooks?> UpdateAsync(int id, BorrowBooks borrowBooks);
    }
}
