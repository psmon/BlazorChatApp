using Akka.Actor;
using Akka.Event;

using BlazorChatApp.Shared;

using Microsoft.AspNetCore.SignalR.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorChatApp.Server.Hubs
{
    public class RoomActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();

        public Dictionary<string,UpdateUserPos> users = new Dictionary<string,UpdateUserPos>();

        private string roomName;

        private int userAutoNo = 0;

        public HubConnection hubConnection { get; set; }

        Random random= new Random();

        public RoomActor(string _roomName)
        {
            string baseUrl = "http://localhost:5000";
            var _hubUrl = baseUrl.TrimEnd('/') + "/chathub";
            hubConnection = new HubConnectionBuilder().WithUrl(_hubUrl).Build();

            hubConnection.StartAsync().Wait();

            roomName = _roomName;

            log.Info($"Create Room{roomName}");

            Receive<RoomCmd>(cmd => {
                log.Info("Received String message: {0}", cmd);
                //Sender.Tell(message);                
            });

            Receive<JoinRoom>(async cmd => {
                userAutoNo++;
                string jsonString = JsonSerializer.Serialize(cmd);
                log.Info("Received JoinRoom message: {0}", jsonString);                
                string RandomColor =  string.Format("#{0:X6}", random.Next(0xFFFFFF));
                
                UserInfo userInfo = new UserInfo()
                { 
                    Id=cmd.UserInfo.Id,
                    Name=$"User-{userAutoNo}",
                    Color=RandomColor
                };

                UpdateUserPos updateUserPos= new UpdateUserPos()
                { 
                    Id=cmd.UserInfo.Id,
                    Name=$"User-{userAutoNo}",
                    PosX=random.NextDouble()*500,PosY=random.NextDouble()*500
                };

                users[cmd.UserInfo.Id] = updateUserPos;

                await OnJoinRoom(cmd.RoomInfo, userInfo, updateUserPos);
            });

            Receive<SyncRoom>(async cmd => {           
                userAutoNo++;
                string jsonString = JsonSerializer.Serialize(cmd);
                log.Info("Received SyncRoom message: {0}", jsonString);

                List<UpdateUserPos> updateUserPosList = users.Values.ToList();                
                //await hubConnection.SendAsync("OnSyncRoom", cmd.UserInfo, updateUserPosList);
                string RandomColor =  string.Format("#{0:X6}", random.Next(0xFFFFFF));

                UserInfo userInfo = new UserInfo()
                { 
                    Id=cmd.UserInfo.Id,
                    Name=$"User-{userAutoNo}",
                    Color=RandomColor
                };

                await OnSyncRoom(userInfo, users.Values.ToList());

            });

            Receive<UpdateUserPos>(async cmd => { 
                string jsonString = JsonSerializer.Serialize(cmd);
                log.Info("Received SyncRoom message: {0}", jsonString);
                if(users.ContainsKey(cmd.Id))
                {
                    users[cmd.Id].PosX+=cmd.PosX;
                    users[cmd.Id].PosY+=cmd.PosY;
                }

                UpdateUserPos updateUserPos = new UpdateUserPos()
                {
                    Id = cmd.Id,
                    Name = cmd.Name,
                    PosX = cmd.PosX,
                    PosY = cmd.PosY
                };

                await OnUpdateUserPos(updateUserPos);

            });


            Receive<LeaveRoom>(async cmd => {                
                string jsonString = JsonSerializer.Serialize(cmd);
                log.Info("Received LeaveRoom message: {0}", jsonString);

                if(users.ContainsKey(cmd.UserInfo.Id))
                {
                    users.Remove(cmd.UserInfo.Id);
                    await OnLeaveRoom(cmd);
                }
            });

        }

        public async Task OnJoinRoom(RoomInfo roomInfo, UserInfo user, UpdateUserPos updateUserPos)
        {
            await hubConnection.SendAsync("OnJoinRoom", user, roomInfo, updateUserPos);
        }

        public async Task OnSyncRoom(UserInfo user, List<UpdateUserPos> updateUserPos )
        {
            await hubConnection.SendAsync("OnSyncRoom", user, updateUserPos);
        }

        public async Task OnLeaveRoom(LeaveRoom leaveRoom)
        {
            await hubConnection.SendAsync("OnLeaveRoom", leaveRoom);
        }

        public async Task OnUpdateUserPos(UpdateUserPos updatePos)
        {
            await hubConnection.SendAsync("OnUpdateUserPos", updatePos);
        }

    }
}
