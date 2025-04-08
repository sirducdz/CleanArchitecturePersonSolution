using PeopleManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleManager.Application.DTOs
{
    public class UpdatePersonDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime? DateOfBirth { get; set; } // Sử dụng nullable (?) để [Required] hoạt động tốt với kiểu giá trị

        [Required(ErrorMessage = "Gender is required.")]
        public Gender? Gender { get; set; } // Sử dụng nullable (?) để [Required] hoạt động tốt với enum

        [Required(ErrorMessage = "Birth place is required.")]
        [StringLength(100, ErrorMessage = "Birth place cannot exceed 100 characters.")]
        public string BirthPlace { get; set; } = string.Empty;

    }
}
