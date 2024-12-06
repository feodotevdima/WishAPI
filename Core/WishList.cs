namespace Core
{
    public class WishList
    {
        public List<string> Present { get; set; }
        public List<int> Price { get; set; }

        public WishList(List<string> present, List<int> price)
        {
            Present = present;
            Price = price;
        }
    }
}
