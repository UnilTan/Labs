namespace MilkProductsCatalog
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnPrice = new System.Windows.Forms.Button();
            this.btnSortAsc = new System.Windows.Forms.Button();
            this.btnSortDesc = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtMaxPrice = new System.Windows.Forms.TextBox();
            this.txtMinPrice = new System.Windows.Forms.TextBox();
            this.txtAvgPrice = new System.Windows.Forms.TextBox();
            this.lblMaxPrice = new System.Windows.Forms.Label();
            this.lblMinPrice = new System.Windows.Forms.Label();
            this.lblAvgPrice = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.btnPartialSearch = new System.Windows.Forms.Button();
            this.txtPartialSearch = new System.Windows.Forms.TextBox();
            this.lblPartialSearch = new System.Windows.Forms.Label();
            this.btnSales = new System.Windows.Forms.Button();
            this.btnSaleDetails = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnDataManagement = new System.Windows.Forms.Button();
            this.btnProductTable = new System.Windows.Forms.Button();
            this.btnSalesManagement = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(312, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Каталог Молочных продуктов";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(776, 300);
            this.dataGridView1.TabIndex = 1;
            // 
            // btnPrice
            // 
            this.btnPrice.Location = new System.Drawing.Point(12, 370);
            this.btnPrice.Name = "btnPrice";
            this.btnPrice.Size = new System.Drawing.Size(100, 30);
            this.btnPrice.TabIndex = 2;
            this.btnPrice.Text = "Цена";
            this.btnPrice.UseVisualStyleBackColor = true;
            this.btnPrice.Click += new System.EventHandler(this.btnPrice_Click);
            // 
            // btnSortAsc
            // 
            this.btnSortAsc.Location = new System.Drawing.Point(130, 370);
            this.btnSortAsc.Name = "btnSortAsc";
            this.btnSortAsc.Size = new System.Drawing.Size(150, 30);
            this.btnSortAsc.TabIndex = 3;
            this.btnSortAsc.Text = "Сортировка по возр.";
            this.btnSortAsc.UseVisualStyleBackColor = true;
            this.btnSortAsc.Click += new System.EventHandler(this.btnSortAsc_Click);
            // 
            // btnSortDesc
            // 
            this.btnSortDesc.Location = new System.Drawing.Point(300, 370);
            this.btnSortDesc.Name = "btnSortDesc";
            this.btnSortDesc.Size = new System.Drawing.Size(150, 30);
            this.btnSortDesc.TabIndex = 4;
            this.btnSortDesc.Text = "Сортировка по убыв.";
            this.btnSortDesc.UseVisualStyleBackColor = true;
            this.btnSortDesc.Click += new System.EventHandler(this.btnSortDesc_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(470, 370);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Поиск";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(590, 375);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(198, 23);
            this.txtSearch.TabIndex = 6;
            // 
            // txtMaxPrice
            // 
            this.txtMaxPrice.Location = new System.Drawing.Point(120, 420);
            this.txtMaxPrice.Name = "txtMaxPrice";
            this.txtMaxPrice.ReadOnly = true;
            this.txtMaxPrice.Size = new System.Drawing.Size(100, 23);
            this.txtMaxPrice.TabIndex = 7;
            // 
            // txtMinPrice
            // 
            this.txtMinPrice.Location = new System.Drawing.Point(120, 450);
            this.txtMinPrice.Name = "txtMinPrice";
            this.txtMinPrice.ReadOnly = true;
            this.txtMinPrice.Size = new System.Drawing.Size(100, 23);
            this.txtMinPrice.TabIndex = 8;
            // 
            // txtAvgPrice
            // 
            this.txtAvgPrice.Location = new System.Drawing.Point(120, 480);
            this.txtAvgPrice.Name = "txtAvgPrice";
            this.txtAvgPrice.ReadOnly = true;
            this.txtAvgPrice.Size = new System.Drawing.Size(100, 23);
            this.txtAvgPrice.TabIndex = 9;
            // 
            // lblMaxPrice
            // 
            this.lblMaxPrice.AutoSize = true;
            this.lblMaxPrice.Location = new System.Drawing.Point(12, 423);
            this.lblMaxPrice.Name = "lblMaxPrice";
            this.lblMaxPrice.Size = new System.Drawing.Size(102, 15);
            this.lblMaxPrice.TabIndex = 10;
            this.lblMaxPrice.Text = "Максимальная:";
            // 
            // lblMinPrice
            // 
            this.lblMinPrice.AutoSize = true;
            this.lblMinPrice.Location = new System.Drawing.Point(12, 453);
            this.lblMinPrice.Name = "lblMinPrice";
            this.lblMinPrice.Size = new System.Drawing.Size(95, 15);
            this.lblMinPrice.TabIndex = 11;
            this.lblMinPrice.Text = "Минимальная:";
            // 
            // lblAvgPrice
            // 
            this.lblAvgPrice.AutoSize = true;
            this.lblAvgPrice.Location = new System.Drawing.Point(12, 483);
            this.lblAvgPrice.Name = "lblAvgPrice";
            this.lblAvgPrice.Size = new System.Drawing.Size(58, 15);
            this.lblAvgPrice.TabIndex = 12;
            this.lblAvgPrice.Text = "Средняя:";
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(590, 357);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(134, 15);
            this.lblSearch.TabIndex = 13;
            this.lblSearch.Text = "Поиск по названию:";
            // 
            // btnPartialSearch
            // 
            this.btnPartialSearch.Location = new System.Drawing.Point(300, 420);
            this.btnPartialSearch.Name = "btnPartialSearch";
            this.btnPartialSearch.Size = new System.Drawing.Size(150, 30);
            this.btnPartialSearch.TabIndex = 14;
            this.btnPartialSearch.Text = "Поиск по символам";
            this.btnPartialSearch.UseVisualStyleBackColor = true;
            this.btnPartialSearch.Click += new System.EventHandler(this.btnPartialSearch_Click);
            // 
            // txtPartialSearch
            // 
            this.txtPartialSearch.Location = new System.Drawing.Point(470, 425);
            this.txtPartialSearch.Name = "txtPartialSearch";
            this.txtPartialSearch.Size = new System.Drawing.Size(150, 23);
            this.txtPartialSearch.TabIndex = 15;
            // 
            // lblPartialSearch
            // 
            this.lblPartialSearch.AutoSize = true;
            this.lblPartialSearch.Location = new System.Drawing.Point(470, 407);
            this.lblPartialSearch.Name = "lblPartialSearch";
            this.lblPartialSearch.Size = new System.Drawing.Size(150, 15);
            this.lblPartialSearch.TabIndex = 16;
            this.lblPartialSearch.Text = "Первые символы:";
            // 
            // btnSales
            // 
            this.btnSales.Location = new System.Drawing.Point(650, 420);
            this.btnSales.Name = "btnSales";
            this.btnSales.Size = new System.Drawing.Size(138, 30);
            this.btnSales.TabIndex = 17;
            this.btnSales.Text = "Продажи";
            this.btnSales.UseVisualStyleBackColor = true;
            this.btnSales.Click += new System.EventHandler(this.btnSales_Click);
            // 
            // btnSaleDetails
            // 
            this.btnSaleDetails.Location = new System.Drawing.Point(650, 460);
            this.btnSaleDetails.Name = "btnSaleDetails";
            this.btnSaleDetails.Size = new System.Drawing.Size(138, 30);
            this.btnSaleDetails.TabIndex = 18;
            this.btnSaleDetails.Text = "Детали продаж";
            this.btnSaleDetails.UseVisualStyleBackColor = true;
            this.btnSaleDetails.Click += new System.EventHandler(this.btnSaleDetails_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(650, 500);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(138, 30);
            this.btnTestConnection.TabIndex = 19;
            this.btnTestConnection.Text = "Тест подключения";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // btnDataManagement
            // 
            this.btnDataManagement.Location = new System.Drawing.Point(350, 500);
            this.btnDataManagement.Name = "btnDataManagement";
            this.btnDataManagement.Size = new System.Drawing.Size(138, 30);
            this.btnDataManagement.TabIndex = 20;
            this.btnDataManagement.Text = "Управление данными";
            this.btnDataManagement.UseVisualStyleBackColor = true;
            this.btnDataManagement.Click += new System.EventHandler(this.btnDataManagement_Click);
            // 
            // btnProductTable
            // 
            this.btnProductTable.Location = new System.Drawing.Point(500, 500);
            this.btnProductTable.Name = "btnProductTable";
            this.btnProductTable.Size = new System.Drawing.Size(138, 30);
            this.btnProductTable.TabIndex = 21;
            this.btnProductTable.Text = "Моя таблица";
            this.btnProductTable.UseVisualStyleBackColor = true;
            this.btnProductTable.Click += new System.EventHandler(this.btnProductTable_Click);
            // 
            // btnSalesManagement
            // 
            this.btnSalesManagement.Location = new System.Drawing.Point(650, 540);
            this.btnSalesManagement.Name = "btnSalesManagement";
            this.btnSalesManagement.Size = new System.Drawing.Size(138, 30);
            this.btnSalesManagement.TabIndex = 22;
            this.btnSalesManagement.Text = "Управление продажами";
            this.btnSalesManagement.UseVisualStyleBackColor = true;
            this.btnSalesManagement.Click += new System.EventHandler(this.btnSalesManagement_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 580);
            this.Controls.Add(this.btnSalesManagement);
            this.Controls.Add(this.btnProductTable);
            this.Controls.Add(this.btnDataManagement);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.btnSaleDetails);
            this.Controls.Add(this.btnSales);
            this.Controls.Add(this.lblPartialSearch);
            this.Controls.Add(this.txtPartialSearch);
            this.Controls.Add(this.btnPartialSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.lblAvgPrice);
            this.Controls.Add(this.lblMinPrice);
            this.Controls.Add(this.lblMaxPrice);
            this.Controls.Add(this.txtAvgPrice);
            this.Controls.Add(this.txtMinPrice);
            this.Controls.Add(this.txtMaxPrice);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnSortDesc);
            this.Controls.Add(this.btnSortAsc);
            this.Controls.Add(this.btnPrice);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblTitle);
            this.Name = "Form1";
            this.Text = "Каталог Молочных продуктов";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private DataGridView dataGridView1;
        private Button btnPrice;
        private Button btnSortAsc;
        private Button btnSortDesc;
        private Button btnSearch;
        private TextBox txtSearch;
        private TextBox txtMaxPrice;
        private TextBox txtMinPrice;
        private TextBox txtAvgPrice;
        private Label lblMaxPrice;
        private Label lblMinPrice;
        private Label lblAvgPrice;
        private Label lblSearch;
        private Button btnPartialSearch;
        private TextBox txtPartialSearch;
        private Label lblPartialSearch;
        private Button btnSales;
        private Button btnSaleDetails;
        private Button btnTestConnection;
        private Button btnDataManagement;
        private Button btnProductTable;
        private Button btnSalesManagement;
    }
}