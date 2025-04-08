using PeopleManager.Application.DTOs;
using PeopleManager.Application.Interfaces.Persistence;
using PeopleManager.Application.Interfaces.Services;
using PeopleManager.Application.Queries;
using PeopleManager.Domain.Entities;

namespace PeopleManager.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<Person, Guid> _personRepository;

        public PersonService(IRepository<Person, Guid> personRepository)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        }

        public async Task<PersonDto> AddPersonAsync(CreatePersonDto createDto)
        {
            // 1. Input Validation (đã được thực hiện một phần bởi ModelState trong Controller)
            if (createDto == null) throw new ArgumentNullException(nameof(createDto));
            // Có thể thêm các validation nghiệp vụ khác ở đây nếu cần

            // 2. Mapping: Chuyển đổi CreatePersonDto -> Person (Domain Entity)
            var personEntity = MapFromCreateDto(createDto);

            // 3. Tương tác với Repository (sử dụng Domain Entity)
            // Repository sẽ xử lý việc tạo Id
            var addedPerson = await _personRepository.AddAsync(personEntity);

            // 4. Mapping: Chuyển đổi Person (Domain Entity) -> PersonDto (để trả về)
            var personDto = MapToPersonDto(addedPerson);

            return personDto;
        }

        public async Task<IEnumerable<PersonDto>> GetAllPeopleAsync()
        {
            // 1. Lấy tất cả entities từ Repository
            var peopleEntities = await _personRepository.GetAllAsync();

            // 2. Mapping: Chuyển đổi List<Person> -> List<PersonDto>
            var peopleDtos = peopleEntities.Select(person => MapToPersonDto(person)).ToList();

            return peopleDtos;
        }

        public async Task<PersonDto?> GetPersonByIdAsync(Guid id)
        {
            // 1. Lấy entity từ Repository
            var personEntity = await _personRepository.GetByIdAsync(id);

            // 2. Kiểm tra nếu không tìm thấy
            if (personEntity == null)
            {
                return null;
            }

            // 3. Mapping: Chuyển đổi Person -> PersonDto
            var personDto = MapToPersonDto(personEntity);

            return personDto;
        }

        public async Task<IEnumerable<PersonDto>> FilterPeopleAsync(PersonFilterCriteria criteria)
        {
            var query = await _personRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                query = query.Where(p =>
                   p.FirstName.Contains(criteria.Name, StringComparison.Ordinal) ||
                   p.LastName.Contains(criteria.Name, StringComparison.Ordinal)
                );
            }
            if (criteria.Gender.HasValue)
            {
                query = query.Where(p => p.Gender == criteria.Gender.Value);
            }
            if (!string.IsNullOrWhiteSpace(criteria.BirthPlace))
            {
                query = query.Where(p => p.BirthPlace.Equals(criteria.BirthPlace, StringComparison.Ordinal));
            }

            var filteredEntities = query.ToList(); // Thực thi lọc

            // Mapping: Chuyển đổi List<Person> (đã lọc) -> List<PersonDto>
            var filteredPeopleDtos = filteredEntities.Select(MapToPersonDto).ToList();

            return filteredPeopleDtos;
        }

        public async Task<bool> UpdatePersonAsync(Guid id, UpdatePersonDto updateDto)
        {
            if (updateDto == null) throw new ArgumentNullException(nameof(updateDto));
            // Có thể thêm validation cho id nếu cần (vd: id != Guid.Empty)

            var existingPerson = await _personRepository.GetByIdAsync(id);

            if (existingPerson == null)
            {
                return false;
            }

            MapFromUpdateDto(updateDto, existingPerson);
            return await _personRepository.UpdateAsync(existingPerson);
        }

        public async Task<bool> DeletePersonAsync(Guid id)
        {
            // Không cần DTO ở đây, chỉ cần Id
            // Repository.DeleteAsync đã trả về bool
            return await _personRepository.DeleteAsync(id);
        }

        private PersonDto MapToPersonDto(Person person)
        {
            if (person == null) return null;

            return new PersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender.ToString(), // Chuyển Enum thành String cho DTO
                BirthPlace = person.BirthPlace
                // Thêm các thuộc tính tính toán nếu PersonDto có (vd: Age, FullName)
            };
        }

        private Person MapFromCreateDto(CreatePersonDto dto)
        {
            // Validation cho dto nên xảy ra trước khi gọi hàm này
            return new Person
            {
                // Id KHÔNG được gán ở đây, Repository sẽ tạo
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                // Giả định validation đã đảm bảo Value không null
                DateOfBirth = dto.DateOfBirth.Value,
                Gender = dto.Gender.Value,
                BirthPlace = dto.BirthPlace
            };
        }

        private void MapFromUpdateDto(UpdatePersonDto dto, Person existingPerson)
        {
            // Validation cho dto và existingPerson nên xảy ra trước khi gọi hàm này
            existingPerson.FirstName = dto.FirstName;
            existingPerson.LastName = dto.LastName;
            existingPerson.DateOfBirth = dto.DateOfBirth.Value; // Giả định validation
            existingPerson.Gender = dto.Gender.Value;         // Giả định validation
            existingPerson.BirthPlace = dto.BirthPlace;
            // Không cập nhật Id
            // Có thể thêm logic cập nhật ngày LastModifiedDate ở đây nếu có
        }
    }
}
