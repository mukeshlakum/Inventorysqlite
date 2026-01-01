namespace Inventory.Model
{
    public class ItemDto
    {
        private const float GSTPercentage = 18;
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category   { get; set; }
        public  double GSTPrice{ get; set; }

        private double _price;

        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                GSTPrice = value + (value / GSTPercentage);
            }
        }

       


    }
}
