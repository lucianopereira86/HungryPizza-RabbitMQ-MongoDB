namespace HungryPizza.Infra.Data.MongoDB.Collections
{
    public class UserCollection : BaseCollection
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

    }
}
