using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApi.Entities
{
    [Table("Book")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(2500)]
        public string? Description { get; set; }

        [MaxLength(200)]
        public string? Author { get; set; }

        [Timestamp]
        public byte[]? Timestamp { get; set; }

        public Book(int id, string title, string description, string author)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
        }
    }
}
