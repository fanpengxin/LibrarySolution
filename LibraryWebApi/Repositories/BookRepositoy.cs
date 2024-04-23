using Dapper;
using LibraryWebApi.Data;
using LibraryWebApi.Dtos;
using LibraryWebApi.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibraryWebApi.Repositories
{
    public class BookRepositoy : IBookRepository
    {
        private readonly LibrarySqliteDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// BookReporsitoy Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        public BookRepositoy(LibrarySqliteDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        /// <summary>
        /// BookReporsitoy CreateAsync
        /// </summary>
        /// <param name="book"></param>
        /// <returns>New Book</returns>
        public async Task<Book> CreateAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }
        /// <summary>
        /// BookReporsitoy DeleteAsync
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>Deleted Book, return null if not found.</returns>
        public async Task<Book?> DeleteAsync(int bookId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(item => item.Id == bookId);
            if (book is not null)
            {
                _context.Books.Remove(book!);
                await _context.SaveChangesAsync();
                return book;
            }

            return null;
        }
        /// <summary>
        /// BookReporsitoy GetAllBooksAsync
        /// </summary>
        /// <returns>return list of books.</returns>
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }
        /// <summary>
        /// BookReporsitoy GetByIdAsync
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>return a book with a list of borrowers.</returns>
        public async Task<BookDto?> GetByIdAsync(int bookId)
        {
            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(item => item.Id == bookId);
                if (book is null)
                {
                    return null;
                }

                using (IDbConnection cnn = new SqliteConnection(_configuration.GetConnectionString("Default")))
                {
                    var results = cnn.QueryMultiple(@"
                    SELECT * FROM Book WHERE Id=@bookId; 
                    SELECT br.* FROM Borrower br JOIN BorrowBooks bbk ON br.ID=bbk.BorrowerId
                    JOIN Book bk ON bbk.BookId = bk.ID WHERE bk.ID = @bookId
                ", new { bookId });
                    var bookDto = results.ReadSingle<BookDto>();
                    var borrowers = results.Read<BorrowerDto>();
                    bookDto.BorrowerDtos.AddRange(borrowers);
                    return bookDto;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// BookReporsitoy UpdateAsync
        /// </summary>
        /// <param name="id"></param>
        /// /// <param name="book"></param>
        /// <returns>return updated book, return null if not found.</returns>
        public async Task<Book?> UpdateAsync(int id, Book book)
        {
            try
            {
                var existingBook = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
                if (existingBook is null)
                {
                    return null;
                }
                existingBook.Title = book.Title;
                existingBook.Description = book.Description;
                existingBook.Author = book.Author;

                _context.Books.Update(existingBook);
                await _context.SaveChangesAsync();
                return existingBook;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       
    }
}
