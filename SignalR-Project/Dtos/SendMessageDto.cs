namespace SignalR_Project.Dtos
{
    public sealed record SendMessageDto(Guid UserId, Guid ToUserId, string Message);
}
