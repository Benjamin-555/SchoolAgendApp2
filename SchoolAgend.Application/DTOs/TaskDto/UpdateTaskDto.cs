namespace SchoolAgendCRUD.DTOs
{
    public class UpdateTaskDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public object SessionId { get; set; }
    }
}
