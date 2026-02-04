using System.ComponentModel.DataAnnotations.Schema;

namespace Winfocus.LMS.Infrastructure
{
    public class Batch : BaseClass
    {
        public string BatchName { get; set; }
        public string BatchCode { get; set; }
        public Guid SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }

    }
}
