using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DapperManyToMany
{
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
                var sql = @"SELECT C.[Id], C.[Name], P.[Id], P.[Name], P.CategoryId
                            FROM Categories AS C
                            INNER JOIN Products AS P
                            ON C.Id = P.CategoryId";
                var result = conn.Query<Category, Product, Product>(sql,
                    (category, product) =>
                    {
                        product.Category = category;
                        return product;
                    });
                dataGrid.ItemsSource = result;
            }
        }
    }
}
