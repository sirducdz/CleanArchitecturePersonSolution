using PeopleManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleManager.Application.Queries
{
    public class PersonFilterCriteria
    {
        public string? Name { get; set; }
        public Gender? Gender { get; set; }
        public string? BirthPlace { get; set; }
    }
}
