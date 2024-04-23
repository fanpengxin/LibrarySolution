﻿using System.ComponentModel.DataAnnotations;

namespace LibraryWebApi.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public List<BorrowerDto> BorrowerDtos { get; set; } = new List<BorrowerDto>();
    }
}
