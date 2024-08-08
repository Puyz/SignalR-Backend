namespace SignalR_Project.Dtos
{
    public sealed record RegisterDto
        (
            string Name,
            IFormFile Avatar
        );
}
