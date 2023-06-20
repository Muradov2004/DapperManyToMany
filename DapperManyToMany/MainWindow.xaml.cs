using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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


            DataTable dataTable = new DataTable();


            dataTable.Columns.Add("Author Id", typeof(int));
            dataTable.Columns.Add("Author Name", typeof(string));
            dataTable.Columns.Add("Book Name", typeof(string));
            dataTable.Columns.Add("Price", typeof(decimal));

            // Add rows to the DataTable
            foreach (var author in authors)
            {
                foreach (var book in author.Books)
                {
                    dataTable.Rows.Add(author.Id, author.Name, book.Name, book.Price);
                }
            }
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

    }
}


