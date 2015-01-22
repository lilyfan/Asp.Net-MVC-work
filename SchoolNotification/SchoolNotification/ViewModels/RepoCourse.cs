using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolNotification.ViewModels;
using AutoMapper;
using System.Web.Mvc;
using System.Data.Entity;

namespace SchoolNotification.ViewModels
{
    public class RepoCourse : RepositoryBase
    {
        //Create method
        public CourseFull CreateCourse(CourseCreate newItem, string facId)
        {
            //Models.Course course = Mapper.Map<Models.Course>(newItem);
            newItem.Faculty = dc.Faculties.FirstOrDefault(f => f.FacultyId == facId);
            dc.Courses.Add(Mapper.Map<Models.Course>(newItem));
            dc.SaveChanges();
            return Mapper.Map<CourseFull>(newItem);
        }
        //Edit method
        public CourseFull EditCourse(CourseFull editItem)
        {
            var courseToEdit = dc.Courses.Find(editItem.Id);
            if (courseToEdit == null) return null;
            else
            {
                dc.Entry(courseToEdit).CurrentValues.SetValues(editItem);
                courseToEdit.Faculty.FacultyId = editItem.Faculty.FacultyId;
                dc.SaveChanges();
            }
            return Mapper.Map<CourseFull>(courseToEdit);
        }
        //Delete method
        public void DeleteCourse(int? id)
        {
            var itemToDelete = dc.Courses.Include("Faculty").Include("Students").FirstOrDefault(c=>c.Id == id);
            if (itemToDelete == null) return;
            else
            {
                try
                {
                    dc.Courses.Remove(itemToDelete);
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        //Get single ViewModel method
        public CourseFull getCourseFull(int? id)
        {
            var course = dc.Courses.Include("Faculty").FirstOrDefault(n => n.Id == id);
            if (course == null) { return null; }
            try { return Mapper.Map<CourseFull>(course); }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return null;
            }
        }
        //Get List of ViewModels method
        public IEnumerable<CourseBase> getListOfCourseBase()
        {
            var courses = dc.Courses.OrderBy(c => c.Id);
            try
            {
                var temp =Mapper.Map<IEnumerable<CourseBase>>(courses);
                return temp;
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine("Automapper exception: "+e.StackTrace);
                return null;
            }

        }
        public IEnumerable<CourseBase> getListOfCourseForStudent(int? stdId)
        {
            System.Diagnostics.Debug.WriteLine("RepoCourses Student Id: " + stdId);
            var student = dc.Students.Include("Courses").FirstOrDefault(s => s.UserId == stdId);

            return Mapper.Map<IEnumerable<CourseBase>>(student.Courses);
        }

        public List<CourseBase> getListOfCourseForFaculty(int? facId)
        {
            System.Diagnostics.Debug.WriteLine("RepoCourses faculty Id: " + facId);
            var faculty = dc.Faculties.Include("Courses").FirstOrDefault(s => s.UserId == facId);
            if (faculty == null)
            {
                System.Diagnostics.Debug.WriteLine("RepoCourse faculty is null" );
                return null;
            }
            var courses = faculty.Courses.OrderBy(c => c.Id);
            System.Diagnostics.Debug.WriteLine("RepoCourse course count: " + courses.Count());
            List<CourseBase> courseBaseList = new List<CourseBase>();
            //if (courses == null) return null;
            foreach (var i in courses)
            {
                System.Diagnostics.Debug.WriteLine("RepoCourse course code: " + i.CourseCode);
                CourseBase temp = new CourseBase();
                temp.Id = i.Id;
                temp.CourseCode = i.CourseCode;
                temp.CourseName = i.CourseName;
                courseBaseList.Add(temp);
            }
            //System.Diagnostics.Debug.WriteLine("RepoCourse course list count: " + courseBaseList.Count());
            return courseBaseList;
        }

        //Get select list method
        public SelectList getCourseSelectList()
        {
            var courselist = getListOfCourseBase();
            if (courselist == null)
            {
                System.Diagnostics.Debug.WriteLine("Get courselist null value");
                return null;
            }
            else
            {
                SelectList sl = new SelectList(courselist, "Id", "CourseCode");
                return sl;
            }
        }

        public SelectList getCourseSelectListForFaculty(int? facId)
        {
            var courselist = getListOfCourseForFaculty(facId);
            if (courselist == null)
            {
                System.Diagnostics.Debug.WriteLine("Get courselist null value");
                return null;
            }
            else
            {
                SelectList sl = new SelectList(courselist, "Id", "CourseCode");
                return sl;
            }
        }

        public SelectList getCourseSelectListForStudent(int? stdId)
        {
            var courselist = getListOfCourseForStudent(stdId);
            if (courselist == null)
            {
                System.Diagnostics.Debug.WriteLine("Get courselist null value");
                return null;
            }
            else
            {
                SelectList sl = new SelectList(courselist, "Id", "CourseCode");
                return sl;
            }
        }

    }
}