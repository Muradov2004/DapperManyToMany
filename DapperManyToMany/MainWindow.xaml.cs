using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;

namespace DapperManyToMany;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        string connectionString;

        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("AppConfig.json");
        var config = builder.Build();
        connectionString = config.GetConnectionString("DefaultConnection")!;

        using (var conn = new SqlConnection(connectionString))
        {
            var sql = @"SELECT a.[Id], a.[Name], b.[Name], b.[Price]
                        FROM AuthorBook AS ab
                        INNER JOIN Authors AS a
                        ON a.Id = ab.AuthorId
                        INNER JOIN Books AS b
                        ON b.Id = ab.BookId";
            var result = conn.Query<Author, Book, Author>(sql,
                (author, book) =>
                {
                    author.Books.Add(book);
                    return author;
                });
            dataGrid.ItemsSource = result;
        }
    }
}
