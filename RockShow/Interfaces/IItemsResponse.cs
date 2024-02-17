namespace RockShow.Interfaces
{
    public interface IItemsResponse<T>
    {
        bool IsSuccessful { get; set; }

        string TransactionId { get; set; }

        T Item { get; }
    }
}