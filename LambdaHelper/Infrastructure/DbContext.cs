using System.Collections.Generic;

namespace LambdaHelper.Infrastructure
{
    public class DbContext
    {
        public DbContext()
        {
            People = new List<Person>();
            var person = new Person();
            person.FirstName = "John";
            person.LastName = "Doe";
            person.Age = 65;
            person.Address.Number = "1234";
            person.Address.Street = "Main Street";
            person.Address.City = "Philadelphia";
            person.Address.State = "PA";
            person.Address.Zipcode = "19101";
            People.Add(person);

            person = new Person();
            person.FirstName = "John1";
            person.LastName = "Doe1";
            person.Age = 17;
            person.Address.Number = "1234";
            person.Address.Street = "Main Street";
            person.Address.City = "Philadelphia";
            person.Address.State = "PA";
            person.Address.Zipcode = "19101";
            People.Add(person);

            person = new Person();
            person.FirstName = "Bob";
            person.LastName = "Smith";
            person.Age = 39;
            person.Address.Number = "123";
            person.Address.Street = "Lancaster Avenue";
            person.Address.City = "Paoli";
            person.Address.State = "PA";
            person.Address.Zipcode = "19301";
            People.Add(person);
        }

        public List<Person> People { get; set; }
    }
}
