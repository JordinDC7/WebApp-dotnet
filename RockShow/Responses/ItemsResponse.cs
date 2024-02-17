namespace RockShow.Responses
{
    public class ItemsResponse<T> : SuccessResponse 
    {
       
        /// <typeparam name="T"></typeparam>
            public List<T> Items { get; set; }
            public T Item { get; set; }
    }
    }

