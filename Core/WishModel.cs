namespace Core
{
    public class WishModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Present { get; set; }
        public string Price { get; set; }
        public Guid? ReservUser { get; set; }
        public string? ReservUserName { get; set; }

        public WishModel(Guid userId, string userName, string present, string price, Guid? reservUser, string? reservUserName)
        {
            UserId = userId;
            UserName = userName;
            Present = present;
            Price = price;
            ReservUser = reservUser;
            ReservUserName = reservUserName;
        }

        public WishModel(Guid id, Guid userId, string userName, string present, string price, Guid? reservUser, string? reservUserName)
        {
            Id = id;
            UserId = userId;
            UserName = userName;
            Present = present;
            Price = price;
            ReservUser = reservUser;
            ReservUserName = reservUserName;
        }

        public WishModel(Guid userId, string userName, string present, string price)
        {
            UserId = userId;
            UserName = userName;
            Present = present;
            Price = price;
        }
    }
}
