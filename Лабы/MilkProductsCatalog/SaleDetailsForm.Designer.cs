namespace MilkProductsCatalog
{
    partial class SaleDetailsForm
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
            this.btnSortByQuantity = new System.Windows.Forms.Button();
            this.btnSearchProduct = new System.Windows.Forms.Button();
            this.txtSearchProduct = new System.Windows.Forms.TextBox();
            this.txtMaxQuantity = new System.Windows.Forms.TextBox();
            this.txtMinQuantity = new System.Windows.Forms.TextBox();
            this.txtAvgQuantity = new System.Windows.Forms.TextBox();
            this.lblMaxQuantity = new System.Windows.Forms.Label();
            this.lblMinQuantity = new System.Windows.Forms.Label();
            this.lblAvgQuantity = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(178, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Детали продаж";
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
            // btnSortByQuantity
            // 
            this.btnSortByQuantity.Location = new System.Drawing.Point(12, 370);
            this.btnSortByQuantity.Name = "btnSortByQuantity";
            this.btnSortByQuantity.Size = new System.Drawing.Size(150, 30);
            this.btnSortByQuantity.TabIndex = 2;
            this.btnSortByQuantity.Text = "Сорт. по количеству";
            this.btnSortByQuantity.UseVisualStyleBackColor = true;
            this.btnSortByQuantity.Click += new System.EventHandler(this.btnSortByQuantity_Click);
            // 
            // btnSearchProduct
            // 
            this.btnSearchProduct.Location = new System.Drawing.Point(180, 370);
            this.btnSearchProduct.Name = "btnSearchProduct";
            this.btnSearchProduct.Size = new System.Drawing.Size(120, 30);
            this.btnSearchProduct.TabIndex = 3;
            this.btnSearchProduct.Text = "Поиск товара";
            this.btnSearchProduct.UseVisualStyleBackColor = true;
            this.btnSearchProduct.Click += new System.EventHandler(this.btnSearchProduct_Click);
            // 
            // txtSearchProduct
            // 
            this.txtSearchProduct.Location = new System.Drawing.Point(320, 375);
            this.txtSearchProduct.Name = "txtSearchProduct";
            this.txtSearchProduct.Size = new System.Drawing.Size(200, 23);
            this.txtSearchProduct.TabIndex = 4;
            // 
            // txtMaxQuantity
            // 
            this.txtMaxQuantity.Location = new System.Drawing.Point(120, 420);
            this.txtMaxQuantity.Name = "txtMaxQuantity";
            this.txtMaxQuantity.ReadOnly = true;
            this.txtMaxQuantity.Size = new System.Drawing.Size(100, 23);
            this.txtMaxQuantity.TabIndex = 5;
            // 
            // txtMinQuantity
            // 
            this.txtMinQuantity.Location = new System.Drawing.Point(120, 450);
            this.txtMinQuantity.Name = "txtMinQuantity";
            this.txtMinQuantity.ReadOnly = true;
            this.txtMinQuantity.Size = new System.Drawing.Size(100, 23);
            this.txtMinQuantity.TabIndex = 6;
            // 
            // txtAvgQuantity
            // 
            this.txtAvgQuantity.Location = new System.Drawing.Point(120, 480);
            this.txtAvgQuantity.Name = "txtAvgQuantity";
            this.txtAvgQuantity.ReadOnly = true;
            this.txtAvgQuantity.Size = new System.Drawing.Size(100, 23);
            this.txtAvgQuantity.TabIndex = 7;
            // 
            // lblMaxQuantity
            // 
            this.lblMaxQuantity.AutoSize = true;
            this.lblMaxQuantity.Location = new System.Drawing.Point(12, 423);
            this.lblMaxQuantity.Name = "lblMaxQuantity";
            this.lblMaxQuantity.Size = new System.Drawing.Size(102, 15);
            this.lblMaxQuantity.TabIndex = 8;
            this.lblMaxQuantity.Text = "Максимальное:";
            // 
            // lblMinQuantity
            // 
            this.lblMinQuantity.AutoSize = true;
            this.lblMinQuantity.Location = new System.Drawing.Point(12, 453);
            this.lblMinQuantity.Name = "lblMinQuantity";
            this.lblMinQuantity.Size = new System.Drawing.Size(95, 15);
            this.lblMinQuantity.TabIndex = 9;
            this.lblMinQuantity.Text = "Минимальное:";
            // 
            // lblAvgQuantity
            // 
            this.lblAvgQuantity.AutoSize = true;
            this.lblAvgQuantity.Location = new System.Drawing.Point(12, 483);
            this.lblAvgQuantity.Name = "lblAvgQuantity";
            this.lblAvgQuantity.Size = new System.Drawing.Size(58, 15);
            this.lblAvgQuantity.TabIndex = 10;
            this.lblAvgQuantity.Text = "Среднее:";
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(250, 440);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(100, 30);
            this.btnCalculate.TabIndex = 11;
            this.btnCalculate.Text = "Вычислить";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // SaleDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 520);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.lblAvgQuantity);
            this.Controls.Add(this.lblMinQuantity);
            this.Controls.Add(this.lblMaxQuantity);
            this.Controls.Add(this.txtAvgQuantity);
            this.Controls.Add(this.txtMinQuantity);
            this.Controls.Add(this.txtMaxQuantity);
            this.Controls.Add(this.txtSearchProduct);
            this.Controls.Add(this.btnSearchProduct);
            this.Controls.Add(this.btnSortByQuantity);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblTitle);
            this.Name = "SaleDetailsForm";
            this.Text = "Детали продаж";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private DataGridView dataGridView1;
        private Button btnSortByQuantity;
        private Button btnSearchProduct;
        private TextBox txtSearchProduct;
        private TextBox txtMaxQuantity;
        private TextBox txtMinQuantity;
        private TextBox txtAvgQuantity;
        private Label lblMaxQuantity;
        private Label lblMinQuantity;
        private Label lblAvgQuantity;
        private Button btnCalculate;
    }
}