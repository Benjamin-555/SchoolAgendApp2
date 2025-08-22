namespace SchoolAgendCRUD.DTOs
{
    public class UpdateSessionDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int DayOfWeek { get; set; } // 1=Monday ... 7=Sunday
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Classroom { get; set; }
        public object Title { get; set; }
    }
}
