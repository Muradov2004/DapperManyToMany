using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
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
            //var result = conn.Query<AuthorBook, Author, Book, AuthorBook>(sql,
            //    (authorBook, author, book) =>
            //    {
            //        authorBook.Books.Add(book);
            //        authorBook.Authors.Add(author);
            //        return authorBook;
            //    });
            var result = conn.Query(sql).Select(row =>new 
            {
                Author =Name
            })
            dataGrid.ItemsSource = result;
        }
    }
}
