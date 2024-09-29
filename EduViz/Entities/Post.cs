using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("Posts")]
    public class Post
    {
        [Key]
        public Guid postId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        public string content { get; set; }

        [ForeignKey("Class")]
        public Guid classId { get; set; }

        public virtual Class mentorClass { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
    }
}
