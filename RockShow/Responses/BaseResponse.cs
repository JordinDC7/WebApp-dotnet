using System;

namespace RockShow.Responses
{
    public class BaseResponse
    {
        public bool IsSuccessful { get; set; }

        public string TransactionId { get; set; }

        public BaseResponse()
        {
            // This TxId we are just faking to demo the purpose
            this.TransactionId = Guid.NewGuid().ToString();
        }
    }
}
