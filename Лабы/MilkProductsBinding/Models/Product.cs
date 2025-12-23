using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace MilkProductsBinding.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            SaleDetails = new HashSet<SaleDetails>();
        }

        [Key]
        [Column("ProductId")]
        public int idProduct { get; set; }

        [Required]
        [StringLength(100)]
        [Column("ProductName")]
        public string nameProduct { get; set; } = null!;

        [Column("Price", TypeName = "decimal(10, 2)")]
        public decimal priceProduct { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = null!;

        public int ExpiryDays { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public virtual ICollection<SaleDetails> SaleDetails { get; set; }

        // ВЫЧИСЛЯЕМЫЕ СВОЙСТВА для ЛР-14 - ИЗОБРАЖЕНИЯ (ПРОСТАЯ ВЕРСИЯ)

        /// <summary>
        /// Вычисляемое свойство pathFoto: Полный путь к изображению продукта
        /// ПРОСТАЯ ЛОГИКА: ищем изображение по ID продукта
        /// </summary>
        [NotMapped]
        public string pathFoto
        {
            get
            {
                try
                {
                    string basePath = Directory.GetCurrentDirectory() + @"\Images\";
                    
                    // Список возможных расширений
                    string[] extensions = { ".png", ".jpg", ".jpeg" };
                    
                    // Ищем файл по ID продукта
                    foreach (string ext in extensions)
                    {
                        string filePath = basePath + idProduct + ext;
                        if (File.Exists(filePath))
                            return filePath;
                    }
                    
                    // Если не найдено - возвращаем заглушку
                    return basePath + "picture.jpg";
                }
                catch
                {
                    // В случае любой ошибки возвращаем заглушку
                    return Directory.GetCurrentDirectory() + @"\Images\picture.jpg";
                }
            }
        }

        /// <summary>
        /// Вычисляемое свойство: Есть ли изображение у продукта
        /// </summary>
        [NotMapped]
        public bool HasImage
        {
            get
            {
                try
                {
                    return File.Exists(pathFoto) && !pathFoto.EndsWith("picture.jpg");
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Вычисляемое свойство: Статус изображения
        /// </summary>
        [NotMapped]
        public string ImageStatus
        {
            get
            {
                return HasImage ? "✓ Есть изображение" : "✗ Заглушка";
            }
        }
    }
}