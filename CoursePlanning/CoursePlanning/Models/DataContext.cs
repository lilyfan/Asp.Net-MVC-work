using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// more...
using System.Data.Entity;

// This source code file is used to define the data context and optional store initializer

// The data context is directly used by manager (i.e. data service operations) classes,
// and other business and application classes

// The store initializer is used in simple projects, and enables you to create some initial data

namespace CoursePlanning.Models
{
    /// <summary>
    /// Data context, the gateway to the project's persistent store
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Default constructor, notice the base name matches the name used in the Web.config connection string
        /// </summary>
        public DataContext() : base("name=DefaultConnection") { }

        // Add DbSet<TEntity> properties here
        public DbSet<Course> Courses { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<CurriculumPlan> CurriculumPlans { get; set; }
        public DbSet<Position> Positions { get; set; }
    }

    /// <summary>
    /// Store initializer, notice the inheritance
    /// </summary>
    public class StoreInitializer : DropCreateDatabaseAlways<DataContext>
    {
        // Enabled by a statement in the WebApiApplication class (in Global.asax.cs)
        // Disable that statement when you are using Entity Framework Code First Migrations

        /// <summary>
        /// Seed the data store with initial data
        /// </summary>
        /// <param name="context">The project's data context object</param>
        protected override void Seed(DataContext context)
        {
            // Create new curriculum plans

            //var cpCPAC13 = new CurriculumPlan(); cpCPAC13.Code = "2013CPAC" ;
            var cpCPAC14 = new CurriculumPlan(); cpCPAC14.Code = "2014CPAC" ;
            var cpCPAC15 = new CurriculumPlan(); cpCPAC15.Code = "2015CPAC" ;
            //var cpCTYC13 = new CurriculumPlan(); cpCTYC13.Code = "2013CTYC" ;
            var cpCTYC14 = new CurriculumPlan(); cpCTYC14.Code = "2014CTYC" ;
            var cpCTYC15 = new CurriculumPlan(); cpCTYC15.Code = "2015CTYC" ;
            //var cpBSD13 = new CurriculumPlan(); cpBSD13.Code = "2013BSD" ;
            var cpBSD14 = new CurriculumPlan(); cpBSD14.Code = "2014BSD" ;
            var cpBSD15 = new CurriculumPlan(); cpBSD15.Code = "2015BSD" ;

            //Create new positions
            var position1ULI101 = new Position(); position1ULI101.Semester = 1; position1ULI101.DisplayOrder = 1 ;
            var position2OOP244 = new Position(); position2OOP244.Semester = 2; position2OOP244.DisplayOrder = 2 ;
            cpCPAC14.AssoPositions.Add(position1ULI101);
            cpCPAC14.AssoPositions.Add(position2OOP244);

            var position3WIN210 = new Position(); position3WIN210.Semester = 2; position3WIN210.DisplayOrder = 1 ;
            var position4OPS235 = new Position(); position4OPS235.Semester = 2; position4OPS235.DisplayOrder = 2;
            cpCTYC14.AssoPositions.Add(position3WIN210);
            cpCTYC14.AssoPositions.Add(position4OPS235);

            var position5BTP100 = new Position(); position5BTP100.Semester = 1; position5BTP100.DisplayOrder = 1 ;
            var position6BTB110 = new Position(); position6BTB110.Semester = 1; position6BTB110.DisplayOrder = 2;
            cpBSD14.AssoPositions.Add(position5BTP100);
            cpBSD14.AssoPositions.Add(position6BTB110);

            var position7IPC144 = new Position(); position7IPC144.Semester = 1; position7IPC144.DisplayOrder = 1 ;
            cpCPAC14.AssoPositions.Add(position7IPC144);
            var position8IPC144 = new Position(); position8IPC144.Semester = 5; position8IPC144.DisplayOrder = 5;
            cpCTYC14.AssoPositions.Add(position8IPC144);

            var uli101 = new Course(); 
                uli101.Code = "ULI101";
                uli101.Title = "Introduction To Unix/linux, & The Internet";
                uli101.Description = "Unix and Linux represent the operating system technology underlying many of the services of the Internet. This subject introduces students to Unix, Linux and the Internet. Students will learn the core utilities to work productively in a Linux environment. Students will do this work using the Bash shell, at the same time learn to configure their login accounts, manipulate data stored in files, effectively use Linux commands and utilities, and write simple shell scripts.";
                uli101.DateStarted = new DateTime(2003, 01, 01);
                uli101.DateRetired = new DateTime(2023, 01, 01);
            uli101.AssoPositions.Add(position1ULI101);
            var oop244 = new Course();
                oop244.Code = "OOP244";
                oop244.Title = "Introduction To Object Oriented Programming Using C++";
                oop244.Description = "This subject introduces the student to object-oriented programming. The student learns to build reusable objects, encapsulate data and logic within a class, inherit one class from another and implement polymorphism.  This subject uses the C++ programming language exclusively and establishes a foundation for learning system analysis and design and more advanced concepts as implemented in languages such as C++, Java, C# and Objective-C.";
                oop244.DateStarted = new DateTime(2000, 01, 01);
                oop244.DateRetired = new DateTime(2020, 01, 01);
            oop244.AssoPositions.Add(position2OOP244);
            var win210 = new Course();
                win210.Code = "WIN210";
                win210.Title = "Basic Administration of Microsoft Windows";
                win210.Description = "This subject provides students with the knowledge and skills necessary to perform administration tasks on a Microsoft Windows Server operating system. Topics such as installation, user account creation and maintenance, permissions, printing, hardware and disk storage will be covered through lecture and hands-on exercises.";
                win210.DateStarted = new DateTime(2000, 01, 01);
                win210.DateRetired = new DateTime(2020, 01, 01);
            win210.AssoPositions.Add(position3WIN210);
            var ops235 = new Course();
                ops235.Code = "OPS235";
                ops235.Title = "Introduction To Open System Servers";
                ops235.Description = "This project-based subject will teach students how to install and configure a Linux server. Further, students will learn how to connect to and communicate over a network in a controlled environment. They will learn how to manage their files on their system and how to set-up file and directory permissions. Students will manage basic system security and firewall settings. In order to configure their system, students will gain knowledge of a few basic Unix/Linux commands and be exposed to 'Shell' basics. Finally students will work with both text and graphical user interfaces.";
                ops235.DateStarted = new DateTime(2003, 01, 01);
                ops235.DateRetired = new DateTime(2020, 01, 01);
            ops235.AssoPositions.Add(position4OPS235);
            var btp100 = new Course();
                btp100.Code = "BTP100";
                btp100.Title = "Programming Fundamentals Using C";
                btp100.Description = "This subject covers the fundamental principles of computer programming, with an emphasis on problem solving strategies using structured programming techniques. The C programming language, which is widely used and forms the syntactical basis for object-oriented languages such as C++, C#, Objective-C, and Java, is used to introduce problem analysis, algorithm design, and program implementation.";
                btp100.DateStarted = new DateTime(2000, 01, 01);
                btp100.DateRetired = new DateTime(2020, 01, 01);
            btp100.AssoPositions.Add(position5BTP100);
            var btb110 = new Course();
                btb110.Code = "BTB110";
                btb110.Title = "Accounting for Business";
                btb110.Description = "This course introduces students to the principles of business accounting and financial management. Students learn the theory and calculations supporting profit and loss statements, interest payments and present value, and have the opportunity to apply concepts learned in a series of business-based assignments. Students will also study the application of the learned financial concepts in real-world ICT projects.";
                btb110.DateStarted = new DateTime(2000, 01, 01);
                btb110.DateRetired = new DateTime(2020, 01, 01);
            btb110.AssoPositions.Add(position6BTB110);
            var ipc144 = new Course();
                ipc144.Code = "IPC144";
                ipc144.Title = "Introduction To Programming Using C";
                ipc144.Description = "This subject covers the fundamental principles of computer programming, with an emphasis on problem solving strategies using structured programming techniques. The C programming language, which is widely used and forms the syntactical basis for object-oriented languages such as C++, C#, Objective-C, and Java, is used to introduce problem analysis, algorithm design, and program implementation. Students work in a LINUX environment.";
                ipc144.DateStarted = new DateTime(2000, 01, 01);
                ipc144.DateRetired = new DateTime(2020, 01, 01);
            ipc144.AssoPositions.Add(position7IPC144);
            ipc144.AssoPositions.Add(position8IPC144);

            position1ULI101.AssoCourse = uli101;
            position2OOP244.AssoCourse = oop244;
            position3WIN210.AssoCourse = win210;
            position4OPS235.AssoCourse = oop244;
            position5BTP100.AssoCourse = btp100;
            position6BTB110.AssoCourse = btb110;
            position7IPC144.AssoCourse = ipc144;
            position8IPC144.AssoCourse = ipc144;

            //Create new program
            var cpac = new Program();
                cpac.Code = "CPAC";
                cpac.Title = "Computer Programming & Analysis";
                cpac.Description = "The CPA program provides students with a rigorous theoretical background in object-oriented methodology particularly in program design and system analysis. Internet concepts are integrated into all courses across the curriculum and focus on the development of dynamic database-driven web applications on a variety of operating system platforms. Small class sizes, computerized classrooms and an expert faculty provide an enabling environment for students to master the craft of programming and analysis. Opportunities will be provided for students to develop individual and teamwork skills. Students with post secondary education or work experience may apply for credits or challenge exams in related subjects.";
                cpac.Credential = "Ontario College Advanced Diploma";
                cpac.DateStarted = new DateTime(1990, 09, 03);
                cpac.DateRetired = new DateTime(2050, 01,01);
            cpac.AssoCurriculumPlan = cpCPAC14;
            cpac.AssoCourses.Add(uli101);
            cpac.AssoCourses.Add(oop244);
            cpac.AssoCourses.Add(ipc144);
            var ctyc = new Program();
                ctyc.Code = "CTYC";
                ctyc.Title = "Computer System Technology";
                ctyc.Description = "This program places the emphasis on practical subjects and 'hands-on' training delivered in speciality labs. These dedicated labs are configured for various environments such as Linux, Microsoft Windows, networking, PC hardware, Internet, AS/400, and RS/6000. Through these labs students have access to standalone, clustered and networked PC's along with a host of LAN servers and mid-range computers. Subjects studied include PC hardware, operating systems, Internet, HTML, Linux/Unix and Microsoft Windows Server administration, data communications, Novell Netware, AS/400 connectivity, web server installation and maintenance, Voice over Internet Protocol and Security. In semesters four, five and six students may select from a broad offering of professional options and will integrate systems and technical skills in the planning and implementation of a 'real-life' computer project for a business client.";
                ctyc.Credential = "Ontario College Advanced Diploma";
                ctyc.DateStarted = new DateTime(1990, 09, 03);
                ctyc.DateRetired = new DateTime(2050, 01, 01);
            ctyc.AssoCurriculumPlan = cpCTYC14;
            ctyc.AssoCourses.Add(win210);
            ctyc.AssoCourses.Add(ops235);
            ctyc.AssoCourses.Add(ipc144);
            var bsd = new Program();
                bsd.Code = "BSD";
                bsd.Title = "Bachelor of Technology (Software Development)";
                bsd.Description ="This unique and innovative degree program was created to address the need for knowledgeable software developers, skilled in both the technical and non-technical aspects of business information technology. A strong theoretical base is developed through extensive practical experience on a variety of computer platforms. The program also emphasizes development of English communications and business skills for today's knowledge-based economy, and includes features such as a paid co-op work term that prepares graduates who are fully functional upon employment. The Bachelor of Technology (Software Development), known as the BSD Program, is a four-year (eight semester) program that offers students the opportunity to start in either September or January. Students learn at Seneca's modern TEL (Technology Enhanced Learning) and SEQ (Steven E. Quinlan) buildings, located on the York University campus in Toronto.";
                bsd.Credential = "Bachelor of Technology (Software Development)";
                bsd.DateStarted = new DateTime(1990, 09, 03);
                bsd.DateRetired = new DateTime(2050, 01, 01);
            bsd.AssoCurriculumPlan = cpBSD14;
            bsd.AssoCourses.Add(btp100);
            bsd.AssoCourses.Add(btb110);
            
            context.CurriculumPlans.Add(cpCPAC14);
            context.CurriculumPlans.Add(cpCPAC15);
            context.CurriculumPlans.Add(cpCTYC14);
            context.CurriculumPlans.Add(cpCTYC15);
            context.CurriculumPlans.Add(cpBSD14);
            context.CurriculumPlans.Add(cpBSD15);

            context.Positions.Add(position1ULI101);
            context.Positions.Add(position2OOP244);
            context.Positions.Add(position3WIN210);
            context.Positions.Add(position4OPS235);
            context.Positions.Add(position5BTP100);
            context.Positions.Add(position6BTB110);

            context.Courses.Add(uli101);
            context.Courses.Add(oop244);
            context.Courses.Add(win210);
            context.Courses.Add(ops235);
            context.Courses.Add(btp100);
            context.Courses.Add(btb110);
            context.Courses.Add(ipc144);

            context.Programs.Add(cpac);
            context.Programs.Add(ctyc);
            context.Programs.Add(bsd);

            //Save the change to data context
            context.SaveChanges();

        }
    }
}
