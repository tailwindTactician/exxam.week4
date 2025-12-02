using System.Globalization;
using My_Project;

var countries = new List<Country>
{
    new Country { Id = 1, Name = "USA" },
    new Country { Id = 2, Name = "France" },
    new Country { Id = 3, Name = "Japan" }
};

var cities = new List<City>
{
    new City { Id = 1, Name = "New York", Population = 8400000, CountryId = 1 },
    new City { Id = 2, Name = "Los Angeles", Population = 3900000, CountryId = 1 },
    new City { Id = 3, Name = "Paris", Population = 2200000, CountryId = 2 },
    new City { Id = 4, Name = "Lyon", Population = 500000, CountryId = 2 },
    new City { Id = 5, Name = "Tokyo", Population = 14000000, CountryId = 3 },
    new City { Id = 6, Name = "Nagasaki", Population = 2700000, CountryId = 3 }
};

var people = new List<Person>
{
    new Person { Id = 1, FullName = "Alice Johnson", Age = 30, CityId = 1 },
    new Person { Id = 2, FullName = "Bob Smith", Age = 25, CityId = 1 },
    new Person { Id = 3, FullName = "Charlie Brown", Age = 35, CityId = 2 },
    new Person { Id = 4, FullName = "David Wilson", Age = 40, CityId = 3 },
    new Person { Id = 5, FullName = "Eve Davis", Age = 22, CityId = 3 },
    new Person { Id = 6, FullName = "Frank Miller", Age = 28, CityId = 4 },
    new Person { Id = 7, FullName = "Grace Lee", Age = 33, CityId = 5 },
    new Person { Id = 8, FullName = "Hank Kim", Age = 20, CityId = 5 },
    new Person { Id = 9, FullName = "Ivy Chen", Age = 27, CityId = 6 },
    new Person { Id = 10, FullName = "Jack White", Age = 31, CityId = 6 }
};
foreach (var city in cities)
{
    city.Country = countries.First(c => c.Id == city.CountryId);
}

foreach (var country in countries)
{
    country.Cities = cities.Where(c => c.CountryId == country.Id).ToList();
}

foreach (var person in people)
{
    person.City = cities.First(c => c.Id == person.CityId);
    person.City.People.Add(person);
}

Console.WriteLine("------------------------------------------------------1");
var result = people
    .Where(p => p.City.Population > 3000000)
    .ToList();

foreach (var p in result)
{
    Console.WriteLine($"{p.FullName} — {p.City.Name} — {p.City.Population}");
}

Console.WriteLine("---------------------------------------------------------2");
foreach (var x in countries
             .SelectMany(c => c.Cities
                 .Where(city => city.Population > c.Cities.Average(p => p.Population))
                 .Select(city => new { Country = c.Name, City = city })))
{
    Console.WriteLine($"{x.Country}: {x.City.Name} — {x.City.Population}");
}

Console.WriteLine("-------------------------------------------------3");
var t = countries
    .Select(c => new { c.Name, MostPopulatedCity = c.Cities
        .OrderByDescending(city => city.Population).First() });

foreach (var item in t)
    Console.WriteLine($"{item.Name}: {item.MostPopulatedCity.Name} — {item.MostPopulatedCity.Population}");

Console.WriteLine("--------------------------------------------------------4");

var s = people
    .Select(p => new { FullName = p.FullName, CityName = cities 
        .First(c => c.Id == p.CityId).Name, CountryName = countries
        .First(a => a.Id == cities
            .First(c => c.Id == p.CityId).CountryId).Name });

foreach (var item in s)
    Console.WriteLine($"{item.FullName} ---- {item.CityName} ----- {item.CountryName}");

Console.WriteLine("-----------------------------------------------------------5");
var k = cities
    .SelectMany(city => people
        .Where(p => p.FullName.ToLower() == "alice johnson" && p.CityId == city.Id)
        .Select(p => new 
        { 
            PersonName = p.FullName, 
            CityName = city.Name, 
            CountryName = countries.First(co => co.Id == city.CountryId).Name 
        }));

foreach (var item in k )
    Console.WriteLine($"{item.PersonName}, {item.CityName}, {item.CountryName}");

Console.WriteLine("-------------------------------------------------------------6");
var a = cities
    .Select(city => new { CityName = city.Name, OldestPerson = people
            .Where(p => p.CityId == city.Id)
            .OrderByDescending(p => p.Age)
            .FirstOrDefault() })
    .Where(x => x.OldestPerson != null);

foreach (var item in a)
    Console.WriteLine($"{item.CityName}: {item.OldestPerson.FullName} — {item.OldestPerson.Age}");

Console.WriteLine("---------------------------------------------------------7");
