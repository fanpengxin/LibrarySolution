using Dapper;
using LibraryWebApi.Data;
using LibraryWebApi.Dtos;
using LibraryWebApi.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibraryWebApi.Repositories
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private LibrarySqliteDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// BorrowerRepository Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        public BorrowerRepository(LibrarySqliteDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        /// <summary>
        /// BorrowerRepository CreateAsync
        /// </summary>
        /// <param name="borrower"></param>
        /// <returns>New Borrower</returns>
        public async Task<Borrower> CreateAsync(Borrower borrower)
        {
            await _context.Borrowers.AddAsync(borrower);
            await _context.SaveChangesAsync();
            return borrower;
        }
        /// <summary>
        /// BorrowerRepository DeleteAsync
        /// </summary>
        /// <param name="borrowerId"></param>
        /// <returns>Deleted Borrower, return null if not found.</returns>
        public async Task<Borrower?> DeleteAsync(int borrowerId)
        {
            var borrower = await _context.Borrowers.FirstOrDefaultAsync(item => item.Id == borrowerId);
            if (borrower is not null)
            {
                _context.Borrowers.Remove(borrower!);
                await _context.SaveChangesAsync();
                return borrower;
            }

            return null;
        }
        /// <summary>
        /// BorrowerRepository GetAllBorrowersAsync
        /// </summary>
        /// <returns>List of Borrowers.</returns>
        public async Task<IEnumerable<Borrower>> GetAllBorrowersAsync()
        {
            return await _context.Borrowers.ToListAsync();
        }

        /// <summary>
        /// BorrowerRepository GetByIdAsync
        /// </summary>
        /// <param name="borrowerId"></param>
        /// <returns>Borrower with all borrowed books, return null if not found.</returns>
        public async Task<BorrowerDto?> GetByIdAsync(int borrowerId)
        {
            try
            {
                var borrower = await _context.Borrowers.FirstOrDefaultAsync(item => item.Id == borrowerId);
                if (borrower is null)
                {
                    return null;
                }

                using (IDbConnection cnn = new SqliteConnection(_configuration.GetConnectionString("Default")))
                {
                    var results = cnn.QueryMultiple(@"
                     SELECT * FROM Borrower WHERE Id=@borrowerId; 
                     SELECT bk.* FROM Book bk JOIN BorrowBooks bbk ON bk.ID=bbk.BookId 
                    JOIN Borrower bw ON bbk.BorrowerId = bw.ID WHERE bw.ID = borrowerId
                ", new { borrowerId });
                    var borrowerDto = results.ReadSingle<BorrowerDto>();
                    var bookDtos = results.Read<BookDto>();
                    borrowerDto.BookDtos.AddRange(bookDtos);
                    return borrowerDto;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// BorrowerRepository UpdateAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="borrower"></param>
        /// <returns>Updated borrower, return null if not found.</returns>
        public async Task<Borrower?> UpdateAsync(int id, Borrower borrower)
        {
            try
            {
                var existingBorrower = await _context.Borrowers.FirstOrDefaultAsync(x => x.Id == id);
                if (existingBorrower is null)
                {
                    return null;
                }
                existingBorrower.FirstName = borrower.FirstName;
                existingBorrower.LastName = borrower.LastName;
                _context.Borrowers.Update(existingBorrower);
                await _context.SaveChangesAsync();
                return existingBorrower;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
