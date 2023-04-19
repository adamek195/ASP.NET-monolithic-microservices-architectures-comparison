using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace MonolithicMultimedia.Entities
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public string Hashtag { get; set; }
    }
}
