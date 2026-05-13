using Chat.Application.Messages.Commands.SendMessage;
using Chat.Application.Messages.Queries.GetRoomMessages;
using Chat.Application.Reactions.Commands.AddReaction;
using Chat.Application.Reactions.Commands.RemoveReaction;
using Chat.Application.Rooms.Queries;
using Chat.Domain.Entities;
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

        public async Task SubscribeToRoom(Guid RoomId)
        {
            var canAccess = await _sender.Send(new CanAccessRoomQuery(RoomId));

            if (!canAccess)
            {
                throw new HubException("You are not a member of this room");
            }

            Console.WriteLine("Connection established");

            await Groups.AddToGroupAsync(Context.ConnectionId, RoomId.ToString());
        }

        public async Task UnsubscribeFromRoom(Guid RoomId)
        {
            Console.WriteLine("Connection terminated");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, RoomId.ToString());
        }

        public async Task SendRoomMessage(Guid RoomId, string Content)
        {
            var message = await _sender.Send(new SendMessageCommand(RoomId, Content));
            Console.WriteLine("User sended a message through signalR");
            await Clients.Group(RoomId.ToString()).SendAsync("ReceiveRoomMessage", message);
        }

        public async Task GetRoomMessages(Guid RoomId)
        {
            var messages = await _sender.Send(new GetRoomMessagesQuery(RoomId));
            Console.WriteLine("User accessed all previous messages");
            foreach (var msg in messages)
            {
                Console.WriteLine(msg);
            }
            await Clients.Group(RoomId.ToString()).SendAsync("ReceiveRoomMessage", messages);
        }

        public async Task AddReaction(Guid MessageId, string Emoji)
        {
            var reaction = await _sender.Send(new AddReactionCommand(MessageId, Emoji));
            Console.WriteLine($"Reaction added {Emoji}");
            await Clients.Group(reaction.RoomId.ToString()).SendAsync("ReactionAdded", reaction);
        }

        public async Task RemoveReaction(Guid MessageId, string Emoji)
        {
            var reaction = await _sender.Send(new RemoveReactionCommand(MessageId, Emoji));
            Console.WriteLine($"Reaction removed {Emoji}");
            await Clients.Group(reaction.RoomId.ToString()).SendAsync("ReactionRemoved", reaction);
        }
    }
}
