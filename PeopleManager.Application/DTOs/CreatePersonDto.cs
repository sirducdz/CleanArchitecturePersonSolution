using PeopleManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleManager.Application.DTOs
{
    public class CreatePersonDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime? DateOfBirth { get; set; } // Dùng nullable để validation Required hoạt động tốt hơn

        [Required]
        public Gender? Gender { get; set; } // Dùng nullable

        [Required]
        [StringLength(100)]
        public string BirthPlace { get; set; } = string.Empty;
    }
}
