using LinqToDB.DataProvider;
using RockShow.Domain.LookUps;
using RockShow.Interfaces;
using System.Data;
using System.Text.RegularExpressions;

namespace RockShow.Services
{
    public class LookUpService : ILookUpService
    {
        private IDatabaseProcCommands _data = null;
        string _connectionString = null;
        public LookUpService(IDatabaseProcCommands data, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _data = data;
        }
        public List<LookUp> GetLookUp(string tableName)
        {
            List<LookUp> list = null;

            string procName = $"[dbo].[{tableName}_SelectAll]";

            _data.ExecuteReader(_connectionString, procName, CommandType.StoredProcedure,
               inputParamMapper: null, 
               singleRecordMapper: (IDataReader reader, short set) =>
               {
                int startingIndex = 0;

                LookUp aLookUp = MapSingleLookUp(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<LookUp>();
                }
                list.Add(aLookUp);
            });

            return list;
        }

        public Dictionary<string, List<LookUp>> GetMany(string[] tableNames)
        {
            Dictionary<string, List<LookUp>> result = null;

            foreach (string table in tableNames)
            {
                List<LookUp> currentList = GetLookUp(table);
                string nameToUse = ToCamelCase(table);

                if (result == null)
                {
                    result = new Dictionary<string, List<LookUp>>();
                }
                result.Add(nameToUse, currentList);
            }

            return result;
        }

        public List<LookUp3Col> GetLookUp3Col(string tableName)
        {
            List<LookUp3Col> list = null;

            string procName = $"[dbo].[{tableName}_SelectAll]";

            _data.ExecuteReader(_connectionString, procName, CommandType.StoredProcedure,
                 inputParamMapper: null,
                 singleRecordMapper: (IDataReader reader, short set) =>
                 {
                int startingIndex = 0;

                LookUp3Col aLookUp = MapLookUp3Col(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<LookUp3Col>();
                }
                list.Add(aLookUp);
            });

            return list;
        }


        #region Mappers and Private Methods
        public LookUp MapSingleLookUp(IDataReader reader, ref int startingIndex)
        {
            LookUp lookUp = new LookUp();

            lookUp.Id = reader.GetInt32(startingIndex++);
            lookUp.Name = reader.GetString(startingIndex++);

            return lookUp;
        }

        public LookUp3Col MapLookUp3Col(IDataReader reader, ref int startingIndex)
        {
            LookUp3Col lookUp = new LookUp3Col();

            lookUp.Id = reader.GetInt32(startingIndex++);
            lookUp.Name = reader.GetString(startingIndex++);
            lookUp.Code = reader.GetString(startingIndex++);

            return lookUp;
        }

        private static string ToCamelCase(string str)
        {
            string name = null;
            if (str.Length > 0)
            {
                str = Regex.Replace(str, "([A-Z])([A-Z]+)($|[A-Z])", m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
                name = char.ToLower(str[0]) + str.Substring(1);
            }
            return name;
        }
        #endregion
    }
}
