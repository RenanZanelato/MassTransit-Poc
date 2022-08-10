using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Playground.Messages;

namespace Playground.Consumer
{
    public sealed class SampleItensCreatedConsumer : IConsumer<SampleItensCreated>
    {
        public async Task Consume(ConsumeContext<SampleItensCreated> context)
        {
            var st = Stopwatch.StartNew();
            var message = context.Message;
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var exampleThatWorks = JsonConvert.DeserializeObject<SampleItem>(JsonConvert.SerializeObject(message.Item), settings); 
            st.Stop();

            Console.WriteLine($"Message #{context.Message.Id} consumed");
            await Task.CompletedTask;
        }
    }
}
