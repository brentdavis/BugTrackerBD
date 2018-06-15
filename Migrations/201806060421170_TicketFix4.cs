namespace BugTrackerBD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketFix4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tickets", "Projects_Id", "dbo.Projects");
            DropIndex("dbo.Tickets", new[] { "Projects_Id" });
            RenameColumn(table: "dbo.Tickets", name: "Projects_Id", newName: "ProjectsId");
            AlterColumn("dbo.Tickets", "ProjectsId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "ProjectsId");
            AddForeignKey("dbo.Tickets", "ProjectsId", "dbo.Projects", "Id", cascadeDelete: true);
            DropColumn("dbo.Tickets", "ProjectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "ProjectId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Tickets", "ProjectsId", "dbo.Projects");
            DropIndex("dbo.Tickets", new[] { "ProjectsId" });
            AlterColumn("dbo.Tickets", "ProjectsId", c => c.Int());
            RenameColumn(table: "dbo.Tickets", name: "ProjectsId", newName: "Projects_Id");
            CreateIndex("dbo.Tickets", "Projects_Id");
            AddForeignKey("dbo.Tickets", "Projects_Id", "dbo.Projects", "Id");
        }
    }
}
