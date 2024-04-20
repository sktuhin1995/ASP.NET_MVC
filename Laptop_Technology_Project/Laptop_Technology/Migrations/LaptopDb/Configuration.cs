namespace Laptop_Technology.Migrations.LaptopDb
{
    using Laptop_Technology.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Laptop_Technology.Models.LaptopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\LaptopDb";
        }

        protected override void Seed(Laptop_Technology.Models.LaptopDbContext db)
        {
            db.Brands.AddRange(new Brand[]
            {
                new Brand{ BrandName = "HP"},
                new Brand{ BrandName = "ASUS"},
                new Brand{ BrandName = "Lenovo"},
                new Brand{ BrandName = "Dell"},
                new Brand{ BrandName = "Acer"}
            });
            db.LaptopModels.AddRange(new LaptopModel[]
            {
                new LaptopModel{ ModelName = "ProBook"},
                new LaptopModel{ ModelName = "Vivobook"},
                new LaptopModel{ ModelName = "IdeaPad"},
                new LaptopModel{ ModelName = "Latitude"},
                new LaptopModel{ ModelName = "Aspire"}
            });
            db.SaveChanges();
            Laptop laptop = new Laptop()
            {
                Name = "HP ProBook 455 G9",
                BrandId = 1,
                LaptopModelId = 1,
                FirstIntroduceOn = new DateTime(2021, 3, 5),
                OnSale = true,
                Picture = "probook-455-g9-01-500x500"
            };
            laptop.Stocks.Add(new Stock { Size = Size.S13, Quantity = 5, Price = 4500 });
            laptop.Stocks.Add(new Stock { Size = Size.S14, Quantity = 5, Price = 5000 });
            laptop.Stocks.Add(new Stock { Size = Size.S15, Quantity = 10, Price = 5500 });
            db.Laptops.Add(laptop);
            db.SaveChanges();
        }
    }
}
