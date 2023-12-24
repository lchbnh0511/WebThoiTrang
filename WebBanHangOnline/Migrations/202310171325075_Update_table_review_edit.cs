namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_table_review_edit : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tb_Review", "ProductID");
            AddForeignKey("dbo.tb_Review", "ProductID", "dbo.tb_Product", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Review", "ProductID", "dbo.tb_Product");
            DropIndex("dbo.tb_Review", new[] { "ProductID" });
        }
    }
}
