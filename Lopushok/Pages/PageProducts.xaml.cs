using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Lopushok.Model;

namespace Lopushok.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageProducts.xaml
    /// </summary>
    public partial class PageProducts : Page
    {
        public PageProducts()
        {
            InitializeComponent();

            var currentProducts = DB.db.Product.ToList();
            var sort = new List<string>();
            var types = DB.db.ProductType.ToList();

            sort.Add("Сортировка");
            sort.Add("По наименованию");
            sort.Add("По номеру цеха");
            sort.Add("По мин. стоимости");

            lvProducts.ItemsSource = currentProducts;

            types.Insert(0, new ProductType{
                Title = "Все типы"
            });
            cbTypes.DisplayMemberPath = "Title";
            cbTypes.SelectedValuePath = "ID";

            cbSort.ItemsSource = sort;
            cbTypes.ItemsSource = types;

            cbTypes.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;

            
        }

        void UpdateProducts()
        {
            var currentProducts = DB.db.Product.ToList();

            if (cbSort.SelectedIndex > 0)
            {
                switch (cbSort.SelectedIndex)
                {
                    case 1:
                        if (rbAsc.IsChecked == true)
                        {
                            currentProducts = currentProducts.OrderBy(p => p.Title).ToList();
                        }
                        else
                            currentProducts = currentProducts.OrderByDescending(p => p.Title).ToList();
                        break;
                    case 2:
                        if (rbAsc.IsChecked == true)
                        {
                            currentProducts = currentProducts.OrderBy(p => p.ProductionWorkshopNumber).ToList();
                        }
                        else
                            currentProducts = currentProducts.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
                        break;
                    case 3:
                        if (rbAsc.IsChecked == true)
                        {
                            currentProducts = currentProducts.OrderBy(p => p.MinCostForAgent).ToList();
                        }
                        else
                            currentProducts = currentProducts.OrderByDescending(p => p.MinCostForAgent).ToList();
                        break;
                }
            }

            if (cbTypes.SelectedIndex > 0)
            {
                currentProducts = currentProducts.Where(p => p.ProductTypeID == int.Parse(cbTypes.SelectedValue.ToString())).ToList();
            }

            if (tbFinder.Text != null)
            {
                currentProducts = currentProducts.Where(p => p.Title.Contains(tbFinder.Text)).ToList();
            }

            lvProducts.ItemsSource = currentProducts;
           
        }

        private void rbAsc_Click(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new PageAddEdit(null));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new PageAddEdit((sender as Button).DataContext as Product));
        }

        private void cbTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void tbFinder_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void lvProducts_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DB.db.ChangeTracker.Entries().ToList().ForEach(a => a.Reload());
            lvProducts.ItemsSource = DB.db.Product.ToList();
        }
    }
}
