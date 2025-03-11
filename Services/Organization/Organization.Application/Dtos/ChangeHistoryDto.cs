namespace Organization.Application.Dtos
{
    public record ChangeHistoryDto(string ChangeLog, UserDto User, DateTime ChangedAt);
}
