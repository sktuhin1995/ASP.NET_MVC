using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Laptop_Technology.Models
{
    public enum Size { S13=13, S14, S15}
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();

    }
    public class LaptopModel
    {
        public int LaptopModelId { get; set; }
        public string ModelName { get; set; }
        public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();

    }

    public class Laptop
    {
        public int LaptopId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "First Introduce"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FirstIntroduceOn { get; set; }
        public bool OnSale { get; set; }
        public string Picture { get; set; }
        //fk
        [ForeignKey("LaptopModel")]
        public int LaptopModelId { get; set; }
        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        //nev
        public virtual LaptopModel LaptopModel { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    }

    public class Stock
    {
        public int StockId { get; set; }
        public Size Size { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Required, ForeignKey("Laptop")]
        public int LaptopId { get; set; }
        // nev
        public virtual Laptop Laptop { get; set; }
    }

    public class LaptopDbContext : DbContext
    {

        public DbSet<Brand> Brands { get; set; }
        public DbSet<LaptopModel> LaptopModels { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}