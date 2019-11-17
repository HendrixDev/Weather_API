using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPI
{
    class Program
    {
        const string key = "d99ef8df64527f2b2c10da65803ec27b";
        
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Please enter a US zip code for the current weather: ");
                var zip = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine(GetWeather(zip));
                Console.ReadKey();
                Console.Clear();
            }
        }
        
        static string GetWeather(string zipCode)
        {
            //needs using system.Net.Http;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.openweathermap.org/");
                client.DefaultRequestHeaders.Accept.Clear();

                //needs using system.net.http.headers
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("data/2.5/weather?zip=" + zipCode + ",us&appid=" + key).Result;

                RootObject data = JsonConvert.DeserializeObject<RootObject>(response.Content.ReadAsStringAsync().Result);

                //we must convert from KELVIN to fahrenheit
                string currentTemp = Math.Round((data.main.temp - 273.15) * 1.8 + 32).ToString() + "°";
                string highTemp = Math.Round((data.main.temp_max - 273.15) * 1.8 + 32).ToString() + "°";
                string lowTemp = Math.Round((data.main.temp_min - 273.15) * 1.8 + 32).ToString() + "°";
                string name = data.name;
                string humidity = data.main.humidity.ToString();
                string visibiliy = Math.Round(data.visibility / 1609.344).ToString();
                string final = $"Current weather for {name}:\n" +
                               $"Current: {currentTemp}\n" +
                               $"Max:{highTemp}\n" +
                               $"Min:{lowTemp}\n" +
                               $"Humidity:{humidity}%\n" +
                               $"Visibility:{visibiliy} miles";
                return final;

            }
        }
    }



}
