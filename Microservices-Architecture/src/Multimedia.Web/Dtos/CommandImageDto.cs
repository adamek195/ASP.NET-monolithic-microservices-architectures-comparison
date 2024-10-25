﻿using Multimedia.Web.Dtos.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Multimedia.Web.Dtos
{
    public class CommandImageDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Title { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [HashtagValidation(ErrorMessage = "Hashtag does not contain #.")]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Hashtag { get; set; }
    }
}
