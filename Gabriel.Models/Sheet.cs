using System;

namespace Gabriel.Models
{
    public class Sheet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Composer { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
