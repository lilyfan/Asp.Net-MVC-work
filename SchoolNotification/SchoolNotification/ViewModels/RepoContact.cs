using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolNotification.ViewModels;
using SchoolNotification.Models;
using AutoMapper;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Web.Security;

namespace SchoolNotification.ViewModels
{
    public class RepoContact : RepositoryBase
    {
        //Create method
        public ContactFull CreateContact(ContactCreate newItem, int? stdId)
        {
            System.Diagnostics.Debug.WriteLine("CreateContact stdId: " + stdId);
            Models.Contact contact = Mapper.Map<Models.Contact>(newItem);
            contact.Student = dc.Students.FirstOrDefault(n => n.UserId == stdId);
            dc.Contacts.Add(contact);
            var student = dc.Students.Include("Contacts").FirstOrDefault(n => n.UserId == stdId);
            student.Contacts.Add(contact);
            dc.SaveChanges();
            return Mapper.Map<ContactFull>(contact);
        }

        //Edit method
        public ContactFull EditContact(ContactFull editItem)
        {
            var contactToEdit = dc.Contacts.Find(editItem.Id);
            if (contactToEdit == null) return null;
            else
            {
                dc.Entry(contactToEdit).CurrentValues.SetValues(editItem);
                dc.SaveChanges();
            }
            return Mapper.Map<ContactFull>(contactToEdit);
        }
        //Delete method
        public void DeleteContact(int? id)
        {
            var contactToDel = dc.Contacts.Find(id);
            if (contactToDel == null) return;
            else
            {
                try
                {
                    dc.Contacts.Remove(contactToDel);
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        //Get single ViewModel method
        public ContactFull getContactFull(int? id)
        {
            var contact = dc.Contacts.FirstOrDefault(n => n.Id == id);
            if (contact == null) return null;
            try
            {
                var temp = Mapper.Map<ContactFull>(contact);
                return temp;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return null;
            }

        }

        //Get list of ViewModels method
        public IEnumerable<ContactBase> getListOfContactBase()
        {
            var contacts = dc.Contacts.OrderBy(n => n.Id);
            if (contacts == null) return null;
            try
            {
                return Mapper.Map<IEnumerable<ContactBase>>(contacts);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return null;
            }
        }

        //Get list of ViewModels method for given cancellation
        public IEnumerable<ContactBase> getListOfContactForStudent(int? stdId)
        {
            System.Diagnostics.Debug.WriteLine("RepoContact stdId: "+stdId);
            //List<Models.Contact> contacts = new List<Models.Contact>();
            var contacts = dc.Contacts.Include("Student").Where(n => n.Student.UserId == stdId);

            return Mapper.Map<IEnumerable<ContactBase>>(contacts);
        }

        public SelectList getContactSelectListForStudent(int? stdId)
        {
            var courselist = getListOfContactForStudent(stdId);
            if (courselist == null)
            {
                System.Diagnostics.Debug.WriteLine("Get contactlist null value");
                return null;
            }
            else
            {
                SelectList sl = new SelectList(courselist, "Id", "ContactInfo");
                return sl;
            }
        }
    }
}