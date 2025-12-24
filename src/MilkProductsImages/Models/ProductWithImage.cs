using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Windows.Media.Imaging;

namespace MilkProductsImages.Models
{
    [Table("ProductWithImage")]
    public partial class ProductWithImage
    {
        [Key]
        [Column("ProductId")]
        public int IdProduct { get; set; }

        [Required]
        [StringLength(100)]
        [Column("ProductName")]
        public string NameProduct { get; set; } = null!;

        [Column("Price", TypeName = "decimal(10, 2)")]
        public decimal PriceProduct { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = null!;

        public int ExpiryDays { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        // НОВОЕ ПОЛЕ для ЛР-14 Часть 2: Изображение в БД
        [Column("ProductImage", TypeName = "varbinary(max)")]
        public byte[]? ProductImage { get; set; }

        [StringLength(100)]
        public string? ImageFileName { get; set; }

        [StringLength(50)]
        public string? ImageContentType { get; set; }

        public long? ImageFileSize { get; set; }

        // ВЫЧИСЛЯЕМЫЕ СВОЙСТВА для работы с изображениями в БД

        /// <summary>
        /// Вычисляемое свойство: Есть ли изображение в БД
        /// </summary>
        [NotMapped]
        public bool HasImageInDatabase
        {
            get
            {
                return ProductImage != null && ProductImage.Length > 0;
            }
        }

        /// <summary>
        /// Вычисляемое свойство: Размер изображения в удобочитаемом формате
        /// </summary>
        [NotMapped]
        public string ImageSizeFormatted
        {
            get
            {
                if (!HasImageInDatabase) return "Нет изображения";
                
                long bytes = ProductImage!.Length;
                if (bytes < 1024) return $"{bytes} байт";
                if (bytes < 1024 * 1024) return $"{bytes / 1024:F1} КБ";
                return $"{bytes / (1024 * 1024):F1} МБ";
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
                if (HasImageInDatabase)
                    return $"✓ В БД ({ImageSizeFormatted})";
                else
                    return "✗ Нет изображения";
            }
        }

        /// <summary>
        /// Вычисляемое свойство: BitmapImage для отображения в WPF
        /// Преобразует byte[] в BitmapImage для привязки к Image.Source
        /// </summary>
        [NotMapped]
        public BitmapImage? ImageSource
        {
            get
            {
                if (!HasImageInDatabase) return null;

                try
                {
                    using (var stream = new MemoryStream(ProductImage!))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                        bitmap.Freeze(); // Важно для многопоточности
                        return bitmap;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Метод для загрузки изображения из файла в byte[]
        /// </summary>
        public void LoadImageFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            try
            {
                ProductImage = File.ReadAllBytes(filePath);
                ImageFileName = Path.GetFileName(filePath);
                ImageFileSize = ProductImage.Length;
                
                // Определяем тип контента по расширению
                string extension = Path.GetExtension(filePath).ToLower();
                ImageContentType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".bmp" => "image/bmp",
                    ".gif" => "image/gif",
                    _ => "image/unknown"
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка загрузки изображения: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Метод для сохранения изображения из БД в файл
        /// </summary>
        public void SaveImageToFile(string filePath)
        {
            if (!HasImageInDatabase)
                throw new InvalidOperationException("Нет изображения для сохранения");

            try
            {
                File.WriteAllBytes(filePath, ProductImage!);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка сохранения изображения: {ex.Message}", ex);
            }
        }
    }
}