using LibraryWebApi.Dtos;
using LibraryWebApi.Entities;

namespace LibraryWebApi.Repositories
{
    public interface IBorrowerRepository
    {
        Task<Borrower> CreateAsync(Borrower borrower);
        Task<Borrower?> DeleteAsync(int borrowerId);
        Task<BorrowerDto?> GetByIdAsync(int borrowerId);
        Task<IEnumerable<Borrower>> GetAllBorrowersAsync();
        Task<Borrower?> UpdateAsync(int id, Borrower borrower);
    }
}
