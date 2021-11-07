using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Akka.Actor;
using Akka.Event;

namespace BlazorChatApp.Server.Hubs
{
    public class UserActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();

        double xPos;

        double yPos;

        IActorRef roomActor;

        public UserActor(IActorRef _roomActor)
        {
            this.roomActor = _roomActor;

            Receive<string>(message => {
                log.Info("Received String message: {0}", message);
                Sender.Tell(message);
            });        
        }
    }
}
