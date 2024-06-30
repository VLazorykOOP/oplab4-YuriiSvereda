using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SQLinEFcore_hw
{
    /// <summary>
    /// Interaction logic for ShowWindow.xaml
    /// </summary>
    public partial class ShowWindow : Window
    {
        public ShowWindow()
        {
            InitializeComponent();
        }

        private async void GetUsers(object sender, RoutedEventArgs e)
        {
            using MyDbContext db = new MyDbContext();
            var usersNames = await db.User.Select(user => user.Name).ToListAsync();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var name in usersNames)
            {
                stringBuilder
                    .Append(name)
                    .Append(";\n");

            }
            AllUsers.Text = stringBuilder.ToString();

        }

        private async void GetProducts(object sender, RoutedEventArgs e)
        {
            using MyDbContext db = new MyDbContext();
            var productsNames = await db.Product.Select(product => product.Name).ToListAsync();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var name in productsNames)
            {
                stringBuilder
                    .Append(name)
                    .Append(";\n");

            }
            AllProducts.Text = stringBuilder.ToString();
        }

        private async void GetCategories(object sender, RoutedEventArgs e)
        {
            using MyDbContext db = new MyDbContext();
            var categoriesNames = await db.Category.Select(category => category.Name).ToListAsync();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var name in categoriesNames)
            {
                stringBuilder
                    .Append(name)
                    .Append(";\n");

            }
            AllCategories.Text = stringBuilder.ToString();
        }

        private async void GetPurchases(object sender, RoutedEventArgs e)
        {
            using MyDbContext db = new MyDbContext();
            var purchases = await db.Cart
                .Include(cart => cart.User)
                .Include(cart => cart.Product)
                .ToListAsync();
            List<Guid> userIds = new();
            AllPurchases.Inlines.Clear();
            foreach (var purchase in purchases)
            {
                var currentId = purchase.UserId;
                if (!userIds.Contains(currentId))
                {
                    Run boldRun = new Run(purchase.User.Name + ":\n");
                    boldRun.FontWeight = FontWeights.Bold;
                    AllPurchases.Inlines.Add(boldRun);
                    userIds.Add(currentId);
                    var productsUserBout = purchases
                        .Where(cart => cart.UserId == currentId)
                        .Select(cart => cart.Product.Name)
                        .ToList();
                    StringBuilder sb = new();
                    foreach (var product in productsUserBout)
                    {
                        sb.Append(product)
                            .Append(", ");
                    }
                    sb.AppendLine();
                    AllPurchases.Inlines.Add(new Run(sb.ToString()));
                }
            }

            // Додати частину тексту звичайним шрифтом
            /*AllPurchases.Inlines.Add(new Run("Це звичайний текст, "));

            // Додати частину тексту жирним шрифтом
            Run boldRun = new Run("а цей текст буде жирним.");
            boldRun.FontWeight = FontWeights.Bold;
            AllPurchases.Inlines.Add(boldRun);*/
        }
    }
}
