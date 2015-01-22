using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using CoursePlanning.Models;

namespace CoursePlanning.ServiceLayer
{
    // Unit of Work

    public class Worker : IDisposable
    {
        private DataContext _ds = new DataContext();
        private bool disposed = false;

        // Properties for each repository

        // Custom getters for each repository

        private Course_repo _courses;

        public Course_repo Courses
        {
            get
            {
                if (this._courses == null)
                {
                    this._courses = new Course_repo(_ds);
                }
                return _courses;
            }
        }

        private Program_repo _program;

        public Program_repo Programs
        {
            get
            {
                if (this._program == null)
                {
                    this._program = new Program_repo(_ds);
                }
                return _program;
            }
        }

        private CurriculumPlan_repo _curriculumPlan;

        public CurriculumPlan_repo CurriculumPlans
        {
            get
            {
                if (this._curriculumPlan == null)
                {
                    this._curriculumPlan = new CurriculumPlan_repo(_ds);
                }
                return _curriculumPlan;
            }
        }

        private Position_repo _position;

        public Position_repo Positions
        {
            get
            {
                if (this._position == null)
                {
                    this._position = new Position_repo(_ds);
                }
                return _position;
            }
        }



        // ############################################################

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _ds.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

}
