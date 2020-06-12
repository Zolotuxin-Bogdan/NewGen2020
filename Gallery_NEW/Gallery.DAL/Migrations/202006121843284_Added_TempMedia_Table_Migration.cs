namespace Gallery.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_TempMedia_Table_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Gallery.TempMedia",
                c => new
                    {
                        TempMediaId = c.Int(nullable: false, identity: true),
                        UniqName = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsLoading = c.Boolean(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TempMediaId)
                .ForeignKey("Gallery.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Gallery.TempMedia", "UserId", "Gallery.Users");
            DropIndex("Gallery.TempMedia", new[] { "UserId" });
            DropTable("Gallery.TempMedia");
        }
    }
}
