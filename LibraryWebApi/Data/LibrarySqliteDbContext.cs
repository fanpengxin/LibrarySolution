using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using LibraryWebApi.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LibraryWebApi.Data
{
    public class LibrarySqliteDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<BorrowBooks> BorrowBooks { get; set; }

        /// <summary>
        /// LibrarySqliteDbContext Constructor
        /// create database and tables if doesn't exist
        /// </summary>    
        public LibrarySqliteDbContext(DbContextOptions<LibrarySqliteDbContext> options) : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        ///Seed data: Book, Borrower, and BorrowBooks table
        /// </summary> 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book(
                    1,
                    "Hidden Pictures for Adults",
                    "A Search and Find Book for Kids and Adults .The Hidden Objects In The Pictures. Challenge Activities For Adults",
                    "Savannah Scarlett"
                ),
                new Book(
                    2,
                    "Born In 1980 Word Search Large Print",
                    "Large Print Word Search Books For Adults Born In 1980, British Themed Wordsearch Puzzles",
                    "Uncle LK Young"
                ),
                new Book(
                    3,
                    "Murdle: More Killer Puzzles",
                    "100 Fiendishly Foul Murder Mystery Logic Puzzles",
                    "G. T. Karber"
                )
              );
            modelBuilder.Entity<Borrower>().HasData(
                new Borrower(
                    1,
                    "George",
                    "RR Martin"
                ),
                new Borrower(
                    2,
                    "James",
                    "Elroy"
                ),
                new Borrower(
                    3,
                    "Douglas",
                    "Karber"
                )
              );

            modelBuilder.Entity<BorrowBooks>().HasData(
                new BorrowBooks(
                    1,
                    1,
                    1,
                    new DateTime(2024, 10, 22)
                ),
                new BorrowBooks(
                    2,
                    1,
                    2,
                    new DateTime(2024, 10, 22)
                ),
               new BorrowBooks(
                    3,
                    1,
                    3,
                    new DateTime(2024, 10, 22)
                )
              );

            base.OnModelCreating(modelBuilder);
        }
    }
}
