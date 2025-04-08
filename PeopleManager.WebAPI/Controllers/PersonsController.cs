using Microsoft.AspNetCore.Mvc;
using PeopleManager.Application.DTOs;
using PeopleManager.Application.Interfaces.Services;
using PeopleManager.Application.Queries;

namespace PeopleManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPeople()
        {
            // Service giờ trả về IEnumerable<PersonDto>
            var peopleDtos = await _personService.GetAllPeopleAsync();
            return Ok(peopleDtos); // Trả về danh sách DTO
        }

        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<PersonDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FilterPeople([FromQuery] PersonFilterCriteria criteria)
        {
            // Service giờ trả về IEnumerable<PersonDto>
            var filteredPeopleDtos = await _personService.FilterPeopleAsync(criteria);
            return Ok(filteredPeopleDtos); // Trả về danh sách DTO
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPersonById(Guid id)
        {
            // Service giờ trả về PersonDto?
            var personDto = await _personService.GetPersonByIdAsync(id);
            if (personDto == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }
            return Ok(personDto); // Trả về DTO
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreatePersonDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // *** SỬA: Nhận CreatePersonDto từ body ***
        public async Task<IActionResult> AddPerson([FromBody] CreatePersonDto createDto)
        {
            // *** SỬA: Kiểm tra DTO đầu vào ***
            if (createDto == null || !ModelState.IsValid)
            {
                // ModelState.IsValid sẽ kiểm tra các validation annotation trên CreatePersonDto
                return BadRequest(ModelState);
            }

            try
            {
                // *** SỬA: Gọi service với CreatePersonDto ***
                // Service giờ nhận CreatePersonDto và trả về PersonDto
                var createdPersonDto = await _personService.AddPersonAsync(createDto);

                // *** SỬA: Trả về PersonDto trong CreatedAtAction ***
                // createdPersonDto giờ chứa Id đã được tạo
                return CreatedAtAction(nameof(GetPersonById),
                                       new { id = createdPersonDto.Id },
                                       createdPersonDto); // Trả về DTO đã tạo
            }
            catch (ArgumentException ex) // Có thể bắt lỗi cụ thể hơn từ service nếu cần
            {
                return BadRequest(ex.Message);
            }
            // Bỏ bớt InvalidOperationException nếu logic gán Id đã chuyển vào Repository/Service
            // catch (InvalidOperationException ex) { ... }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] UpdatePersonDto updateDto)
        {
            if (updateDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _personService.UpdatePersonAsync(id, updateDto);

            if (!success)
            {
                return NotFound($"Person with ID {id} not found or update failed."); // Có thể gộp lỗi
            }

            return NoContent(); // Thành công
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePerson(Guid id) // Action này không thay đổi nhiều
        {
            var success = await _personService.DeletePersonAsync(id);

            if (!success)
            {
                return NotFound($"Person with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
