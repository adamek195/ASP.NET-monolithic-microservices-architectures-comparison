using System.ComponentModel.DataAnnotations;

namespace Multimedia.Images.Dtos.Validations
{
    public class HashtagValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var hashtag = value as string;
            var isValid = true;

            if (string.IsNullOrEmpty(hashtag))
                return isValid;

            if ((hashtag[0] != '#'))
            {
                isValid = false;
                return isValid;
            }

            return isValid;
        }
    }
}
