﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PubSub.Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Logging;
using Rebus.Routing.TypeBased;

namespace PubSub.Subscriber2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register(() => new Handler());

                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))                    
                    .Transport(t => t.UseSqlServer("Server=.;Database=SqlPubSub;trusted_connection=yes;", "Messages", "subscriber2"))
                    .Routing(r => r.TypeBased().MapAssemblyOf<StringMessage>("publisher"))
                    .Start();

                activator.Bus.Subscribe<StringMessage>().Wait();

                Console.WriteLine("This is Subscriber 2");
                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
                Console.WriteLine("Quitting...");
            }
        }
    }

    class Handler : IHandleMessages<StringMessage>
    {
        public async Task Handle(StringMessage message)
        {
            Console.WriteLine("Got string: {0}", message.Text);
        }
    }
}
