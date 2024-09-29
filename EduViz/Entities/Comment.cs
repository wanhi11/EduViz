using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public Guid commentId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR")]
        public string content { get; set; }

        [ForeignKey("Post")]
        public Guid postId { get; set; }

        [ForeignKey("User")]
        public Guid userId { get; set; }

        public Guid? parentCommentId { get; set; }

        public virtual Post post { get; set; }
        public virtual User user { get; set; }
        public virtual Comment parentComment { get; set; }
        public virtual ICollection<Comment> replies { get; set; }
    }

}
