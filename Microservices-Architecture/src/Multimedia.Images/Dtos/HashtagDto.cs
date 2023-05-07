using Multimedia.Images.Dtos.Validations;

namespace Multimedia.Images.Dtos
{
    public class HashtagDto
    {
        [HashtagValidation]
        public string Hashtag { get; set; }
    }
}
