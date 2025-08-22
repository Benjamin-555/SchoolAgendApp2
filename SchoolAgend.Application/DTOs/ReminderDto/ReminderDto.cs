using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAgend.Application.Dto
{
    public class ReminderDto
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string Message { get; set; }
        public DateTime ReminderDateTime { get; set; }
    }
}
