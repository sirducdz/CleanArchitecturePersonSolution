using PeopleManager.Application.DTOs;
using PeopleManager.Application.Queries;
using PeopleManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleManager.Application.Interfaces.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonDto>> GetAllPeopleAsync();
        Task<PersonDto?> GetPersonByIdAsync(Guid id);
        Task<PersonDto> AddPersonAsync(CreatePersonDto createDto);
        Task<bool> UpdatePersonAsync(Guid id, UpdatePersonDto updateDto);
        Task<bool> DeletePersonAsync(Guid id);
        Task<IEnumerable<PersonDto>> FilterPeopleAsync(PersonFilterCriteria criteria);
    }
}
