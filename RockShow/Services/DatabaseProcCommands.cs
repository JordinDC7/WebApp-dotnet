using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using RockShow.Interfaces;

namespace RockShow.Services
{
    public class DatabaseProcCommands : IDatabaseProcCommands
    {
        // Set the connection, command, and then execute the command with non query.  

        //ExecuteNonQuery is used to execute a command that will not return any data, for example Insert, Update, Delete.
        public int ExecuteNonQuery(string connectionString, string commandText, CommandType commandType,
       Action<SqlParameterCollection> inputParamMapper,
       Action<SqlParameterCollection> returnParameters = null,
       Action<System.Data.SqlClient.SqlCommand> cmdModifier = null)
        {
            int affectedRows = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            { 
                conn.Open();
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    // Invoke the action to add parameters
                    inputParamMapper?.Invoke(cmd.Parameters);
                    cmdModifier?.Invoke(cmd);

                    affectedRows = cmd.ExecuteNonQuery();

                    // Optionally, handle return parameters after command execution
                    returnParameters?.Invoke(cmd.Parameters);
 
                }
            }
            return affectedRows;
        }
        //used for executing a command that returns data, IE Gets/Selects
        public void ExecuteReader(String connectionString,
        String commandText,
        CommandType commandType,
        Action<System.Data.SqlClient.SqlParameterCollection> inputParamMapper,
        Action<IDataReader, short> singleRecordMapper,
        Action<System.Data.SqlClient.SqlParameterCollection> returnParameters = null)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;

                    inputParamMapper?.Invoke(cmd.Parameters);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        short set = 0; // Initialize the set index
                        while (reader.Read())
                        {
                            singleRecordMapper(reader, set);
                            set++; // Increment the set index if needed (for handling multiple result sets)
                        }
                    }

                    returnParameters?.Invoke(cmd.Parameters);


                }
            }
        }
    }
}
    

