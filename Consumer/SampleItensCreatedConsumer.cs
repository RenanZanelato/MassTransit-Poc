using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Playground.Messages;

namespace Playground.Consumer
{
    public sealed class SampleItensCreatedConsumer : IConsumer<SampleItensCreated>
    {
        public async Task Consume(ConsumeContext<SampleItensCreated> context)
        {
            var st = Stopwatch.StartNew();
            var message = context.Message;
            st.Stop();

            Console.WriteLine($"Message #{context.Message.Id} consumed");
            await Task.CompletedTask;
        }
    }
}
