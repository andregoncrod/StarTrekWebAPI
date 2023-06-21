namespace StarTrekWebAPI.Models
{
    public class SpacecraftsPagedDto
    {
        public int Total { get; set; }
        public int Filtered { get; set; }
        public IEnumerable<Spacecraft> Results { get; set; }
    }
}
