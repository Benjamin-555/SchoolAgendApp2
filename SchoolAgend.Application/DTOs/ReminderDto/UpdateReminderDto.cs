namespace SchoolAgendCRUD.DTOs
{
    public class UpdateReminderDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? CourseId { get; set; }
        public string Message { get; set; }
        public DateTime ReminderDateTime { get; set; }
    }
}
