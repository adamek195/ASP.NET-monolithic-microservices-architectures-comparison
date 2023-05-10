using Multimedia.Videos.Dtos.Validations;

namespace Multimedia.Videos.Dtos
{
    public class HashtagDto
    {
        [HashtagValidation]
        public string Hashtag { get; set; }
    }
}
