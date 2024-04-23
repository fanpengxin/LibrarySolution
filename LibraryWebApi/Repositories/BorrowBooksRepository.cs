using LibraryWebApi.Data;
using LibraryWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Repositories
{
    public class BorrowBooksRepository : IBorrowBooksRepository
    {
        private LibrarySqliteDbContext _context;

        /// <summary>
        /// BorrowBooksRepository Constructor
        /// </summary>
        /// <param name="context"></param>
        public BorrowBooksRepository(LibrarySqliteDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// BorrowBooksRepository CreateAsync
        /// </summary>
        /// <param name="borrowBooks"></param>
        /// <returns>New BorrowBooks</returns>
        public async Task<BorrowBooks> CreateAsync(BorrowBooks borrowBooks)
        {
            await _context.BorrowBooks.AddAsync(borrowBooks);
            await _context.SaveChangesAsync();
            return borrowBooks;
        }
        /// <summary>
        /// BorrowBooksRepository DeleteAsync
        /// </summary>
        /// <param name="borrowBooksId"></param>
        /// <returns>Deleted BorrowBooks, return null if not found.</returns>
        public async Task<BorrowBooks?> DeleteAsync(int borrowBooksId)
        {
            var borrowBooks = await _context.BorrowBooks.FirstOrDefaultAsync(item => item.Id == borrowBooksId);
            if (borrowBooks is not null)
            {
                _context.BorrowBooks.Remove(borrowBooks!);
                await _context.SaveChangesAsync();
                return borrowBooks;
            }

            return null;
        }

        /// <summary>
        /// BorrowBooksRepository GetAllBorrowBooksAsync
        /// </summary>
        /// <returns>list of BorrowBooks</returns>
        public async Task<IEnumerable<BorrowBooks>> GetAllBorrowBooksAsync()
        {
            return await _context.BorrowBooks.ToListAsync();
        }
        /// <summary>
        /// BorrowBooksRepository GetByIdAsync
        /// </summary>
        /// <param name="borrowBooksId"></param>
        /// <returns>BorrowBooks, return null if not found.</returns>
        public async Task<BorrowBooks?> GetByIdAsync(int borrowBooksId)
        {
            return await _context.BorrowBooks.FirstOrDefaultAsync(item => item.Id == borrowBooksId);
        }

        /// <summary>
        /// BorrowBooksRepository UpdateAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="borrowBooks"></param>
        /// <returns>Updated BorrowBooks, return null if not found.</returns>
        public async Task<BorrowBooks?> UpdateAsync(int id, BorrowBooks borrowBooks)
        {
            try
            {
                var existingBorrowBook = await _context.BorrowBooks.FirstOrDefaultAsync(x => x.Id == id);
                if (existingBorrowBook is null)
                {
                    return null;
                }
                existingBorrowBook.BookId = borrowBooks.BookId;
                existingBorrowBook.BorrowerId = borrowBooks.BorrowerId;
                existingBorrowBook.ExpireDate = borrowBooks.ExpireDate;

                _context.BorrowBooks.Update(existingBorrowBook);
                await _context.SaveChangesAsync();
                return existingBorrowBook;

            }
            catch (Exception ex)
            {
                return borrowBooks;
            }
        }
    }
}
