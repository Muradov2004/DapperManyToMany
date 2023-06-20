using System.Collections.Generic;

namespace DapperManyToMany;

internal class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public List<Author> Authors { get; set; } = new();
    public override string ToString() => $"{Name} - {Price}";
}
