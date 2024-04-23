
namespace LibraryWebApi.Tests.Services
{
    public class TestBookRepository : IDisposable
    {
        private LibrarySqliteDbContext _context;
        private Mock<IConfiguration> _configuration;
        private Mock<IConfigurationSection> _configurationSection;
        private BookRepositoy repo;
        private IMapper _mapper;
     
        public TestBookRepository()
        {
            var options = new DbContextOptionsBuilder<LibrarySqliteDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new LibrarySqliteDbContext(options);
            _context.Database.EnsureCreated();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfiles());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            _configuration = new Mock<IConfiguration>();
            _configurationSection = new Mock<IConfigurationSection>();
            repo = new BookRepositoy(_context, _configuration.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnBookList()
        {
            /// Arrange     

            /// Act
            var result = await repo.GetAllBooksAsync();

            /// Assert
            result.Should().HaveCount(MockData.GetBooksData().Count);
        }

        [Fact]
        public async Task CreateAsync_AddNewBook_Success()
        {
            /// Arrange          
            var newBook = MockData.NewBook();

            /// Act
            await repo.CreateAsync(_mapper.Map<Book>(newBook));

            ///Assert
            int expectedRecordCount = MockData.GetBooksData().Count + 1;
            _context.Books.Count().Should().Be(expectedRecordCount);
        }

        [Fact]
        public async Task DeleteAsync_DeleteBook_Success()
        {
            /// Arrange
            var id = 1;

            /// Act
            await repo.DeleteAsync(id);

            ///Assert
            int expectedRecordCount = MockData.GetBooksData().Count - 1;
            _context.Books.Count().Should().Be(expectedRecordCount);
        }

        [Fact]
        public async Task DeleteAsync_DeleteBook_Return_null()
        {
            /// Arrange
            var id = 10;
         
            /// Act
            var result = await repo.DeleteAsync(id);

            ///Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_UpdateBook_Success()
        {
            /// Arrange   
            var id = 2;

            var newBook = _mapper.Map<Book>(MockData.NewBook());
            var selectedBook = MockData.GetBooksData().FirstOrDefault(x => x.Id == id);       
            selectedBook!.Author = newBook.Author;
            selectedBook!.Title = newBook.Title;
            selectedBook!.Description = newBook.Description;
            /// Act
            var result = await repo.UpdateAsync(id, selectedBook!);

            ///Assert
            result?.Id.Should().Be(id);
            result?.Author.Should().Be(newBook.Author);
            result?.Title.Should().Be(newBook.Title);
            result?.Description.Should().Be(newBook.Description);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
