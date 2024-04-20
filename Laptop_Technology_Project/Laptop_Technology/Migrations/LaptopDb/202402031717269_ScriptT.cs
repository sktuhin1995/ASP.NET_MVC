namespace Laptop_Technology.Migrations.LaptopDb
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScriptT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandId = c.Int(nullable: false, identity: true),
                        BrandName = c.String(),
                    })
                .PrimaryKey(t => t.BrandId);
            
            CreateTable(
                "dbo.Laptops",
                c => new
                    {
                        LaptopId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        FirstIntroduceOn = c.DateTime(nullable: false, storeType: "date"),
                        OnSale = c.Boolean(nullable: false),
                        Picture = c.String(),
                        LaptopModelId = c.Int(nullable: false),
                        BrandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LaptopId)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.LaptopModels", t => t.LaptopModelId, cascadeDelete: true)
                .Index(t => t.LaptopModelId)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.LaptopModels",
                c => new
                    {
                        LaptopModelId = c.Int(nullable: false, identity: true),
                        ModelName = c.String(),
                    })
                .PrimaryKey(t => t.LaptopModelId);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockId = c.Int(nullable: false, identity: true),
                        Size = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        LaptopId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockId)
                .ForeignKey("dbo.Laptops", t => t.LaptopId, cascadeDelete: true)
                .Index(t => t.LaptopId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "LaptopId", "dbo.Laptops");
            DropForeignKey("dbo.Laptops", "LaptopModelId", "dbo.LaptopModels");
            DropForeignKey("dbo.Laptops", "BrandId", "dbo.Brands");
            DropIndex("dbo.Stocks", new[] { "LaptopId" });
            DropIndex("dbo.Laptops", new[] { "BrandId" });
            DropIndex("dbo.Laptops", new[] { "LaptopModelId" });
            DropTable("dbo.Stocks");
            DropTable("dbo.LaptopModels");
            DropTable("dbo.Laptops");
            DropTable("dbo.Brands");
        }
    }
}
