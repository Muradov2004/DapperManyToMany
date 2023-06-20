using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

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

            var sql = @"SELECT a.[Id], a.[Name], b.[Name], b.[Price]
                        FROM Authors AS a
                        INNER JOIN AuthorBook AS ab 
                        ON a.Id = ab.AuthorId
                        INNER JOIN Books AS b
                        ON b.Id = ab.BookId";
            var authors = connection.Query<Author, Book, Author>(sql,
                (author, book) =>
                {
                    author.Books.Add(book);
                    return author;
                },
            splitOn: "Name");

            var result = authors.GroupBy(a => a.Id).Select(g =>
            {
                var groupedBook = g.First();
                groupedBook.Books = g.Select(b => b.Books.Single()).ToList();
                return groupedBook;
            });

            //var mydict = new Dictionary<string, List<string>>()
            //{
            //    { "salam1", new() { "salam11","salam12","salam13" } },
            //    { "salam2", new() { "salam21","salam22","salam23"} },
            //    { "salam3", new() { "salam31","salam32","salam33"} },
            //};

            //foreach (var post in result)
            //{
            //    Console.Write($"{post.Headline}: ");

            //    foreach (var tag in post.Tags)
            //    {
            //        Console.Write($" {tag.TagName} ");
            //    }

            //    Console.Write(Environment.NewLine);
            //}

            dataGrid.ItemsSource = result;

            // Create a column for the author name
            DataGridTextColumn authorColumn = new DataGridTextColumn();
            authorColumn.Header = "Author";
            authorColumn.Binding = new Binding("Name");
            dataGrid.Columns.Add(authorColumn);

            // Iterate over each author to generate book columns dynamically
            foreach (var author in result)
            {
                foreach (Book book in author.Books)
                {
                    // Create a column for each book
                    DataGridTextColumn bookColumn = new DataGridTextColumn();
                    bookColumn.Header = book.Name;  // Use the book title as the column header
                    bookColumn.Binding = new Binding($"Books[{author.Books.IndexOf(book)}].Title");
                    dataGrid.Columns.Add(bookColumn);
                }
            }


        }
    }
}
