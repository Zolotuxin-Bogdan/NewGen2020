namespace Gallery.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_IsDeleted_Property_To_Media : DbMigration
    {
        public override void Up()
        {
            AddColumn("Gallery.Media", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Gallery.Media", "IsDeleted");
        }
    }
}
