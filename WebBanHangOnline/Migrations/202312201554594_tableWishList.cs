namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tableWishList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Wishlist",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        UserName = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Wishlist", "ProductID", "dbo.tb_Product");
            DropIndex("dbo.tb_Wishlist", new[] { "ProductID" });
            DropTable("dbo.tb_Wishlist");
        }
    }
}
