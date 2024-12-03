namespace Core
{
    public class WishList
    {
        public string Present { get; set; }
        public int Price { get; set; }

        public WishList(string present, int price)
        {
            Present = present;
            Price = price;
        }
    }
}
