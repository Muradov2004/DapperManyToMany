using System.Collections.Generic;

namespace DapperManyToMany;

internal class AuthorBook
{
    public int AuthorId { get; set; }
    public int BookId { get; set; }
    public List<Book> Books { get; set; } = new();
    public List<Author> Authors { get; set; } = new();
    public override string ToString() => $"{AuthorId} {BookId}";
}
