using AutoMapper;
using LibraryWebApi.Dtos;
using LibraryWebApi.Entities;
using LibraryWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _reporsitoy;
        private readonly IMapper _mapper;
        private readonly ILogger<BookController> _logger;

        /// <summary>
        /// Book Constructor
        /// </summary>
        /// <param name="reporsitoy"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public BookController(
            IBookRepository reporsitoy,
            IMapper mapper,
            ILogger<BookController> logger
        )
        {
            _reporsitoy = reporsitoy;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Book API GetAllBooksAsync
        /// </summary>
        /// <returns>List BookDto objects if found, otherwise return no content.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            try
            {
                var result = await _reporsitoy.GetAllBooksAsync();
                if (result.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(_mapper.Map<List<BookDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }          
        }
        /// <summary>
        /// Book API GetBookByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BookDto object if found, otherwise return not found.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetBookByIdAsync([FromRoute] int id)
        {
            try
            {
                var result = await _reporsitoy.GetByIdAsync(id);
                if(result is null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Book API CreateBookAsync
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns>New BookDto</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBookAsync([FromBody] BookDto bookDto)
        {
            try
            {
                var entity = _mapper.Map<Book>(bookDto);                
                var result = await _reporsitoy.CreateAsync(entity);
                bookDto = _mapper.Map<BookDto>(result);
                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Book API UpdateBookAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDto"></param>
        /// <returns>Updated BookDto if found, otherwise return not found.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateBookAsync([FromRoute] int id, [FromBody] BookDto bookDto)
        {
            try
            {
                var entity = _mapper.Map<Book>(bookDto);
                var result = await _reporsitoy.UpdateAsync(id, entity);
                if (result is null)
                {
                    return NotFound();
                }
                bookDto = _mapper.Map<BookDto>(result);
                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Book API DeleteAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted BookDto if found, otherwise return not found.</returns>
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
