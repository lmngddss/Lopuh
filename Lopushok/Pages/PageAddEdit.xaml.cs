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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lopushok.Model;

namespace Lopushok.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddEdit.xaml
    /// </summary>
    public partial class PageAddEdit : Page
    {
        Product product = new Product();

        public PageAddEdit(Product currentProd)
        {
            InitializeComponent();

            if (product.ID == 0)
            {
                btnDelete.Visibility = Visibility.Hidden;
            }

            if (currentProd != null)
            {
                product = currentProd;
            }

            cbTypes.ItemsSource = DB.db.ProductType.ToList();

            DataContext = product;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(product.Title))
                errors.AppendLine("Укажите название продукта");
            if (string.IsNullOrWhiteSpace(product.ArticleNumber))
                errors.AppendLine("Укажите артикул продукта");
            if (string.IsNullOrWhiteSpace(product.MinCostForAgent.ToString()))
                errors.AppendLine("Укажите минимальную стоимость");
            if (string.IsNullOrWhiteSpace(product.ProductionWorkshopNumber.ToString()))
                errors.AppendLine("Укажите номер цеха производства");
            if (string.IsNullOrWhiteSpace(product.ProductionPersonCount.ToString()))
                errors.AppendLine("Укажите кол-во человек для производства");
            if (product.ProductType == null)
                errors.AppendLine(cbTypes.Text.ToString());

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (product.ID == 0)
            {
                DB.db.Product.Add(product);
            }

            try
            {
                DB.db.SaveChanges();
                MessageBox.Show("Данные добавлены", "Уведомление");
                Manager.mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DB.db.Product.Remove(product);
                    DB.db.SaveChanges();
                    MessageBox.Show("Запись удалена", "Уведомление");
                    Manager.mainFrame.GoBack();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
