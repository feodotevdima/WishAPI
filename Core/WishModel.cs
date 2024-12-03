namespace Core
{
    public class WishModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<WishList> Presents { get; set; }

        public WishModel(Guid userId, string userName, List<WishList> presents)
        {
            UserId = userId;
            UserName = userName;
            Presents = presents;
        }
    }
}
