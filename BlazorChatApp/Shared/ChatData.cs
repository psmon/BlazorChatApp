﻿namespace BlazorChatApp.Shared
{
    public class ChatData
    {
    }

    public class UserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
 
        public string Color { get; set; }
    }

    public class RoomInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateUserPos :UserInfo
    { 
        public double PosX{get; set; }
        public double PosY{get; set; }

        public string ConnectionId {get;set; }
        
    }


    public class ChatMessage
    { 
        public UserInfo From { get; set; }
        public string Message { get; set; }
    }

    public class JoinRoom
    {
        public RoomInfo RoomInfo { get; set; }
        public UserInfo UserInfo { get; set; }

        public string ConnectionId {get;set; }

    }

    public class SyncRoom
    {
        public RoomInfo RoomInfo { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class LeaveRoom
    {
        public RoomInfo RoomInfo { get; set; }
        public UserInfo UserInfo { get; set; }
    }


    public class BaseCmd
    {
        public string Command {get;set; }
    }


    public class RoomCmd : BaseCmd
    {        
        public UserInfo UserInfo{ get; set; }
        public object Data{get;set; }
    }

}
