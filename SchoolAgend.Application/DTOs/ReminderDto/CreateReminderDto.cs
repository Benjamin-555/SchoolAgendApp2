namespace SchoolAgendCRUD.DTOs
{
    public class CreateReminderDto
    {
        public int? CourseId { get; set; }
        public string Message { get; set; }
        public DateTime ReminderDateTime { get; set; }
    }
}
