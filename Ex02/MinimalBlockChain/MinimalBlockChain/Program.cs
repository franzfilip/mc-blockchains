using MinimalBlockChain.Utility;
using Newtonsoft.Json;
using System.Text.Json;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        Person person = new Person { Name = "Max", Age = 18, Address = "asdf" };

        var settings = new JsonSerializerSettings
        {
            ContractResolver = new AlphabeticalContractResolver(),
            Formatting = Formatting.Indented
        };

        var json = JsonConvert.SerializeObject(person, settings);
        var json2 = JsonConvert.SerializeObject(person);
        Console.WriteLine(json);
    }
}

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
}