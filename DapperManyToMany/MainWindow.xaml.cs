using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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

        using (var connection = new SqlConnection(connectionString))
        {
            //var authors = new List<Author>();

            var sql = @"SELECT a.[Id], a.[Name], b.[Name], b.[Price]
                        FROM Authors AS a
                        INNER JOIN AuthorBook AS ab 
                        ON a.Id = ab.AuthorId
                        INNER JOIN Books AS b
                        ON b.Id = ab.BookId";
            var result = connection.Query<Author, Book, Author>(sql,
                    (author, book) =>
                    {
                        if (!authors.Exists(c => c.Id == author.Id))
                        {
                            author.Books.Add(book);
                            authors.Add(author);
                        }
                        else
                        {
                            authors
                            .FirstOrDefault(a => a.Id == author.Id)!
                            .Books.Add(book);
                        }
                        return author;
                    });
            dataGrid.ItemsSource = authors;

        }
    }
}
