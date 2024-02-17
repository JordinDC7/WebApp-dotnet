using RockShow.Domain.Rocks;
using RockShow.Requests.Ratings;
using RockShow.Services;

namespace RockShow.Interfaces
{
    public interface IRockServiceNew 
    {
        List<RockModel> GetAll();
        int Add(AddRatingModel model);
    }
}