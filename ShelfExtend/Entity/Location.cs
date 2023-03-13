namespace ShelfExtend.Entity
{
    public class Location
    {
        public int Location_id { get; set; }
        public string? City { get; set; }

        public static List<Location> All { get; set; } = new List<Location>();
    }
}
