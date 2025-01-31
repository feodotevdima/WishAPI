namespace Core
{
    public class WishModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Present { get; set; }
        public string Price { get; set; }
        public Guid? ReservUser { get; set; }

        public WishModel(Guid userId, string present, string price, Guid? reservUser)
        {
            UserId = userId;
            Present = present;
            Price = price;
            ReservUser = reservUser;
        }

        public WishModel(Guid id, Guid userId, string present, string price, Guid? reservUser)
        {
            Id = id;
            UserId = userId;
            Present = present;
            Price = price;
            ReservUser = reservUser;
        }

        public WishModel(Guid userId, string present, string price)
        {
            UserId = userId;
            Present = present;
            Price = price;
        }
    }
}
