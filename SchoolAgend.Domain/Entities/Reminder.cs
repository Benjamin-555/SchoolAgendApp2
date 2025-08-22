using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace SchoolAgend.Domain.Entities
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }
        public int? CourseId { get; set; }
        [MaxLength(200)]
        public string Message { get; set; }
        public DateTime ReminderDateTime { get; set; }
        // Relación opcional
        [ForeignKey("CourseId")]
        public byte[] RowVersion { get; set; }

        public Reminder()
        {
            // RowVersion de 8 bytes aleatorio
            RowVersion = Guid.NewGuid().ToByteArray()[..8];
        }
    }
}
