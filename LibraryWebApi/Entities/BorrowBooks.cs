using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApi.Entities
{
    [Table("BorrowBooks")]
    public class BorrowBooks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int BorrowerId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }

        [Timestamp]
        public byte[]? Timestamp { get; set; }

        public BorrowBooks(int id, int borrowerId, int bookId, DateTime expireDate)
        {
            Id = id;
            BorrowerId = borrowerId;
            BookId = bookId;
            ExpireDate = expireDate;
        }
    }
}
