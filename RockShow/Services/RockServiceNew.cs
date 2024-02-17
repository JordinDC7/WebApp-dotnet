
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using RockShow.Interfaces;
using System.Xml.Linq;
using RockShow.Domain.Rocks;
using RockShow.Requests.Ratings;

namespace RockShow.Services

{
    public class RockServiceNew : IRockServiceNew 
    {
         string _connectionString = null;

         IDatabaseProcCommands _data = null;
        public RockServiceNew(IConfiguration configuration , IDatabaseProcCommands data)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _data = data;
        }

        public List<RockModel> GetAll()
        {
            List<RockModel> rockList = null;  // Initialize the list to avoid null reference

            string procName = "dbo.Rock_SelectAll";

            _data.ExecuteReader(_connectionString, procName, CommandType.StoredProcedure,
                inputParamMapper: null,  // stored procedure doesn't require parameters, this can be null
                singleRecordMapper: (IDataReader reader, short set) =>  // lambda for clarity
                {
                    int idx = 0;  // Initialize index at the start of each record
                    if (rockList == null) rockList = new List<RockModel>();  // Initialize the list if it's null
                    RockModel rockModel = MapSingleRock(reader, ref idx);
                    rockList.Add(rockModel);
                },
                returnParameters: null  // stored procedure doesn't have output parameters
            );

            return rockList;
        }



        public int Add(AddRatingModel model)
        {
            int id = 0;
            string procName = "dbo.Rating_Insert";
            _data.ExecuteNonQuery(_connectionString, procName, CommandType.StoredProcedure,
                inputParamMapper: delegate (SqlParameterCollection collection)
            {
                AddSingleRating(model, collection);

               
                SqlParameter idOut = new SqlParameter("@id", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                collection.Add(idOut);

                
            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object outPutId = returnCollection["@id"].Value;
                if (outPutId != DBNull.Value)
                {
                    id = Convert.ToInt32(outPutId);
                }
            });
            return id;
        }


        
        private void AddSingleRating(AddRatingModel model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@rock_id", model.rock_id);
            collection.AddWithValue("@rating", model.rating);
        }


        private RockModel MapSingleRock(IDataReader reader, ref int idx)
        {
             
            RockModel rockModel = new RockModel();

            rockModel.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            rockModel.Name = reader.GetString(reader.GetOrdinal("Name"));
            rockModel.Category = reader.GetString(reader.GetOrdinal("Category"));
            rockModel.Price = reader.GetDouble(reader.GetOrdinal("Price"));
            rockModel.SalePrice = reader.GetDouble(reader.GetOrdinal("SalePrice"));
            rockModel.ShippingCost = reader.GetDouble(reader.GetOrdinal("ShippingCost"));
            rockModel.AverageRating = reader.IsDBNull(reader.GetOrdinal("AverageRating"))? (double?)null : reader.GetDouble(reader.GetOrdinal("AverageRating"));
            rockModel.TotalReview = reader.GetInt32(reader.GetOrdinal("TotalReview"));
            rockModel.isInStock = reader.GetBoolean(reader.GetOrdinal("IsInStock"));
            rockModel.isNew = reader.GetBoolean(reader.GetOrdinal("IsNew"));
            rockModel.Image = reader.GetString(reader.GetOrdinal("Image"));


            return rockModel;
        }


    }


}



