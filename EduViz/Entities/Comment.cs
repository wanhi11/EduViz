using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduViz.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Post")]
        public Guid PostId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        public Guid? ParentCommentId { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }
    }

}
