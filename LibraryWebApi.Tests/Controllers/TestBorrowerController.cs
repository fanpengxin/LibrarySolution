namespace LibraryWebApi.Tests.Controllers
{
    public class TestBorrowerController
    {
        private IMapper _mapper;
        private Mock<IBorrowerRepository> _repository;        
        private Mock<ILogger<BorrowerController>> _logger;
        private BorrowerController borrowerController;

        public TestBorrowerController()
        {
            _repository = new Mock<IBorrowerRepository>();
            _logger = new Mock<ILogger<BorrowerController>>();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfiles());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            borrowerController = new BorrowerController(_repository.Object, _mapper, _logger.Object);
        }

        [Fact]
        public async Task GetAllBorrowersAsync_ShouldReturn200Status()
        {
            /// Arrange            
            _repository.Setup(x => x.GetAllBorrowersAsync()).ReturnsAsync(MockData.GetBorrowersData());

            /// Act
            var result = (OkObjectResult)await borrowerController.GetAllBorrowersAsync();

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturn204NoContentStatus()
        {
            /// Arrange           
            _repository.Setup(x => x.GetAllBorrowersAsync()).ReturnsAsync(MockData.GetEmptyBorrower());

            /// Act
            var result = (NoContentResult)await borrowerController.GetAllBorrowersAsync();

            /// Assert
            result.StatusCode.Should().Be(204);
            _repository.Verify(x => x.GetAllBorrowersAsync(), Times.Exactly(1));
        }

        [Fact]
        public async Task CreateBookAsync_ShouldCall_IBorrowerRepository_And_Success()
        {
            /// Arrange           
            var newBorrower = MockData.NewBorrower();
            var newBorrowerDto = _mapper.Map<BorrowerDto>(newBorrower);

            /// Act
            var result = (OkObjectResult)await borrowerController.CreateBorrowerAsync(newBorrowerDto);

            /// Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetBorrowerByIdAsync_ShouldCall_IBorrowerRepository_And_Success()
        {
            //Arrange
            var id = 2;
            var borrowerList = MockData.GetBorrowersData();
            var selectedBorrower = borrowerList.FirstOrDefault(x => x.Id == id);
            _repository.Setup(x => x.GetByIdAsync(2))
                .ReturnsAsync(_mapper.Map<BorrowerDto>(selectedBorrower));

            //Act
            var successResult = (OkObjectResult)await borrowerController.GetBorrowerByIdAsync(id);
            var borrowerResult = successResult.Value as BorrowerDto;

            //Assert
            Assert.NotNull(borrowerResult);
            Assert.Equal(selectedBorrower?.Id, borrowerResult?.Id);
            Assert.Equal(selectedBorrower?.FirstName, borrowerResult?.FirstName);
            Assert.Equal(selectedBorrower?.LastName, borrowerResult?.LastName);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldCall_IBorrowerRepository_And_NotFound()
        {
            //Arrange
            var id = 20;
            var borrowerList = MockData.GetBorrowersData();
            var selectedBorrower = borrowerList.FirstOrDefault(x => x.Id == id);
            _repository.Setup(x => x.GetByIdAsync(2))
                .ReturnsAsync(_mapper.Map<BorrowerDto>(selectedBorrower));

            //Act
            var result = (NotFoundResult)await borrowerController.GetBorrowerByIdAsync(id);

            //Assert
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCall_IBookRepository_Return_NotFound()
        {
            // Arrange
            var id = 22;
            var borrowerList = MockData.GetBorrowersData();
            var selectedBorrower = borrowerList.FirstOrDefault(x => x.Id == id);

            var borrower = new Borrower(id, "John", "Smith");
            var updateBorrower = new Borrower(id, "Scott", "Smith");

            _repository.Setup(x => x.UpdateAsync(id, borrower)).ReturnsAsync(updateBorrower);

            var borrowerController = new BorrowerController(_repository.Object, _mapper, _logger.Object);
            // Act
            var dtoModel = _mapper.Map<BorrowerDto>(borrower);
            var result = (NotFoundResult)await borrowerController.UpdateBorrowerAsync(id, dtoModel);

            // Assert
            result.StatusCode.Should().Be(404);
            _repository.Verify(x => x.UpdateAsync(id, borrower), Times.Exactly(0));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCall_IBorrowerRepository_And_Success()
        {
            // Arrange
            var id = 1;
            var borrowerList = MockData.GetBorrowersData();
            var selectedBorrower = borrowerList.FirstOrDefault(x => x.Id == id);
            _repository.Setup(x => x.DeleteAsync(id)).ReturnsAsync(selectedBorrower);

            // Act
            var result = (OkObjectResult)await borrowerController.DeleteAsync(id);
            var deletedBorrower = result.Value as BorrowerDto;

            // Assert
            deletedBorrower?.Id.Should().Be(selectedBorrower?.Id);
            deletedBorrower?.FirstName.Should().Be(selectedBorrower?.FirstName);
            deletedBorrower?.LastName.Should().Be(selectedBorrower?.LastName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCall_IBorrowerRepository_Return_NoFound()
        {
            // Arrange
            var id = 10;
            var borrowerList = MockData.GetBorrowersData();
            var selectedBorrower = borrowerList.FirstOrDefault(x => x.Id == id);
            _repository.Setup(x => x.DeleteAsync(id)).ReturnsAsync(selectedBorrower);

            // Act
            var result = (NotFoundResult)await borrowerController.DeleteAsync(id);

            // Assert
            result.StatusCode.Should().Be(404);
        }
    }
}
