using AutoMapper;
using LibraryWebApi.Dtos;
using LibraryWebApi.Entities;
using LibraryWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowBooksController : ControllerBase
    {
        private readonly IBorrowBooksRepository _reporsitoy;
        private readonly IMapper _mapper;
        private readonly ILogger<BorrowBooksController> _logger;

        /// <summary>
        /// BorrowBooksController Constructor
        /// </summary>
        /// <param name="reporsitoy"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public BorrowBooksController(
            IBorrowBooksRepository reporsitoy,
            IMapper mapper,
            ILogger<BorrowBooksController> logger
        )
        {
            _reporsitoy = reporsitoy;
            _mapper = mapper;
            _logger = logger;
        }


        /// <summary>
        /// BorrowBooks API GetAllBorrowBooksAsync
        /// </summary>
        /// <returns>List BookDto objects if found, otherwise return no content</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBorrowBooksAsync()
        {
            try
            {
                var result = await _reporsitoy.GetAllBorrowBooksAsync();
                if (result.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(_mapper.Map<List<BorrowBooksDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Book API GetBorrowBooksByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BorrowBooksDto object if found, otherwise return not found.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetBorrowBooksByIdAsync([FromRoute] int id)
        {
            try
            {
                var result = await _reporsitoy.GetByIdAsync(id);
                if(result is null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<BorrowBooksDto>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// BorrowBooks API CreateBorrowBooksAsync
        /// </summary>
        /// <param name="borrowBooksDto"></param>
        /// <returns>New BorrowBooksDto object</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBorrowBooksAsync([FromBody] BorrowBooksDto borrowBooksDto)
        {
            try
            {
                var entity = _mapper.Map<BorrowBooks>(borrowBooksDto);
                var result = await _reporsitoy.CreateAsync(entity);
                borrowBooksDto = _mapper.Map<BorrowBooksDto>(result);
                return Ok(borrowBooksDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// BorrowBooks API UpdateBorrowBooksAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="borrowBooksDto"></param>
        /// <returns>Update BorrowBooksDto object if found, otherwise return not found.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateBorrowBooksAsync([FromRoute] int id, [FromBody] BorrowBooksDto borrowBooksDto)
        {
            try
            {
                var entity = _mapper.Map<BorrowBooks>(borrowBooksDto);
                var result = await _reporsitoy.UpdateAsync(id, entity);
                if(result is null)
                {
                    return NotFound();
                }
                borrowBooksDto = _mapper.Map<BorrowBooksDto>(result);
                return Ok(borrowBooksDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// BorrowBooks API DeleteAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted BorrowBooksDto object if found, otherwise return not found.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var entity = await _reporsitoy.DeleteAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookDto>(entity));
        }
    }
}
