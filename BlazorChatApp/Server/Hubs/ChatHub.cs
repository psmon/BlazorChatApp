using System.Collections.Generic;
using System.Threading.Tasks;

using Akka.Actor;

using BlazorChatApp.Shared;

using Microsoft.AspNetCore.SignalR;

namespace BlazorChatApp.Server.Hubs
{
    public class ChatHub : Hub
    {
        private ActorSystem actorSystem;

        private ActorSelection roomActor;

        public ChatHub(ActorSystem _actorSystem)
        {
            actorSystem = _actorSystem;
            roomActor = actorSystem.ActorSelection("user/room1");
        }

        
        // Client To Server

        public async Task JoInRoom(JoinRoom joinRoom)
        {
            roomActor.Tell(joinRoom);
        }

        public async Task SyncRoom(SyncRoom syncRoom)
        {
            roomActor.Tell(syncRoom);
        }

        public async Task LeaveRoom(LeaveRoom leaveRoom)
        {
            roomActor.Tell(leaveRoom);
        }

        public async Task UpdateUserPos(UpdateUserPos updateUserPos)
        {
            roomActor.Tell(updateUserPos);
        }

        // Server To Client

        public async Task OnJoinRoom(RoomInfo roomInfo, UserInfo user, UpdateUserPos updateUserPos)
        {
            await Clients.All.SendAsync("OnJoinRoom", user, roomInfo, updateUserPos);
        }

        public async Task OnSyncRoom(UserInfo user, List<UpdateUserPos> updateUserPos)
        {
            await Clients.All.SendAsync("OnSyncRoom", user, updateUserPos);
        }

        public async Task OnLeaveRoom(LeaveRoom leaveRoom)
        {
            await Clients.All.SendAsync("OnLeaveRoom", leaveRoom);
        }

        public async Task OnUpdateUserPos(UpdateUserPos updateUserPos)
        {
            await Clients.All.SendAsync("OnUpdateUserPos", updateUserPos);
        }

        
        public async Task SendMessage(string user, string message)       
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendRoomMessage(RoomCmd roomCmd)
        {
            roomActor.Tell(roomCmd);        
        }

    }
}
