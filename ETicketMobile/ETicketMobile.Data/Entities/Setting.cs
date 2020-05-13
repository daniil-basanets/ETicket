using SQLite;

namespace ETicketMobile.Data.Entities
{
    public class Setting
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}