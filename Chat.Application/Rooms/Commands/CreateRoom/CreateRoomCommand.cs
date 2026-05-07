using Chat.Contracts.Rooms;
using Chat.Domain.Enums;
using MediatR;

namespace Chat.Application.Rooms.Commands.CreateRoom
{
    public sealed record CreateRoomCommand(string? RoomName, string RoomType) : IRequest<RoomDto>;
}
