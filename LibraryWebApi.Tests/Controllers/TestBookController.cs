

namespace LibraryWebApi.Tests.Controllers
{
    public class TestBookController
    {
        private Mock<IBookRepository> _mockRepository;
        private Mock<ILogger<BookController>> _logger;
        private IMapper _mapper;
        private BookController bookController;

        public TestBookController()
        {
            _mockRepository = new Mock<IBookRepository>();
            _logger = new Mock<ILogger<BookController>>();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfiles());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            bookController = new BookController(_mockRepository.Object, _mapper, _logger.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturn200Status()
        {
            /// Arrange            
            _mockRepository.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(MockData.GetBooksData());           

            /// Act
            var result = (OkObjectResult)await bookController.GetAllBooksAsync();

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturn204NoContentStatus()
        {
            /// Arrange           
            _mockRepository.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(MockData.GetEmptyBook());

            /// Act
            var result = (NoContentResult)await bookController.GetAllBooksAsync();

            /// Assert
            result.StatusCode.Should().Be(204);
            _mockRepository.Verify(x => x.GetAllBooksAsync(), Times.Exactly(1));
        }

        [Fact]
        public async Task CreateBookAsync_ShouldCall_IBookRepository_And_Success()
        {
            /// Arrange           
            var newBook = MockData.NewBook();

            /// Act
            var result = (OkObjectResult)await bookController.CreateBookAsync(newBook);

            /// Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldCall_IBookRepository_And_Success()
        {
            //Arrange
            var id = 2;
            var bookList = MockData.GetBooksData();
            var selectedBook = bookList.FirstOrDefault(x=>x.Id == id);
            _mockRepository.Setup(x => x.GetByIdAsync(2))
                .ReturnsAsync(_mapper.Map<BookDto>(selectedBook));

            //Act
            var successResult = (OkObjectResult)await bookController.GetBookByIdAsync(2);
            var bookResult = successResult.Value as BookDto;

            //Assert
            Assert.NotNull(bookResult);
            Assert.Equal(bookList[1].Id, bookResult?.Id);
            Assert.Equal(bookList[1].Author, bookResult?.Author);
            Assert.Equal(bookList[1].Title, bookResult?.Title);
            Assert.Equal(bookList[1].Description, bookResult?.Description);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldCall_IBookRepository_And_NotFound()
        {
            //Arrange
            var id = 20;
            var bookList = MockData.GetBooksData();
            var selectedBook = bookList.FirstOrDefault(x => x.Id == id);
            _mockRepository.Setup(x => x.GetByIdAsync(2))
                .ReturnsAsync(_mapper.Map<BookDto>(selectedBook));

            //Act
            var result = (NotFoundResult)await bookController.GetBookByIdAsync(2);
            
            //Assert
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCall_IBookRepository_Return_Notfound()
        {
            // Arrange
            var id = 10;
            var book = new Book(id, "Manual", "This is a user manual", "Smith Jones");
            var updateBook = new Book(id, "User Manual", "This is a user manual", "Smith Jones");
            _mockRepository.Setup(x => x.UpdateAsync(id, book)).ReturnsAsync(updateBook);

            // Act
            var result = (NotFoundResult)await bookController.UpdateBookAsync(12, _mapper.Map<BookDto>(book));

            // Assert
            result.StatusCode.Should().Be(404);
            _mockRepository.Verify(x => x.UpdateAsync(id, book), Times.Exactly(0));
        }



        [Fact]
        public async Task DeleteAsync_ShouldCall_IBookRepository_And_Success()
        {
            // Arrange
            var id = 1;
            var bookList = MockData.GetBooksData();
            var selectedBook = bookList.FirstOrDefault(x => x.Id == id);
            _mockRepository.Setup(x => x.DeleteAsync(id)).ReturnsAsync(selectedBook);

            // Act
            var result = (OkObjectResult)await bookController.DeleteAsync(id);
            var deletedBook = result.Value as BookDto;
            // Assert
            deletedBook?.Id.Should().Be(selectedBook?.Id);
            deletedBook?.Author.Should().Be(selectedBook?.Author);
            deletedBook?.Title.Should().Be(selectedBook?.Title);
            deletedBook?.Description.Should().Be(selectedBook?.Description);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCall_IBookRepository_And_NotFound()
        {
            // Arrange
            var id = 10;
            var bookList = MockData.GetBooksData();
            var selectedBook = bookList.FirstOrDefault(x => x.Id == id);
            _mockRepository.Setup(x => x.DeleteAsync(id)).ReturnsAsync(selectedBook);

            // Act
            var result = (NotFoundResult)await bookController.DeleteAsync(id);

            // Assert
            result.StatusCode.Should().Be(404);
            _mockRepository.Verify(x => x.DeleteAsync(id), Times.Exactly(1));
        }
    }
}
