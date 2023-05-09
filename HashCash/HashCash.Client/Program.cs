using System;
using System.Runtime.InteropServices;

namespace HashCash.Client {
    public class Program {
        public static async Task Main(string[] args) {
            HttpClient client = new HttpClient();

            while (true) {
                string endpoint = GetUserInput("Enter last part of Endpoint http://localhost:5279/HashCash/ ");
                int difficulty = int.Parse(GetUserInput("Enter Difficulty "));

                Header header = new Header("http://localhost:5279/HashCash/" + endpoint);
                //header = HeaderBuilder.CreateHashWithLeadingZeros(header, 1);
                //Header header = new Header(endpoint);
                header = HeaderBuilder.CreateHashWithLeadingZeros(header, difficulty);
                Console.WriteLine("New Header: " + header);
                Console.WriteLine("Message: "+ HeaderBuilder.GenerateMessage(header));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, header.URL);
                request.Headers.Add("HashREST", HeaderBuilder.GenerateMessage(header));

                HttpResponseMessage response = await client.SendAsync(request);

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response Status Code: " + response.StatusCode);
                Console.WriteLine("Response Body: " + responseBody);
            }

        }

        public static string GetUserInput(string message) {
            Console.WriteLine(message);
            string entry = Console.ReadLine();
            Console.WriteLine();
            return entry;
        }
    }
}