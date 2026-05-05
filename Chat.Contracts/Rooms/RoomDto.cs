namespace Chat.Contracts.Rooms
{
    public sealed record RoomDto(Guid Id, string? RoomName, string RoomType, DateTime CreatedAtUtc);
}
