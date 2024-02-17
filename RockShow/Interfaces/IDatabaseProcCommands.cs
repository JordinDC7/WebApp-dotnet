using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RockShow.Interfaces
{
    public interface IDatabaseProcCommands
    {
        int ExecuteNonQuery(string connectionString, string commandText, CommandType commandType,
            Action<SqlParameterCollection> inputParamMapper,
            Action<SqlParameterCollection> returnParameters = null,
            Action<System.Data.SqlClient.SqlCommand> cmdModifier = null);

        void ExecuteReader(string connectionString, string commandText,
            CommandType commandType, Action<SqlParameterCollection> inputParamMapper, 
            Action<IDataReader, short> singleRecordMapper,
            Action<SqlParameterCollection> returnParameters = null);
    }
}
