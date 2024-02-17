using RockShow.Domain.LookUps;
using System.Data;

namespace RockShow.Interfaces
{
    public interface ILookUpService
    {
        List<LookUp> GetLookUp(string tableName);
        List<LookUp3Col> GetLookUp3Col(string tableName);
        Dictionary<string, List<LookUp>> GetMany(string[] tableNames);
        LookUp MapSingleLookUp(IDataReader reader, ref int startingIndex);
        LookUp3Col MapLookUp3Col(IDataReader reader, ref int startingIndex);
    }
}
