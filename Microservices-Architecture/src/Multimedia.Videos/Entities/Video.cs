using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Multimedia.Videos.Entities
{
    public class Video
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public string Hashtag { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
