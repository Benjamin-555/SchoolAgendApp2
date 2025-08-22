using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAgend.Domain.Entities
{
    public class TaskI
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        [MaxLength(150)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        // Relación
        [ForeignKey("CourseId")]
        public byte[] RowVersion { get; set; }
        public TaskI()
        {
            // RowVersion de 8 bytes aleatorio
            RowVersion = Guid.NewGuid().ToByteArray()[..8];
        }
    }
}
