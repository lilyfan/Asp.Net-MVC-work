namespace INT422Project.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;
    using INT422Project.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<INT422Project.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "INT422Project.Models.DataContext";
        }

        protected override void Seed(INT422Project.Models.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            System.Diagnostics.Debug.WriteLine("Configuration Here! ");

            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                "UserProfile", "UserId", "UserName", autoCreateTables: true);

            //Create roles
            string[] roleNames = new string[] { "Admin", "Faculty", "Student" };
            var roles = (SimpleRoleProvider)Roles.Provider;
            for (var i = 0; i < roleNames.Length; i++)
            {
                if (!roles.RoleExists(roleNames[i])) { roles.CreateRole(roleNames[i]); }
            }

        }
    }
}
