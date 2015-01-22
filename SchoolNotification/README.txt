INT422Project | Linpei Fan | 068721117

There are seven domain classes: Faculty, Student, Admin, Course, Cancellation, Message, Contact in this project.

There are three roles in this project: Faculty, Student and Admin. When a user logs in, he/she will see the different home page depending on the role he/she belongs to. 

When a user from following different roles logs in, the available functionalities are described as follows:

++++++++++++Faculty:
When a faculty logs in, it directly opens the Cancellation Index page.

The faculty can do following things:
1. Create a cancellation. On cancellation create page, a faculty only can access the course he/she teaches. The date on create page only shows the list from the current system date to the end of the semester. Moreover, the system will automatically catch the faculty information(name or facultyId) to create a new cancellation.

2. When creating a new cancellation, a corresponding message will be created as well. A faculty can view the massage details and edit the message.

3. A faculty can CRUD the cancellations.

4. A faculty can see all of the cancellations he/she created, including previous and upcoming cancellations.

+++++++++++++Student:
When a student logs in, it directly opens the Student home page. On this page, a student can view all the courses he/she registered, then view all upcoming the cancellations for the courses he/she registered, and finally add/modify/view/delete all of the contact information he/she has.

+++++++++++++Admin:
When an admin logs in, it directly opens the admin home page. An admin can do all the things that a faculty and a student can do. But an admin also have more additional functions. Please see details as follows:

1. Manage Faculty. An admin can CRUD faculties here. When creating a new faculty, it automatically registers a faculty as a user in faculty role and assigns a default password "123456".

2. Manage Course. An admin can CRUD courses here.

3. Manage Student. An admin can CRUD student here. When creating a new student, it automatically registers a student as a user in student role and assigns a default password "123456". Moreover, an admin can CRUD student contacts like a student does.

4. Manage Cancellaiton. An admin can CRUD cancellations for any faculties or courses.

4.1. On this section, an admin has the search function available to search the cancellation by CourseCode.

4.2. When an admin creates an cancellation, he/she has all of the course list available and all of the faculty list available to select.

4.3. Rest of the cancellation functions for an admin are same as those for faculties.

5. Manage Admin. An admin can CRUD admin here. When creating a new admin, it automatically registers an admin as a user in admin role and assigns a default password "123456".

++++++++++++++Register function on home page:
When a user registers on the home page, it will automatically be assigned to the role faculty.

