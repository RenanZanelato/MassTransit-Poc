using System;
using System.Diagnostics;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Playground.Consumer;
using Playground.Messages;

namespace Playground
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IConsumer<SampleItensCreated>, SampleItensCreatedConsumer>();

            services
                .AddMassTransit(busConfigure =>
                {
                    busConfigure.UsingInMemory((context, serviceBusConfigure) =>
                    {
                        serviceBusConfigure.ConfigureEndpoints(context);

                        busConfigure.AddConsumer<IConsumer<SampleItensCreated>>();
                        serviceBusConfigure.ReceiveEndpoint(new TemporaryEndpointDefinition(), endpointConfig =>
                        {
                            endpointConfig.ConfigureConsumer<IConsumer<SampleItensCreated>>(context);
                        });
                    });
                })
                .AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var publisher = context.RequestServices.GetRequiredService<IPublishEndpoint>();

                    var example = new SampleItem
                    {
                        Id = Guid.NewGuid(),
                        Name = "example1",
                        Example = "example2"
                    };

                    var @event = new
                    {
                        Id = Guid.NewGuid(),
                        Item = example
                    };

                    var st = Stopwatch.StartNew();

                    await publisher.Publish<SampleItensCreated>(@event);
                    st.Stop();

                    Console.WriteLine($"Publishing event #{@event.Id} {st.ElapsedMilliseconds}ms");

                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
