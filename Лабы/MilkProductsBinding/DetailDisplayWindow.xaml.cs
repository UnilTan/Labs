using System.Windows;
using MilkProductsBinding.Models;

namespace MilkProductsBinding
{
    public partial class DetailDisplayWindow : Window
    {
        public DetailDisplayWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Устанавливает данные для отображения
        /// </summary>
        /// <param name="saleDetail">Деталь продажи с загруженным продуктом</param>
        public void SetData(SaleDetails saleDetail)
        {
            if (saleDetail?.Product != null)
            {
                txtArticul.Text = saleDetail.IdProductDetailSale.ToString();
                txtName.Text = saleDetail.Product.nameProduct;
                txtPrice.Text = saleDetail.Product.priceProduct.ToString("F4");
                txtQuantity.Text = saleDetail.QuantityProduct.ToString();
            }
        }

        /// <summary>
        /// Устанавливает данные напрямую (альтернативный метод)
        /// </summary>
        public void SetData(string articul, string name, string price, string quantity)
        {
            txtArticul.Text = articul;
            txtName.Text = name;
            txtPrice.Text = price;
            txtQuantity.Text = quantity;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}