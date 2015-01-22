using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolNotification.Models;

namespace SchoolNotification.ViewModels
{
    public class RepositoryBase
    {
        public RepositoryBase()
        {
            dc = new DataContext();
            dc.Configuration.ProxyCreationEnabled = true;
            dc.Configuration.LazyLoadingEnabled = true;
        }

        protected DataContext dc;
    }
}