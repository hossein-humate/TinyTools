using System.Collections.Generic;

namespace LambdaHelper.Infrastructure
{
    public class DbContext
    {
        public DbContext()
        {
            People = new List<Person>();
            var person = new Person
            {
                FirstName = "Hossein",
                LastName = "SB"
            };
            person.FullName = $"{person.FirstName} {person.LastName}";
            person.Age = 65;
            person.Address.Number = "1";
            person.Address.Street = "Main Street";
            person.Address.City = "Philadelphia";
            person.Address.State = "PA";
            person.Address.Zipcode = "19101";
            People.Add(person);

            person = new Person
            {
                FirstName = "John",
                LastName = "Doe"
            };
            person.FullName = $"{person.FirstName} {person.LastName}";
            person.Age = 17;
            person.Address.Number = "2";
            person.Address.Street = "Main Street";
            person.Address.City = "Philadelphia";
            person.Address.State = "PA";
            person.Address.Zipcode = "19101";
            People.Add(person);

            person = new Person
            {
                FirstName = "Bob",
                LastName = "Smith"
            };
            person.FullName = $"{person.FirstName} {person.LastName}";
            person.Age = 39;
            person.Address.Number = "3";
            person.Address.Street = "Lancaster Avenue";
            person.Address.City = "Paoli";
            person.Address.State = "PA";
            person.Address.Zipcode = "19301";
            People.Add(person);
        }

        public List<Person> People { get; set; }
    }
}
