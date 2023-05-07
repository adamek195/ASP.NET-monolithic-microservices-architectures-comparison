using Multimedia.Web.Dtos.Validations;

namespace Multimedia.Web.Dtos
{
    public class HashtagDto
    {
        [HashtagValidation]
        public string Hashtag { get; set; }
    }
}
