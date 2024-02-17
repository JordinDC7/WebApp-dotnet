using System.Data;

namespace RockShow.Interfaces
{
    public interface IDbConnectionCreator
    {
        IDbConnection CreateConnection();
    }
}
