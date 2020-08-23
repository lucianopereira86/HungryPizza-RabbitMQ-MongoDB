using System;

namespace HungryPizza.Infra.Data.MongoDB.Collections
{
    public class RequestCollection: BaseCollection
    {
        public RequestCollection(DateTime createdAt, Guid uid, int quantity, decimal total, int? idCustomer, string address, bool active)
        {
            CreatedAt = createdAt;
            Uid = uid;
            Quantity = quantity;
            Total = total;
            IdCustomer = idCustomer;
            Address = address;
            Active = active;
        }

        public DateTime CreatedAt { get; private set; }
        public Guid Uid { get; private set; }
        public int Quantity { get; private set; }
        public decimal Total { get; private set; }
        public int? IdCustomer { get; private set; }
        public string Address { get; private set; }
        public bool Active { get; private set; }

        public void SetAddress(string address)
        {
            Address = address;
        }

    }
}
