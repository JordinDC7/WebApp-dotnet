using RockShow.Requests.Rocks;

namespace RockShow.Domain.Rocks
{
    public class RockModel : AddRockModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }

        public double ShippingCost { get; set; }

        public int TotalReview { get; set; }

        public bool isInStock { get; set; }

        public bool isNew { get; set; }

        public string Image { get; set; }


    }
}
