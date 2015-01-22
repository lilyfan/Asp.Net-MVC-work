namespace INT422Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        FacultyId = c.String(),
                        StudentId = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false),
                        CourseCode = c.String(nullable: false),
                        Schedule = c.String(),
                        Room = c.String(),
                        Faculty_Id = c.String(maxLength: 128),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Faculty_Id)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .Index(t => t.Faculty_Id)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContactInfo = c.String(),
                        StudentId = c.String(maxLength: 128),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId)
                .Index(t => t.User_UserId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false),
                        Date = c.String(nullable: false),
                        Time = c.String(nullable: false),
                        FacultyName = c.String(nullable: false),
                        MsgContent = c.String(),
                        CancellationId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                        Faculty_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.Faculty_Id)
                .Index(t => t.User_UserId)
                .Index(t => t.Faculty_Id);
            
            CreateTable(
                "dbo.Cancellations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        Course_Id = c.Int(),
                        Message_Id = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .Index(t => t.Course_Id)
                .Index(t => t.Message_Id)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        Student_Id = c.String(nullable: false, maxLength: 128),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_Id, t.Course_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.Course_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cancellations", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Cancellations", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.Cancellations", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Faculty_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Courses", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.StudentCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.StudentCourses", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Contacts", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Contacts", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Courses", "Faculty_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Cancellations", new[] { "User_UserId" });
            DropIndex("dbo.Cancellations", new[] { "Message_Id" });
            DropIndex("dbo.Cancellations", new[] { "Course_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "Faculty_Id" });
            DropIndex("dbo.Messages", new[] { "User_UserId" });
            DropIndex("dbo.Courses", new[] { "User_UserId" });
            DropIndex("dbo.StudentCourses", new[] { "Course_Id" });
            DropIndex("dbo.StudentCourses", new[] { "Student_Id" });
            DropIndex("dbo.Contacts", new[] { "StudentId" });
            DropIndex("dbo.Contacts", new[] { "User_UserId" });
            DropIndex("dbo.Courses", new[] { "Faculty_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropTable("dbo.StudentCourses");
            DropTable("dbo.Cancellations");
            DropTable("dbo.Messages");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Contacts");
            DropTable("dbo.Courses");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
        }
    }
}
