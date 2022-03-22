namespace LambdaHelper.Infrastructure
{
    public class Person
    {
        public Person()
        {
            Address = new Address();
        }

        public int Id { get; set; }
        public byte Byte { get; set; }
        public short Short { get; set; }
        public decimal Decimal { get; set; }
        public double Double { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public int Age { get; set; }
    }
}
