namespace AssignmentSortechMvc.Dto
{
    public class EventDto
    {
        public string? Id { get; set; }
        public string Summary { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string StartDateTime { get; set; } = null!;
        public string EndDateTime { get; set; } = null!;
    }
}
