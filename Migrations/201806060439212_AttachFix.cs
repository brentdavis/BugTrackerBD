namespace BugTrackerBD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttachFix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TicketAttachments", "Updated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketAttachments", "Updated", c => c.DateTimeOffset(precision: 7));
        }
    }
}
