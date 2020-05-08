namespace Gallery.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EMail = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserId = c.Int(),
                        MediaTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MediaTypes", t => t.MediaTypeId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.MediaTypeId);
            
            CreateTable(
                "dbo.MediaTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Role_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Media", "UserId", "dbo.Users");
            DropForeignKey("dbo.Media", "MediaTypeId", "dbo.MediaTypes");
            DropIndex("dbo.UserRoles", new[] { "Role_Id" });
            DropIndex("dbo.UserRoles", new[] { "User_Id" });
            DropIndex("dbo.Media", new[] { "MediaTypeId" });
            DropIndex("dbo.Media", new[] { "UserId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.MediaTypes");
            DropTable("dbo.Media");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
        }
    }
}
