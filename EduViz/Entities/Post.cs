using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{

    [Table("Posts")]
    public class Post
    {
        [Key]
        public Guid PostId { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Class")]
        public Guid ClassId { get; set; }

        public virtual Class Class { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
