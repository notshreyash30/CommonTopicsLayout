using System;
using System.ComponentModel.DataAnnotations;

namespace CommonTopicsLayout.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Summary { get; set; }
        public string? Category { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public string? AuthorName { get; set; }
    }
}