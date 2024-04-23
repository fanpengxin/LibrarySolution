namespace LibraryWebApi.Dtos
{
    public class BorrowBooksDto
    {
        public int Id { get; set; }
        public int BorrowerId { get; set; }
        public int BookId { get; set; }
        public DateTime ExpireDate { get; set; }       
    }
}
