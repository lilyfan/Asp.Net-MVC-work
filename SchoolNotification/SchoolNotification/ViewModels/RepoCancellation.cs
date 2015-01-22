using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolNotification.ViewModels;
using AutoMapper;
using System.Web.Mvc;
using System.Data.Entity;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class RepoCancellation : RepositoryBase
    {
        //Create method
        //public CancellationFull CreateCancellation( string courseId, string date, string msgId, string facId )
        public CancellationFull CreateCancellation(string courseId, string date, string time, string msg, string facId)
        {
            Models.Cancellation cancellation = new Models.Cancellation();
            //*********AutoMapper
            int cId = Convert.ToInt32(courseId);
            cancellation.Course = dc.Courses.FirstOrDefault(n => n.Id == cId);
            System.Diagnostics.Debug.WriteLine("RepoCancellation Create() cancellation.Course: "+cancellation.Course.CourseCode);
            System.Diagnostics.Debug.WriteLine("RepoCancellation Create() cancellation.Date: " + date);
            cancellation.Date = date;
            cancellation.Message = new Models.Message(cancellation.Course.CourseCode, date, time, msg);            
            cancellation.Faculty = dc.Faculties.FirstOrDefault(n => n.FacultyId== facId);
            System.Diagnostics.Debug.WriteLine("RepoCancellation Create() cancellation.Faculty: " + cancellation.Faculty.FirstName);
            dc.Cancellations.Add(cancellation);
            dc.SaveChanges();
            return Mapper.Map<CancellationFull>(cancellation);
        }
        //Edit method
        public CancellationFull EditCancellation(CancellationFull editItem)
        {
            var cancellationToEdit = dc.Cancellations.Include("Message").Include("Faculty").Include("Course").FirstOrDefault(n=>n.Id==editItem.Id);
            if (cancellationToEdit == null) return null;
            else
            {
                //dc.Entry(cancellationToEdit).CurrentValues.SetValues(Mapper.Map<Cancellation>(editItem));
                cancellationToEdit.Date = editItem.Date;
                cancellationToEdit.Course.CourseCode = editItem.Course.CourseCode;
                cancellationToEdit.Faculty.FacultyId = editItem.Faculty.FacultyId;
                cancellationToEdit.Message.Time = editItem.Message.Time;
                cancellationToEdit.Message.MsgContent = editItem.Message.MsgContent;
                dc.SaveChanges();
            }
            return Mapper.Map<CancellationFull>(cancellationToEdit);

        }
        //Delete method
        public void DeleteCancellation(int? id)
        {
            var cancellationToDel = dc.Cancellations.Find(id);
            if (cancellationToDel == null) return;
            else
            {
                try
                {
                    dc.Cancellations.Remove(cancellationToDel);
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        //Get single ViewModel
        public CancellationFull getCancellationFull(int? id)
        {
            //var cancellation = dc.Cancellations.Include("Courses").Include("Messages").FirstOrDefault(n => n.Id == id);
            var cancellation = dc.Cancellations.Include("Course").Include("Faculty").Include("Message").FirstOrDefault(n => n.Id == id);
            if (cancellation == null) return null;
            try { return Mapper.Map<CancellationFull>(cancellation); }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Get cancellation automapper expection: " + e.StackTrace);
                return null;
            }
        }
        //Get list of ViewModels
        public IEnumerable<CancellationBase> getListOfCancellationBase()
        {
            var cancellations = dc.Cancellations.Include("Course").OrderBy(n => n.Id);
            if (cancellations == null) return null;
            try
            {
                var temp = Mapper.Map<IEnumerable<CancellationBase>>(cancellations);
                return temp;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Get cancellation automapper expection: " + e.StackTrace);
                return null;
            }
            
        }

        //Get list of ViewModels
        public IEnumerable<CancellationBase> getListOfCancellationBaseForStd(int? stdId)
        {
            System.Diagnostics.Debug.WriteLine("RepoCancellation Student Id: " + stdId);
            var student = dc.Students.Include("Courses").FirstOrDefault(s => s.UserId == stdId);
            var courses = student.Courses.OrderBy(c=>c.Id);
            List<CancellationBase> canBaseList = new List<CancellationBase>();
            foreach (var i in courses)
            {
                System.Diagnostics.Debug.WriteLine("RepoCancellation course.CourseCode: " + i.CourseCode+" course.Id: "+i.Id);
                //CancellationBase canBase = new CancellationBase();                                
                try
                {
                    var can = dc.Cancellations.Include("Course").Where(n => n.Course.Id == i.Id);
                    System.Diagnostics.Debug.WriteLine("RepoCancellation can.count: " + can.Count());
                    if (can != null)
                    {
                        foreach (var j in can)
                        {
                            var canDate = Convert.ToDateTime(j.Date);
                            int result = DateTime.Compare(canDate, DateTime.Today);
                            System.Diagnostics.Debug.WriteLine("RepoCancellation getListOfCancellationBaseForStd() -> canDate: " + canDate+" result: "+result);
                            if (result >= 0)
                            {
                                canBaseList.Add(Mapper.Map<CancellationBase>(j));
                                System.Diagnostics.Debug.WriteLine("RepoCancellation can.Id: " + j.Id);
                            }
                        }
                        
 
                        //
                        //canBase.Date = can.Date;
                        //System.Diagnostics.Debug.WriteLine("RepoCancellation can.Date: " + canBase.Date);
                        //canBase.Course = can.Course;
                        //System.Diagnostics.Debug.WriteLine("RepoCancellation can.Course: " + canBase.Course.CourseCode);
                        //canBaseList.Add(canBase);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("No cancellation for course "+i.CourseCode+" "+e.StackTrace);
                }
            }
            return canBaseList;
            //var cancellations = dc.Cancellations.Include("Course").Where(n => n.Course.Id ==std.Courses.));
            //if (cancellations == null) return null;
            /*try
            {
                var temp = Mapper.Map<IEnumerable<CancellationBase>>(cancellations);
                return temp;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Get cancellation automapper expection: " + e.StackTrace);
                return null;
            }*/

        }

        //Get list of ViewModels for given faculty
        public IEnumerable<CancellationBase> getListOfCancellationForFaculty(int facId)
        {
            System.Diagnostics.Debug.WriteLine("RepoCancellation faculty Id: " + facId);
            var faculty = dc.Faculties.Include("Courses").FirstOrDefault(s => s.UserId == facId);
            var courses = faculty.Courses.OrderBy(c => c.Id);
            List<CancellationBase> canBaseList = new List<CancellationBase>();
            foreach (var i in courses)
            {
                System.Diagnostics.Debug.WriteLine("RepoCancellation course.CourseCode: " + i.CourseCode + " course.Id: " + i.Id);
                //CancellationBase canBase = new CancellationBase();                                
                try
                {
                    var can = dc.Cancellations.Include("Course").Where(n => n.Course.Id == i.Id);
                    System.Diagnostics.Debug.WriteLine("RepoCancellation can.count: " + can.Count());
                    if (can != null)
                    {
                        foreach (var j in can)
                        {
                            canBaseList.Add(Mapper.Map<CancellationBase>(j));
                            System.Diagnostics.Debug.WriteLine("RepoCancellation can.Id: " + j.Id);
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("No cancellation for course " + i.CourseCode + " " + e.StackTrace);
                }
            }
            return canBaseList;
        }

        //Get select list method
        public SelectList getCancellationSelectList()
        {
            var canlist = getListOfCancellationBase();
            if (canlist == null)
            {
                System.Diagnostics.Debug.WriteLine("Get cancellation list null value");
                return null;
            }
            else
            {
                SelectList sl = new SelectList(canlist, "Id", "Date");
                return sl;
            }
        }

        //Get select list for dates method
        public SelectList getDatesSelectList()
        {
            var lsDates = new List<string>();
            lsDates.Add("Select Date");
            DateTime startDt = DateTime.Today;
            DateTime endDt = new DateTime(2014, 4, 18);
            for (var dt = startDt; dt <= endDt; dt = dt.AddDays(1))
            {
                //System.Diagnostics.Debug.WriteLine("Date is: " + dt.ToShortDateString());
                lsDates.Add(dt.ToShortDateString());
            }
            SelectList sl = new SelectList(lsDates);
            return sl;
        }

        public IEnumerable<CancellationBase> searchListOfCancellationBase(string searchStr)
        {
            var cancellations = dc.Cancellations.Include("Course").Where(n=>n.Course.CourseCode.Contains(searchStr));
            if (cancellations == null) return null;
            try
            {
                var temp = Mapper.Map<IEnumerable<CancellationBase>>(cancellations);
                return temp;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Get cancellation automapper expection: " + e.StackTrace);
                return null;
            }
        }
    }
}