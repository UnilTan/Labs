namespace MilkProductsCatalog
{
    partial class SaleForm
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
            this.btnSortByDate = new System.Windows.Forms.Button();
            this.btnSortByAmount = new System.Windows.Forms.Button();
            this.btnSearchCustomer = new System.Windows.Forms.Button();
            this.txtSearchCustomer = new System.Windows.Forms.TextBox();
            this.txtMaxAmount = new System.Windows.Forms.TextBox();
            this.txtMinAmount = new System.Windows.Forms.TextBox();
            this.txtAvgAmount = new System.Windows.Forms.TextBox();
            this.lblMaxAmount = new System.Windows.Forms.Label();
            this.lblMinAmount = new System.Windows.Forms.Label();
            this.lblAvgAmount = new System.Windows.Forms.Label();
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
            this.lblTitle.Size = new System.Drawing.Size(104, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Продажи";
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
            // btnSortByDate
            // 
            this.btnSortByDate.Location = new System.Drawing.Point(12, 370);
            this.btnSortByDate.Name = "btnSortByDate";
            this.btnSortByDate.Size = new System.Drawing.Size(120, 30);
            this.btnSortByDate.TabIndex = 2;
            this.btnSortByDate.Text = "Сорт. по дате";
            this.btnSortByDate.UseVisualStyleBackColor = true;
            this.btnSortByDate.Click += new System.EventHandler(this.btnSortByDate_Click);
            // 
            // btnSortByAmount
            // 
            this.btnSortByAmount.Location = new System.Drawing.Point(150, 370);
            this.btnSortByAmount.Name = "btnSortByAmount";
            this.btnSortByAmount.Size = new System.Drawing.Size(120, 30);
            this.btnSortByAmount.TabIndex = 3;
            this.btnSortByAmount.Text = "Сорт. по сумме";
            this.btnSortByAmount.UseVisualStyleBackColor = true;
            this.btnSortByAmount.Click += new System.EventHandler(this.btnSortByAmount_Click);
            // 
            // btnSearchCustomer
            // 
            this.btnSearchCustomer.Location = new System.Drawing.Point(290, 370);
            this.btnSearchCustomer.Name = "btnSearchCustomer";
            this.btnSearchCustomer.Size = new System.Drawing.Size(120, 30);
            this.btnSearchCustomer.TabIndex = 4;
            this.btnSearchCustomer.Text = "Поиск клиента";
            this.btnSearchCustomer.UseVisualStyleBackColor = true;
            this.btnSearchCustomer.Click += new System.EventHandler(this.btnSearchCustomer_Click);
            // 
            // txtSearchCustomer
            // 
            this.txtSearchCustomer.Location = new System.Drawing.Point(430, 375);
            this.txtSearchCustomer.Name = "txtSearchCustomer";
            this.txtSearchCustomer.Size = new System.Drawing.Size(200, 23);
            this.txtSearchCustomer.TabIndex = 5;
            // 
            // txtMaxAmount
            // 
            this.txtMaxAmount.Location = new System.Drawing.Point(120, 420);
            this.txtMaxAmount.Name = "txtMaxAmount";
            this.txtMaxAmount.ReadOnly = true;
            this.txtMaxAmount.Size = new System.Drawing.Size(100, 23);
            this.txtMaxAmount.TabIndex = 6;
            // 
            // txtMinAmount
            // 
            this.txtMinAmount.Location = new System.Drawing.Point(120, 450);
            this.txtMinAmount.Name = "txtMinAmount";
            this.txtMinAmount.ReadOnly = true;
            this.txtMinAmount.Size = new System.Drawing.Size(100, 23);
            this.txtMinAmount.TabIndex = 7;
            // 
            // txtAvgAmount
            // 
            this.txtAvgAmount.Location = new System.Drawing.Point(120, 480);
            this.txtAvgAmount.Name = "txtAvgAmount";
            this.txtAvgAmount.ReadOnly = true;
            this.txtAvgAmount.Size = new System.Drawing.Size(100, 23);
            this.txtAvgAmount.TabIndex = 8;
            // 
            // lblMaxAmount
            // 
            this.lblMaxAmount.AutoSize = true;
            this.lblMaxAmount.Location = new System.Drawing.Point(12, 423);
            this.lblMaxAmount.Name = "lblMaxAmount";
            this.lblMaxAmount.Size = new System.Drawing.Size(102, 15);
            this.lblMaxAmount.TabIndex = 9;
            this.lblMaxAmount.Text = "Максимальная:";
            // 
            // lblMinAmount
            // 
            this.lblMinAmount.AutoSize = true;
            this.lblMinAmount.Location = new System.Drawing.Point(12, 453);
            this.lblMinAmount.Name = "lblMinAmount";
            this.lblMinAmount.Size = new System.Drawing.Size(95, 15);
            this.lblMinAmount.TabIndex = 10;
            this.lblMinAmount.Text = "Минимальная:";
            // 
            // lblAvgAmount
            // 
            this.lblAvgAmount.AutoSize = true;
            this.lblAvgAmount.Location = new System.Drawing.Point(12, 483);
            this.lblAvgAmount.Name = "lblAvgAmount";
            this.lblAvgAmount.Size = new System.Drawing.Size(58, 15);
            this.lblAvgAmount.TabIndex = 11;
            this.lblAvgAmount.Text = "Средняя:";
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(250, 440);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(100, 30);
            this.btnCalculate.TabIndex = 12;
            this.btnCalculate.Text = "Вычислить";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // SaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 520);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.lblAvgAmount);
            this.Controls.Add(this.lblMinAmount);
            this.Controls.Add(this.lblMaxAmount);
            this.Controls.Add(this.txtAvgAmount);
            this.Controls.Add(this.txtMinAmount);
            this.Controls.Add(this.txtMaxAmount);
            this.Controls.Add(this.txtSearchCustomer);
            this.Controls.Add(this.btnSearchCustomer);
            this.Controls.Add(this.btnSortByAmount);
            this.Controls.Add(this.btnSortByDate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblTitle);
            this.Name = "SaleForm";
            this.Text = "Продажи";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private DataGridView dataGridView1;
        private Button btnSortByDate;
        private Button btnSortByAmount;
        private Button btnSearchCustomer;
        private TextBox txtSearchCustomer;
        private TextBox txtMaxAmount;
        private TextBox txtMinAmount;
        private TextBox txtAvgAmount;
        private Label lblMaxAmount;
        private Label lblMinAmount;
        private Label lblAvgAmount;
        private Button btnCalculate;
    }
}