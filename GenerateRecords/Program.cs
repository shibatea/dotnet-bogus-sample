using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;

namespace GenerateRecords
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Seed 値が設定されていないので、実行するたびに生成されるデータが変わる
            var orders = GenerateRandomOrders(10);
            var text2 = JsonSerializer.Serialize(orders);
            Console.WriteLine(text2);

            // Seed 値を設定することに実行される都度生成されるデータが固定化される
            Randomizer.Seed = new Random(123456789);
            var billingDetails = GenerateRandomBillingDetails(10);
            var text1 = JsonSerializer.Serialize(billingDetails);
            Console.WriteLine(text1);

            // 永遠にデータを生成する
            var faker = new Faker<BillingDetails>()
                // .StrictMode(true)
                .RuleFor(x => x.CustomerName, x => x.Person.FullName)
                .RuleFor(x => x.Email, x => x.Person.Email)
                .RuleFor(x => x.Phone, x => x.Person.Phone)
                .RuleFor(x => x.AddressLine, x => x.Address.StreetAddress())
                .RuleFor(x => x.City, x => x.Address.City())
                .RuleFor(x => x.PostCode, x => x.Address.ZipCode())
                .RuleFor(x => x.Country, x => x.Address.Country());

            foreach (var record in faker.GenerateForever())
            {
                var text3 = JsonSerializer.Serialize(record);
                Console.WriteLine(text3);
                await Task.Delay(1000);
            }
        }

        private static List<BillingDetails> GenerateRandomBillingDetails(int numberOfRecordsPerBatch)
        {
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

        private static List<Order> GenerateRandomOrders(int numberOfRecordsPerBatch)
        {
            var billingDetailsFaker = new Faker<BillingDetails>()
                .RuleFor(x => x.CustomerName, x => x.Person.FullName)
                .RuleFor(x => x.Email, x => x.Person.Email)
                .RuleFor(x => x.Phone, x => x.Person.Phone)
                .RuleFor(x => x.AddressLine, x => x.Address.StreetAddress())
                .RuleFor(x => x.City, x => x.Address.City())
                .RuleFor(x => x.PostCode, x => x.Address.ZipCode())
                .RuleFor(x => x.Country, x => x.Address.Country());

            var orderFaker = new Faker<Order>()
                .RuleFor(x => x.Id, Guid.NewGuid)
                .RuleFor(x => x.Currency, x => x.Finance.Currency().Code)
                .RuleFor(x => x.Price, x => x.Finance.Amount(5, 100))
                .RuleFor(x => x.BillingDetails, x => billingDetailsFaker);

            var orders = orderFaker.Generate(numberOfRecordsPerBatch);

            return orders;
        }
    }
}
