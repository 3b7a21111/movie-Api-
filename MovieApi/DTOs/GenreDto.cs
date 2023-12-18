using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs
{
    public class GenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
