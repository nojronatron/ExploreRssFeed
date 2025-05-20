using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreGetRssFeed.Data
{
    public class FeedEntryDataModel
    {
        // integer Id:
        // simple, auto incrementing, non-distributed db, fast indexing
        // string Id:
        // distributed db, human-readable, globally unique, must be generated in code (not automatic)
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string WebAddress { get; set; }

        [Required]
        [MaxLength(100)]
        public string RouteName { get; set; }

        public bool OpenInNewTab { get; set; } = false;
    }
}
