using GildedRose.DataAccess.Interfaces.Entities;

namespace GildedRose.DataAccess.Entities
{
    public class ItemEntity : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int ItemsLeft { get; set; }
    }
}
