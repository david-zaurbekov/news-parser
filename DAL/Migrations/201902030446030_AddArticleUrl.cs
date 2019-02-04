namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArticleUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Url", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Url");
        }
    }
}
