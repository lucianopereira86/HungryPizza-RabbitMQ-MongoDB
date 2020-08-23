using HungryPizza.Infra.Data.MongoDB.Collections;
using HungryPizza.Infra.Shared.CommandQuery;
using HungryPizza.Infra.Shared.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq.Expressions;

namespace HungryPizza.Domain.Queries.Request
{
    public class RequestGetQuery : CommandQuery
    {
        public RequestGetQuery(int? idCustomer, DateTime createdAt, Guid uid, int quantity, decimal total, string address,
            AppSettings appSettings) : base(appSettings)
        {
            IdCustomer = idCustomer;
            CreatedAt = createdAt;
            Uid = uid;
            Quantity = quantity;
            Total = total;
            Address = address;
        }

        public int? IdCustomer { get; private set; }
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; private set; }
        public Guid Uid { get; private set; }
        public int Quantity { get; private set; }
        public decimal Total { get; private set; }
        public string Address { get; private set; }

        public Expression<Func<RequestCollection, bool>> Get()
        {
            string address = Address ?? string.Empty;
            DateTime dayStart = CreatedAt.Date;
            DateTime dayEnds = CreatedAt.Date.AddDays(1);

            return r => (IdCustomer == null || IdCustomer == 0 || r.IdCustomer == IdCustomer)
                        &&
                        (CreatedAt.Equals(default) || (r.CreatedAt >= dayStart && r.CreatedAt < dayEnds))
                        &&
                        r.Address.ToLower().Contains(address.ToLower().Trim())
                        &&
                        (Quantity == 0 || r.Quantity == Quantity)
                        &&
                        (Total == 0 || r.Total == Total)
                        &&
                        (Uid.Equals(Guid.Empty) || r.Uid.Equals(Uid))
                        &&
                        r.Active;
        }
    }
}
