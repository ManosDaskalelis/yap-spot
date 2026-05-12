using Chat.Application.Messages.Commands.SendMessage;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs
{
    public sealed class ChatHub : Hub
    {
        private readonly ISender _sender;

        public ChatHub(ISender sender)
        {
            _sender = sender;
        }

        public async Task JoinRoom(Guid RoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, RoomId.ToString());
        }

        public async Task LeaveRoom(Guid RoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, RoomId.ToString());
        }

        public async Task SendRoomMessage(Guid RoomId, string Content)
        {
            var message = await _sender.Send(new SendMessageCommand(RoomId, Content));

            await Clients.Group(RoomId.ToString()).SendAsync("ReceiveRoomMessage", message);
        }
    }
}
