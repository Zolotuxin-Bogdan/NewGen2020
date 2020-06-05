namespace Gallery.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Media_Name_Lenght_Changed_Migration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Gallery.Media", "Name", c => c.String(nullable: false, maxLength: 255, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("Gallery.Media", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
