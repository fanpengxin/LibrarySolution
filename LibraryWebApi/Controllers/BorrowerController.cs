using AutoMapper;
using LibraryWebApi.Dtos;
using LibraryWebApi.Entities;
using LibraryWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowerController : ControllerBase
    {
        private readonly IBorrowerRepository _reporsitoy;
        private readonly IMapper _mapper;
        private readonly ILogger<BorrowerController> _logger;

        /// <summary>
        /// Borrower Constructor
        /// </summary>
        /// <param name="reporsitoy"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public BorrowerController(
            IBorrowerRepository reporsitoy,
            IMapper mapper,
            ILogger<BorrowerController> logger
        )
        {
            _reporsitoy = reporsitoy;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Borrower API GetAllBorrowersAsync
        /// </summary>
        /// <returns>List BorrowerDto objects if found, otherwise return no content.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBorrowersAsync()
        {
            try
            {
                var result = await _reporsitoy.GetAllBorrowersAsync();
                if (result.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(_mapper.Map<List<BorrowerDto>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Book API GetBorrowerByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BorrowerDto object if found, otherwise resturn not found.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetBorrowerByIdAsync([FromRoute] int id)
        {
            try
            {
                var result = await _reporsitoy.GetByIdAsync(id);
                if (result is null)
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
        /// Book API CreateBorrowerAsync
        /// </summary>
        /// <param name="borrowerDto"></param>
        /// <returns>New BorrowerDto object</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBorrowerAsync([FromBody] BorrowerDto borrowerDto)
        {
            try
            {
                var entity = _mapper.Map<Borrower>(borrowerDto);
                var result = await _reporsitoy.CreateAsync(entity);
                borrowerDto = _mapper.Map<BorrowerDto>(result);
                return Ok(borrowerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Book API UpdateBorrowerAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="borrowerDto"></param>
        /// <returns>Updated BorrowerDto object if found, otherwise return not found.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateBorrowerAsync([FromRoute] int id, [FromBody] BorrowerDto borrowerDto)
        {
            try
            {
                var entity = _mapper.Map<Borrower>(borrowerDto);
                var result = await _reporsitoy.UpdateAsync(id, entity);
                if (result is null)
                {
                    return NotFound();
                }
                borrowerDto = _mapper.Map<BorrowerDto>(result);
                return Ok(borrowerDto);
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
        /// <returns>Deleted BorrowerDto object if found, otherwise return not found.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var entity = await _reporsitoy.DeleteAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BorrowerDto>(entity));
        }
    }
}
