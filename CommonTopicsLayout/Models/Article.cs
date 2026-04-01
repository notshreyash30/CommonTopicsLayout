using System;
using System.Collections.Generic;

namespace CommonTopicsLayout.Models;

public partial class Article
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Summary { get; set; }

    public string? Category { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string? AuthorName { get; set; }
}
