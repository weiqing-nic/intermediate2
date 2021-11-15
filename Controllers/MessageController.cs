using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using producerd.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;

namespace producerd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        [HttpGet]
        public void getbpi() 
        {
            int i2 = 0;
            while (true)
            {
                if (i2 == 0)
                {
                    i2 += 1;
                    var factory = new ConnectionFactory()
                    {
                   
                        HostName = "localhost",
                        Port = 31672
                    };
                    int i = 0;
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "Bpi",
                                   durable: false,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

                        var reponsestring = "https://api.coindesk.com/v1/bpi/currentprice.json";
                        var client = new HttpClient();
                        var responseresult = client.GetAsync(reponsestring);
                        responseresult.Wait();
                        var result = responseresult.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readAsync = result.Content.ReadAsStringAsync();
                            readAsync.Wait();
                            string s = readAsync.Result;
                            readAsync.Wait();
    //                        int index2 = s.IndexOf("bpi") + 5;
  //                          int nlen = s.Length - index2 - 1;
//                            string news = s.Substring(index2, nlen);
                            string message = "hello";
                            var body = Encoding.UTF8.GetBytes(s);
                            channel.BasicPublish(exchange: "",
                                         routingKey: "Bpi",
                                         basicProperties: null,
                                         body: body);
                        }


                    }
                }
                else
                {
                    var factory = new ConnectionFactory()
                    {
                        //HostName = "host.docker.internal"
                        //Port = "31859"
                        //                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
                        //              Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBITMQ_PORT"))
                        HostName = "localhost",
                        Port = 31672
                    };
                    int i = 0;
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {

                        var reponsestring = "https://api.coindesk.com/v1/bpi/currentprice.json";
                        var client = new HttpClient();
                        var responseresult = client.GetAsync(reponsestring);
                        responseresult.Wait();
                        var result = responseresult.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readAsync = result.Content.ReadAsStringAsync();
                            readAsync.Wait();
                            string s = readAsync.Result;
                            readAsync.Wait();
                            //                        int index2 = s.IndexOf("bpi") + 5;
                            //                          int nlen = s.Length - index2 - 1;
                            //                            string news = s.Substring(index2, nlen);
                            string message = "hello";
                            var body = Encoding.UTF8.GetBytes(s);
                            channel.BasicPublish(exchange: "",
                                         routingKey: "Bpi",
                                         basicProperties: null,
                                         body: body);
                        }


                    }
                }
                int timesleep = 15 *   60  * 1000;
                Thread.Sleep(timesleep); //10 seconds

            }
        }
        [HttpPost]
        public async Task<Message> Post( Message greeting)
        {

            var json = JsonConvert.SerializeObject(greeting);

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "https://httpbin.org/post";
            var client = new HttpClient();

            var responseresult = await client.PostAsync(url, data);

            if (responseresult.IsSuccessStatusCode)
            {
                Console.WriteLine("heele");
                var readAsync = responseresult.Content.ReadAsStringAsync();
                string s = readAsync.Result;
                readAsync.Wait();
                Console.WriteLine(readAsync);
                Console.WriteLine("hdsdfhdsfhsdfh");
                greeting.Greet = s;
                return greeting;
            }
            else
            {
                Console.WriteLine("fail");
            }


            return greeting;
        }

    }
}
