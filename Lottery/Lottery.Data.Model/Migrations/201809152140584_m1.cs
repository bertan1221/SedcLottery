namespace Lottery.Data.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Awards",
                c => new
                    {
                        AwardID = c.Int(nullable: false, identity: true),
                        AwardName = c.String(),
                        AwardDescription = c.String(),
                        Quantity = c.Int(nullable: false),
                        RuffledType = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.AwardID);
            
            CreateTable(
                "dbo.Codes",
                c => new
                    {
                        CodeID = c.Int(nullable: false, identity: true),
                        CodeValue = c.String(),
                        IsWinning = c.Boolean(nullable: false),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CodeID);
            
            CreateTable(
                "dbo.UserCodeAwards",
                c => new
                    {
                        UserCodeAwardID = c.Int(nullable: false, identity: true),
                        UserCodeID = c.Int(nullable: false),
                        AwardID = c.Int(nullable: false),
                        WonAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserCodeAwardID)
                .ForeignKey("dbo.Awards", t => t.AwardID, cascadeDelete: true)
                .ForeignKey("dbo.UserCodes", t => t.UserCodeID, cascadeDelete: true)
                .Index(t => t.UserCodeID)
                .Index(t => t.AwardID);
            
            CreateTable(
                "dbo.UserCodes",
                c => new
                    {
                        UserCodeID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        CodeID = c.Int(nullable: false),
                        SentAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserCodeID)
                .ForeignKey("dbo.Codes", t => t.CodeID, cascadeDelete: true)
                .Index(t => t.CodeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCodeAwards", "UserCodeID", "dbo.UserCodes");
            DropForeignKey("dbo.UserCodes", "CodeID", "dbo.Codes");
            DropForeignKey("dbo.UserCodeAwards", "AwardID", "dbo.Awards");
            DropIndex("dbo.UserCodes", new[] { "CodeID" });
            DropIndex("dbo.UserCodeAwards", new[] { "AwardID" });
            DropIndex("dbo.UserCodeAwards", new[] { "UserCodeID" });
            DropTable("dbo.UserCodes");
            DropTable("dbo.UserCodeAwards");
            DropTable("dbo.Codes");
            DropTable("dbo.Awards");
        }
    }
}
