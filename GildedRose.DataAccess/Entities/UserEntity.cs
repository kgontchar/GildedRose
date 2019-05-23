using GildedRose.DataAccess.Interfaces.Entities;

namespace GildedRose.DataAccess.Entities
{
    public class UserEntity : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
