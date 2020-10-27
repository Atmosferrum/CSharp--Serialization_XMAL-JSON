using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization
{
    struct Employee
    {
        #region Constuctor;

        public Employee(int Number, 
                        string Name,
                        string LastName,
                        int Age,
                        string Department,
                        int Salary,
                        int NumberOfProjects)
        {
            this.number = Number;
            this.name = Name;
            this.lastName = LastName;
            this.age = Age;
            this.department = Department;
            this.salary = Salary;
            this.numberOfProjects = NumberOfProjects;
        }

        #endregion Constuctor


        #region Fields;

        private int number { get; set; }
        private string name { get; set; }
        private string lastName { get; set; }
        private int age { get; set; }
        private string department { get; set; }
        private int salary { get; set; }
        private int numberOfProjects { get; set; }

        #endregion Fields


        #region Properties;

        public int Number { get { return this.number; }
                            set { this.number = value; } }

        public string Name { get { return this.name; }
                             set { this.name = value; } }

        public string LastName { get { return this.lastName; }
                                 set { this.lastName = value; } }

        public int Age { get { return this.age; }
                         set { this.age = value; } }

        public string Department { get { return this.department; }
                                   set { this.department = value; } }

        public int Salary { get { return this.salary; }
                            set { this.salary = value; } }

        public int NumberOfProjects { get { return this.numberOfProjects; }
                                      set { this.numberOfProjects = value; } }

        #endregion Properties


        #region Methods;

        public string print()
        {
            return $"{this.number}" +
                   $"{this.name, 20}" +
                   $"{this.lastName, 20}" +
                   $"{this.age, 20}" +
                   $"{this.department, 20}" +
                   $"{this.salary, 20}" +
                   $"{this.numberOfProjects, 20}";
        }

        #endregion Methods
    }
}
