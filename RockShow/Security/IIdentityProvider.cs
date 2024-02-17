namespace RockShow.Security
{
    public interface IIdentityProvider<T>
    {
        T GetCurrentUserId();
    }
}
