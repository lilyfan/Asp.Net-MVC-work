using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolNotification.ViewModels;
using AutoMapper;
using WebMatrix.WebData;
using System.Web.Security;
using System.Data.Entity.Validation;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class RepoAdmin : RepositoryBase
    {
        public AdminFull CreateAdmin(string fn, string ln, string eId)
        {
            var membership = (SimpleMembershipProvider)Membership.Provider;
            var roles = (SimpleRoleProvider)Roles.Provider;
            var adminUserName = fn;
            //Admin Admin= new Admin(fn, ln, fId);
            if (membership.GetUser(adminUserName, false) == null)
            {
                membership.CreateUserAndAccount(adminUserName, "123456", new Dictionary<string, object> { { "FirstName", fn }, { "LastName", ln }, { "EmployeeId", eId }, { "Discriminator", "Admin" } });
            }
            if (!roles.GetRolesForUser(adminUserName).Contains("Admin")) { roles.AddUsersToRoles(new[] { adminUserName }, new[] { "Admin" }); }
            var Admin = dc.Admins.FirstOrDefault(n => n.EmployeeId == eId);

            try
            {
                dc.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult result in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("{0} is Valid? {1}", result.Entry.Entity.ToString(), result.IsValid);

                    foreach (DbValidationError err in result.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("{0}: {1}", err.PropertyName, err.ErrorMessage);
                    }
                }
            }
            return Mapper.Map<AdminFull>(Admin);
        }
        public AdminFull EditAdmin(AdminFull editItem)
        {
            var AdminToEdit = dc.Admins.Find(editItem.UserId);
            if (AdminToEdit == null)
            {
                return null;
            }
            else
            {
                dc.Entry(AdminToEdit).CurrentValues.SetValues(editItem);
                dc.SaveChanges();
            }
            return Mapper.Map<AdminFull>(AdminToEdit);
        }
        public void DeleteAdmin(int? id)
        {
            System.Diagnostics.Debug.WriteLine("RepoAdmin delete Admin admin.Id: " + id);

            var itemToDelete = dc.Admins.Find(id);

            if (itemToDelete == null)
            {
                return;
            }
            else
            {
                try
                {
                    dc.Admins.Remove(itemToDelete);
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        //Get a ViewModel or list of ViewModels
        public IEnumerable<AdminBase> getListOfAdminBase()
        {
            var admins = dc.Admins.Where(a => a.EmployeeId != null);
            //System.Diagnostics.Debug.WriteLine("course size: " + admins.Count());
            if (admins == null) return null;

            try
            {
                var temp = Mapper.Map<IEnumerable<AdminBase>>(admins);
                return temp;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Automapper exception: " + e.StackTrace);
                return null;
            }

        }
        public AdminFull getAdminFull(int? id)
        {
            var Admin = dc.Admins.SingleOrDefault(n => n.UserId == id);
            if (Admin == null) return null;

            try
            {
                var temp = Mapper.Map<AdminFull>(Admin);
                return temp;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Automapper exception: " + e.StackTrace);
                return null;
            }
        }
    }
}