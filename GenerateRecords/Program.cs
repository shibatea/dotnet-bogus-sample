using System;
using System.Collections.Generic;
using System.Text.Json;
using Bogus;

namespace GenerateRecords
{
    class Program
    {
        static void Main(string[] args)
        {
            var billingDetails = GenerateRandomBillingDetails(10);

            var text = JsonSerializer.Serialize(billingDetails);

            Console.WriteLine(text);
        }

        private static List<BillingDetails> GenerateRandomBillingDetails(int numberOfRecordsPerBatch)
        {
            // Seed 値を設定することに実行される都度生成されるデータが固定化される
            Randomizer.Seed = new Random(123456789);

            var faker = new Faker<BillingDetails>()
                // .StrictMode(true)
                .RuleFor(x => x.CustomerName, x => x.Person.FullName)
                .RuleFor(x => x.Email, x => x.Person.Email)
                .RuleFor(x => x.Phone, x => x.Person.Phone)
                .RuleFor(x => x.AddressLine, x => x.Address.StreetAddress())
                .RuleFor(x => x.City, x => x.Address.City())
                .RuleFor(x => x.PostCode, x => x.Address.ZipCode())
                .RuleFor(x => x.Country, x => x.Address.Country());

            var billingDetails = faker.Generate(numberOfRecordsPerBatch);

            return billingDetails;
        }
    }
}
