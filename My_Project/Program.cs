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
    .Join(cities, p => p.CityId, c => c.Id, (p, c) => new { p.FullName, c.Name, c.CountryId })
    .Join(countries, x => x.CountryId, co => co.Id, (x, co) => new
        {
            x.FullName,
            CityName = x.Name,
            CountryName = co.Name
        });

foreach (var item in s)
{
    Console.WriteLine($"{item.FullName} ---- {item.CityName} ----- {item.CountryName}");
}

Console.WriteLine("-----------------------------------------------------------5");
var k = cities
    .SelectMany(city => people
        .Where(p => p.FullName.ToLower() == "alice johnson" && p.CityId == city.Id)
        .Select(p => new 
        { 
            PersonName = p.FullName, 
            CityName = city.Name, 
            CountryName = countries.First(g => g.Id == city.CountryId).Name 
        }));

foreach (var item in k )
    Console.WriteLine($"{item.PersonName}, {item.CityName}, {item.CountryName}");

Console.WriteLine("-------------------------------------------------------------6");
var cityById = cities.ToDictionary(c => c.Id, c => c);

var r = people
    .GroupBy(p => p.CityId)
    .Select(g => new
    {
        CityId = g.Key,
        OldestPerson = g.OrderByDescending(p => p.Age).First()
    })
    .Join(cities, x => x.CityId, c => c.Id, (x, c) => new
        {
            CityName = c.Name,
            FullName = x.OldestPerson.FullName,
            Age = x.OldestPerson.Age
        })
    .OrderBy(x => x.CityName);

foreach (var item in r)
{
    Console.WriteLine($"{item.CityName}: {item.FullName} — {item.Age}");
}

Console.WriteLine("-------------------------------------------------------------7");

var e = countries
    .Select(country => new
    {
        Country = country.Name,
        BiggestCity = country.Cities.OrderByDescending(c => c.Population).First(),
    })
    .SelectMany(x => x.BiggestCity.People
        .Select(p => $"{p.FullName} — {x.BiggestCity.Name}, {x.Country}")
    )
    .OrderBy(x => x);

foreach (var line in e)
    Console.WriteLine(line);


Console.WriteLine("-------------------------------------------------------------8");

var u = cities
    .Where(city => city.Name.Length == 5)
    .SelectMany(city => city.People, (city, p) => $"{p.FullName} ---- {city.Name}")
    .OrderBy(x => x);

foreach (var line in u) 
    Console.WriteLine(line);
    
    
    
Console.WriteLine("-------------------------------------------------------------9");

var l = countries
    .Select(country => new
    {
        CountryName = country.Name,
        YoungestPerson = country.Cities
            .SelectMany(city => city.People)
            .OrderBy(p => p.Age)
            .FirstOrDefault()
    })
    .Where(x => x.YoungestPerson != null)
    .OrderBy(x => x.CountryName);

foreach (var item in l)
{
    Console.WriteLine($"{item.CountryName}: {item.YoungestPerson.FullName} — {item.YoungestPerson.Age}");
}


Console.WriteLine("-------------------------------------------------------------10");

var f = cities
    .Select(city => new
    {
        CityName = city.Name,
        CountryName = city.Country.Name,
        Count = city.People.Count(p => p.Age >= 20 && p.Age <= 30)
    })
    .OrderByDescending(x => x.Count)
    .First();

Console.WriteLine($"{f.CityName}, {f.CountryName} — {result.Count} person");