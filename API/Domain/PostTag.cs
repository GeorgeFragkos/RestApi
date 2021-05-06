using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain
{
    public class PostTag
    {
        [ForeignKey(nameof(TagName))]
        public virtual Tag Tag { get; set; }

        public string TagName { get; set; }

        public virtual Post Post { get; set; }

        public Guid PostId { get; set; }

    }
}
