namespace Gallery.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_AttemptTable_Migration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserRoles", newName: "UsersRoles");
            MoveTable(name: "dbo.Media", newSchema: "Gallery");
            MoveTable(name: "dbo.MediaTypes", newSchema: "Gallery");
            MoveTable(name: "dbo.Users", newSchema: "Gallery");
            MoveTable(name: "dbo.Roles", newSchema: "Gallery");
            MoveTable(name: "dbo.UsersRoles", newSchema: "Gallery");
            RenameColumn(table: "Gallery.Media", name: "Id", newName: "MediaId");
            RenameColumn(table: "Gallery.MediaTypes", name: "Id", newName: "MediaTypeId");
            RenameColumn(table: "Gallery.Users", name: "Id", newName: "UserId");
            RenameColumn(table: "Gallery.Roles", name: "Id", newName: "RoleId");
            RenameColumn(table: "Gallery.UsersRoles", name: "User_Id", newName: "UserId");
            RenameColumn(table: "Gallery.UsersRoles", name: "Role_Id", newName: "RoleId");
            RenameIndex(table: "Gallery.UsersRoles", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "Gallery.UsersRoles", name: "IX_Role_Id", newName: "IX_RoleId");
            CreateTable(
                "Gallery.LoginAttempts",
                c => new
                    {
                        AttemptId = c.Int(nullable: false, identity: true),
                        IpAddress = c.String(nullable: false, maxLength: 30, unicode: false),
                        TimeStamp = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsSuccess = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AttemptId)
                .ForeignKey("Gallery.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AlterColumn("Gallery.Media", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("Gallery.MediaTypes", "Type", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("Gallery.Users", "Email", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("Gallery.Users", "Password", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("Gallery.Roles", "Name", c => c.String(nullable: false, maxLength: 30, unicode: false));
            CreateIndex("Gallery.Users", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Gallery.LoginAttempts", "UserId", "Gallery.Users");
            DropIndex("Gallery.Users", new[] { "Email" });
            DropIndex("Gallery.LoginAttempts", new[] { "UserId" });
            AlterColumn("Gallery.Roles", "Name", c => c.String());
            AlterColumn("Gallery.Users", "Password", c => c.String(nullable: false));
            AlterColumn("Gallery.Users", "Email", c => c.String(nullable: false));
            AlterColumn("Gallery.MediaTypes", "Type", c => c.String());
            AlterColumn("Gallery.Media", "Name", c => c.String());
            DropTable("Gallery.LoginAttempts");
            RenameIndex(table: "Gallery.UsersRoles", name: "IX_RoleId", newName: "IX_Role_Id");
            RenameIndex(table: "Gallery.UsersRoles", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "Gallery.UsersRoles", name: "RoleId", newName: "Role_Id");
            RenameColumn(table: "Gallery.UsersRoles", name: "UserId", newName: "User_Id");
            RenameColumn(table: "Gallery.Roles", name: "RoleId", newName: "Id");
            RenameColumn(table: "Gallery.Users", name: "UserId", newName: "Id");
            RenameColumn(table: "Gallery.MediaTypes", name: "MediaTypeId", newName: "Id");
            RenameColumn(table: "Gallery.Media", name: "MediaId", newName: "Id");
            MoveTable(name: "Gallery.UsersRoles", newSchema: "dbo");
            MoveTable(name: "Gallery.Roles", newSchema: "dbo");
            MoveTable(name: "Gallery.Users", newSchema: "dbo");
            MoveTable(name: "Gallery.MediaTypes", newSchema: "dbo");
            MoveTable(name: "Gallery.Media", newSchema: "dbo");
            RenameTable(name: "dbo.UsersRoles", newName: "UserRoles");
        }
    }
}
