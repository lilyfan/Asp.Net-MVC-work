namespace INT422Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSortOrder : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetUsers", newName: "Faculties");
            DropForeignKey("dbo.Courses", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Messages", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Cancellations", "User_UserId", "dbo.UserProfile");
            DropIndex("dbo.Courses", new[] { "User_UserId" });
            DropIndex("dbo.Messages", new[] { "User_UserId" });
            DropIndex("dbo.Cancellations", new[] { "User_UserId" });
            RenameColumn(table: "dbo.Courses", name: "User_UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Messages", name: "User_UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Cancellations", name: "User_UserId", newName: "User_Id");
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id2", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Discriminator", c => c.String(maxLength: 128));
            AlterColumn("dbo.Courses", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Messages", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Cancellations", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id1");
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id2");
            CreateIndex("dbo.Faculties", "Id");
            CreateIndex("dbo.Faculties", "ApplicationUser_Id");
            CreateIndex("dbo.Courses", "User_Id");
            CreateIndex("dbo.Messages", "User_Id");
            CreateIndex("dbo.Cancellations", "User_Id");
            AddForeignKey("dbo.AspNetUsers", "ApplicationUser_Id1", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "ApplicationUser_Id2", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.Faculties", "Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.Faculties", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.Courses", "User_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.Messages", "User_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.Cancellations", "User_Id", "dbo.ApplicationUsers", "Id");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FacultyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "FacultyId", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            DropForeignKey("dbo.Cancellations", "User_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Courses", "User_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Faculties", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Faculties", "Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUsers", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ApplicationUser_Id2", "dbo.ApplicationUsers");
            DropForeignKey("dbo.AspNetUsers", "ApplicationUser_Id1", "dbo.ApplicationUsers");
            DropIndex("dbo.Cancellations", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropIndex("dbo.Courses", new[] { "User_Id" });
            DropIndex("dbo.Faculties", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Faculties", new[] { "Id" });
            DropIndex("dbo.ApplicationUsers", new[] { "Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id2" });
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id1" });
            AlterColumn("dbo.Cancellations", "User_Id", c => c.Int());
            AlterColumn("dbo.Messages", "User_Id", c => c.Int());
            AlterColumn("dbo.Courses", "User_Id", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id2");
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id1");
            DropTable("dbo.ApplicationUsers");
            RenameColumn(table: "dbo.Cancellations", name: "User_Id", newName: "User_UserId");
            RenameColumn(table: "dbo.Messages", name: "User_Id", newName: "User_UserId");
            RenameColumn(table: "dbo.Courses", name: "User_Id", newName: "User_UserId");
            CreateIndex("dbo.Cancellations", "User_UserId");
            CreateIndex("dbo.Messages", "User_UserId");
            CreateIndex("dbo.Courses", "User_UserId");
            AddForeignKey("dbo.Cancellations", "User_UserId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.Messages", "User_UserId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.Courses", "User_UserId", "dbo.UserProfile", "UserId");
            RenameTable(name: "dbo.Faculties", newName: "AspNetUsers");
        }
    }
}
