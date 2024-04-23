namespace LibraryWebApi.Dtos
{
    public class BorrowerDto
    {    
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<BookDto> BookDtos { get; set; } = new List<BookDto>();
    }
}
