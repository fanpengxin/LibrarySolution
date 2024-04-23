using LibraryWebApi.Dtos;
using LibraryWebApi.Entities;

namespace LibraryWebApi.Tests.Fixtures
{
    public class MockData
    {
        public static List<Book> GetBooksData()
        {
            return new List<Book>() {
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
            };
        }

        public static List<Book> LoadNewBooksData()
        {
            return new List<Book>() {
                new Book(
                    0,
                    "Hidden Pictures for Adults abc",
                    "A Search and Find Book for Kids and Adults .The Hidden Objects In The Pictures. Challenge Activities For Adults",
                    "Savannah Scarlett"
                ),
                new Book(
                    0,
                    "Born In 1980 Word Search Large Print abc",
                    "Large Print Word Search Books For Adults Born In 1980, British Themed Wordsearch Puzzles",
                    "Uncle LK Young"
                ),
                new Book(
                    0,
                    "Murdle: More Killer Puzzles abc",
                    "100 Fiendishly Foul Murder Mystery Logic Puzzles",
                    "G. T. Karber"
                )
            };
        }

        public static List<Book> GetEmptyBook()
        {
            return new List<Book>();
        }

        public static BookDto NewBook()
        {
            return new BookDto
            {
                Id = 0,
                Title = "Murdle: More Killer Puzzles",
                Description = "100 Fiendishly Foul Murder Mystery Logic Puzzles",
                Author = "G. T. Karber"
            };
        }

        public static List<Borrower> GetBorrowersData()
        {
            return new List<Borrower>() {
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
            };
        }

        public static List<Borrower> GetEmptyBorrower()
        {
            return new List<Borrower>();
        }

        public static Borrower NewBorrower()
        {
            return new Borrower(
                    0,
                    "Douglas",
                    "Karber"
                );
        }


        public static List<BorrowBooks> GetBorrowBooksData()
        {
            return new List<BorrowBooks>() {
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
            };
        }

        public static List<BorrowBooks> GetEmptyBorrowBooks()
        {
            return new List<BorrowBooks>();
        }

        public static BorrowBooks NewBorrowBooks()
        {
            return new BorrowBooks(
                    0,
                    1,
                    3,
                    new DateTime(2024, 10, 22)
                );
        }
    }
}
