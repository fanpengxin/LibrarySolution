
namespace LibraryWebApi.Tests.Services
{
    public class TestBorrowerRepository : IDisposable
    {
        private LibrarySqliteDbContext _context;
        private Mock<IConfiguration> _configuration;
        private BorrowerRepository repo;
        private IMapper _mapper;
     
        public TestBorrowerRepository()
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
            repo = new BorrowerRepository(_context, _configuration.Object);
        }

        [Fact]
        public async Task GetAllBorrowersAsync_ReturnAllBorrowers()
        {
            /// Arrange                  

            /// Act
            var result = await repo.GetAllBorrowersAsync();

            /// Assert
            result.Should().HaveCount(MockData.GetBorrowersData().Count);
        }

        [Fact]
        public async Task CreateAsync_AddNewBorrower_Success()
        {
            /// Arrange         
            var newBorrower = MockData.NewBorrower();          

            /// Act
            await repo.CreateAsync(_mapper.Map<Borrower>(newBorrower));

            ///Assert
            int expectedRecordCount = MockData.GetBorrowersData().Count + 1;
            _context.Borrowers.Count().Should().Be(expectedRecordCount);
        }

        [Fact]
        public async Task DeleteAsync_DeleteBorrower_Success()
        {
            /// Arrange
            var id = 1;

            /// Act
            await repo.DeleteAsync(id);

            ///Assert
            int expectedRecordCount = MockData.GetBorrowersData().Count - 1;
            _context.Borrowers.Count().Should().Be(expectedRecordCount);
        }

        [Fact]
        public async Task DeleteAsync_DeleteBorrower_Return_null()
        {
            /// Arrange
            var id = 10;
         
            /// Act
            var result = await repo.DeleteAsync(id);

            ///Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_UpdateBorrower_Success()
        {
            /// Arrange   
            var id = 2;                     
            var newBorrower = _mapper.Map<Borrower>(MockData.NewBorrower());
            var selectedBorrower = MockData.GetBorrowersData().FirstOrDefault(x => x.Id == id);       
            selectedBorrower!.FirstName = newBorrower.FirstName;
            selectedBorrower!.LastName = newBorrower.LastName;

            /// Act
            var result = await repo.UpdateAsync(id, selectedBorrower!);

            ///Assert
            result?.Id.Should().Be(id);
            result?.FirstName.Should().Be(newBorrower.FirstName);
            result?.LastName.Should().Be(newBorrower.LastName);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
