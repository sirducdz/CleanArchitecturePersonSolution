using PeopleManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleManager.Infrastructure.Data
{
    public static class DummyData
    {
        public static List<Person> GetPeople()
        {
            return new List<Person>
            {
                new Person
                {
                    // Id will be auto-assigned by Repository if 0
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    Gender = Gender.Male,
                    BirthPlace = "New York"
                },
                new Person
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1992, 8, 20),
                    Gender = Gender.Female,
                    BirthPlace = "London"
                },
                 new Person
                {
                    FirstName = "Peter",
                    LastName = "Jones",
                    DateOfBirth = new DateTime(1985, 1, 10),
                    Gender = Gender.Male,
                    BirthPlace = "Paris"
                },
                 new Person
                {
                    FirstName = "Alice",
                    LastName = "Williams",
                    DateOfBirth = new DateTime(1995, 11, 30),
                    Gender = Gender.Female,
                    BirthPlace = "New York"
                },
                 new Person
                {
                    FirstName = "Robert",
                    LastName = "Brown", // For testing case sensitive name search
                    DateOfBirth = new DateTime(1988, 3, 25),
                    Gender = Gender.Male,
                    BirthPlace = "London"
                }
            };
        }
    }
}
