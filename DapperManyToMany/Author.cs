namespace DapperManyToMany;

internal class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public AuthorBook AuthorBook { get; set; }
    public override string ToString() => Name;
}
