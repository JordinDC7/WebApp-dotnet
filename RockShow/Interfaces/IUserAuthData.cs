namespace RockShow.Interfaces
{
    public interface IUserAuthData
      {
        int Id { get; }
        string Email { get; }
        string Role { get; }
        object TenantId { get; }

    }
}
