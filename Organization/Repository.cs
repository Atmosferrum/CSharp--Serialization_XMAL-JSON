using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static System.Console;

namespace Organization
{
    struct Repository
    {

        #region Variables;

        List<Employee> employees; //Employees DATA array

        public List<Department> departments; //Departments DATA array

        private string path; //PATH to file

        public int employeeIndex; //Current INDEX for employee to add

        public int departmentIndex; //Current INDEX for department to add

        string[] titles; //TITLES array for Employees 

        #endregion Variables

        #region Constructor;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to file to construct</param>
        public Repository(string Path)
        {
            this.path = Path;
            this.employeeIndex = 0;
            this.departmentIndex = 0;            
            this.employees = new List<Employee>();
            this.departments = new List<Department>();
            this.titles = new string[] { "#",
                                         "Name",
                                         "Last Name",
                                         "Age",
                                         "Department",
                                         "Salary",
                                         "Number Of Projects" };

            Load(path);
        }

        #endregion Constructor

        #region Methods;

        /// <summary>
        /// Main LOAD method that Checks File extenstion before determinds LOAD method
        /// </summary>
        /// <param name="path">Path to check File extenstion</param>
        public void Load(string path)
        {
            this.path = path;

            departments.Clear();
            employees.Clear();
            departmentIndex = 0;
            employeeIndex = 0;

            if (Path.GetExtension(path) == ".xml")
                manualDeserializeXML(this.path);
            else if (Path.GetExtension(path) == ".json")
                manualDeserializeJSON(this.path);
        }

        /// <summary>
        /// Main SAVE method taht Checks file extesion before determinds LOAD method
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            this.path = path;

            if (Path.GetExtension(path) == ".xml")
                manualSerializeXML(this.path);
            else if (Path.GetExtension(path) == ".json")
                manualSerializeJSON(this.path);
        }

        #region XML;

        /// <summary>
        /// Method to SAVE .xml DATA to File
        /// </summary>
        /// <param name="path">Path to SAVE</param>
        void manualSerializeXML(string path)
        {
            XElement myOrganization = new XElement("ORGANIZATION");

            foreach (Department dept in departments)
            {
                int numberOfEmployees = 0;

                XElement myDepartment = new XElement("DEPARTMENT");

                XAttribute departmentName = new XAttribute("name", dept.Name);
                XAttribute departmentDateOfCreation = new XAttribute("dateOfCreation", DateTime.Now.ToShortDateString());

                foreach (Employee emply in employees)
                {
                    if (emply.Department == dept.Name)
                    {
                        XElement myEmployee = new XElement("EMPLOYEE");

                        XAttribute employeeNumber = new XAttribute("number", emply.Number);
                        XAttribute employeeName = new XAttribute("name", emply.Name);
                        XAttribute employeeLastName = new XAttribute("lastName", emply.LastName);
                        XAttribute employeeAge = new XAttribute("age", emply.Age);
                        XAttribute employeeDepartment = new XAttribute("department", emply.Department);
                        XAttribute employeeSalary = new XAttribute("salary", emply.Salary);
                        XAttribute employeeNumberOfProjects = new XAttribute("numberOfProjects", emply.NumberOfProjects);

                        myDepartment.Add(myEmployee);

                        myEmployee.Add(employeeNumber, employeeName, employeeLastName, employeeAge, employeeDepartment, employeeSalary, employeeNumberOfProjects);
                        ++numberOfEmployees;
                    }
                }

                XAttribute departmentNumberOfEmployees = new XAttribute("numberOfEmployees", numberOfEmployees);

                myDepartment.Add(departmentName, departmentDateOfCreation, departmentNumberOfEmployees);

                myOrganization.Add(myDepartment);
            }

            myOrganization.Save(path);
        }

        /// <summary>
        /// Method to LOAD .xml DATA to File
        /// </summary>
        /// <param name="path">Path to LOAD</param>
        void manualDeserializeXML(string path)
        {
            this.departmentIndex = 0;
            this.employeeIndex = 0;

            string xml = File.ReadAllText(path);

            var departmentXML = XDocument.Parse(xml)
                                         .Descendants("ORGANIZATION")
                                         .Descendants("DEPARTMENT")
                                         .ToList();

            foreach (var item in departmentXML)
            {
                AddDepartment(new Department(item.Attribute("name").Value,
                                           DateTime.Parse(item.Attribute("dateOfCreation").Value),
                                           Convert.ToInt32(item.Attribute("numberOfEmployees").Value)));
            }

            var employeeXML = XDocument.Parse(xml)
                                       .Descendants("ORGANIZATION")
                                       .Descendants("DEPARTMENT")
                                       .Descendants("EMPLOYEE")
                                       .ToList();

            foreach (var item in employeeXML)
            {
                AddEmployee(new Employee(Convert.ToInt32(item.Attribute("number").Value),
                                         item.Attribute("name").Value,
                                         item.Attribute("lastName").Value,
                                         Convert.ToInt32(item.Attribute("age").Value),
                                         item.Attribute("department").Value,
                                         Convert.ToInt32(item.Attribute("salary").Value),
                                         Convert.ToInt32(item.Attribute("numberOfProjects").Value)));
            }
        }

        #endregion XML

        #region JSON;

        /// <summary>
        /// Method to SAVE .json DATA to File
        /// </summary>
        /// <param name="path">Path to SAVE</param>
        void manualSerializeJSON(string path)
        {
            JObject organization = new JObject();
            JArray departmentsJSON = new JArray();

            foreach (Department dept in departments)
            {
                int numberOfEmployees = 0;

                JObject departmentJSON = new JObject();
                departmentJSON["name"] = dept.Name;
                departmentJSON["dateOfCreation"] = DateTime.Now.ToShortDateString();

                foreach (Employee emply in employees)
                    if (emply.Department == dept.Name)
                        ++numberOfEmployees;

                departmentJSON["numberOfEmployees"] = numberOfEmployees;

                departmentsJSON.Add(departmentJSON);

                organization["Department"] = departmentsJSON;
            }

            for (int i = 0; i < departments.Count; i++)
            {
                JArray employeesJSON = new JArray();

                foreach (Employee emply in employees)
                {
                    if (emply.Department == departments[i].Name)
                    {
                        JObject employeeJSON = new JObject();
                        employeeJSON["number"] = emply.Number;
                        employeeJSON["name"] = emply.Name;
                        employeeJSON["lastName"] = emply.LastName;
                        employeeJSON["age"] = emply.Age;
                        employeeJSON["department"] = emply.Department;
                        employeeJSON["salary"] = emply.Salary;
                        employeeJSON["numberOfProjects"] = emply.NumberOfProjects;

                        employeesJSON.Add(employeeJSON);

                        organization["Department"][i]["Employee"] = employeesJSON;
                    }
                }
            }

            string json = JsonConvert.SerializeObject(organization);

            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Method to LOAD .json DATA to File
        /// </summary>
        /// <param name="path">Path to LOAD</param>
        void manualDeserializeJSON(string path)
        {
            string json = File.ReadAllText(path);

            var departmentJSON = JObject.Parse(json)["Department"].ToArray();

            foreach (var dept in departmentJSON)
            {
                AddDepartment(new Department(dept["name"].ToString(),
                                             DateTime.Parse(dept["dateOfCreation"].ToString()),
                                             int.Parse(dept["numberOfEmployees"].ToString())));
            }

            for (int i = 0; i < departmentJSON.Length; i++)
            {
                var employeeJSON = JObject.Parse(json)["Department"][i]["Employee"].ToArray();

                foreach (var emply in employeeJSON)
                {
                    AddEmployee(new Employee(Convert.ToInt32(emply["number"].ToString()),
                                             emply["name"].ToString(),
                                             emply["lastName"].ToString(),
                                             Convert.ToInt32(emply["age"].ToString()),
                                             emply["department"].ToString(),
                                             Convert.ToInt32(emply["salary"].ToString()),
                                             Convert.ToInt32(emply["numberOfProjects"].ToString())));
                }
            }
        }

        #endregion JSON

        /// <summary>
        /// Method to SORT by 1 Field
        /// </summary>
        /// <param name="x">1st Field</param>
        public void SortBy(int x)
        {
            switch (x)
            {
                case 1:
                    employees = employees.OrderBy(e => e.Number).ToList();
                    break;
                case 2:
                    employees = employees.OrderBy(e => e.Name).ToList();
                    break;
                case 3:
                    employees = employees.OrderBy(e => e.LastName).ToList();
                    break;
                case 4:
                    employees = employees.OrderBy(e => e.Age).ToList();
                    break;
                case 5:
                    employees = employees.OrderBy(e => e.Department).ToList();
                    break;
                case 6:
                    employees = employees.OrderBy(e => e.Salary).ToList();
                    break;
                case 7:
                    employees = employees.OrderBy(e => e.NumberOfProjects).ToList();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Method to SORT by 2 Fileds
        /// </summary>
        /// <param name="x">1st Filed</param>
        /// <param name="y">2nd Filed</param>
        public void SortBy(int x, int y)
        {
            SortBy(y);
            SortBy(x);
        }

        /// <summary>
        /// Method to SORT by 3 Fileds
        /// </summary>
        /// <param name="x">1st Field</param>
        /// <param name="y">2nd Field</param>
        /// <param name="z">3rd Filed</param>
        public void SortBy(int x, int y, int z)
        {
            SortBy(z);
            SortBy(y);
            SortBy(x);
        }

        /// <summary>
        /// Show results in Console
        /// </summary>
        public void ShowResults()
        {
            WriteLine($"\n{this.titles[0]} {this.titles[1],19} {this.titles[2],19} {this.titles[3],19} {this.titles[4],19} {this.titles[5],19}{this.titles[6],20}");

            for (int i = 0; i < employeeIndex; i++)
                WriteLine(this.employees[i].print());

            WriteLine();
        }
              
        /// <summary>
        ///Method to add EMPLOYEE to Repository 
        /// </summary>
        /// <param name="currentEmployee">Current Record</param>
        public void AddEmployee(Employee currentEmployee)
        {
            this.employees.Add(currentEmployee);
            this.employees[employeeIndex] = currentEmployee;
            this.employeeIndex++;
        }

        /// <summary>
        /// Method to Edit given Employee
        /// </summary>
        /// <param name="number">Number of Employee</param>
        /// <param name="currentEmployee">New DATA for edited Employee</param>
        public void EditEmployee(int number, Employee currentEmployee)
        {
            employees[number - 1] = currentEmployee;
        }

        /// <summary>
        /// Method to Remove give Employee
        /// </summary>
        /// <param name="number">Number of Employee</param>
        public void RemoveEmployee(int number)
        {
            employees.Remove(employees[number - 1]);
            --employeeIndex;

            for (int i = 0; i < this.employees.Count; i++)
            {
                Employee tempEmployee = employees[i];
                tempEmployee.Number = i + 1;
                employees[i] = tempEmployee;
            }
        }

        /// <summary>
        ///Method to add DEPARTMENT to Repository 
        /// </summary>
        /// <param name="currentDepartment">Current Record</param>
        public void AddDepartment(Department currentDepartment)
        {
            this.departments.Add(currentDepartment);
            this.departments[departmentIndex] = currentDepartment;
            this.departmentIndex++;
        }

        /// <summary>
        /// Method to Edit given Employee
        /// </summary>
        /// <param name="number">Number of Employee</param>
        /// <param name="currentDepartment">New DATA for edited Employee</param>
        public void EditDepartment(int number, string name)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                if(employees[i].Department == departments[number - 1].Name)
                {
                    Employee tempEmployee = employees[i];
                    tempEmployee.Department = name;
                    employees[i] = tempEmployee;
                }
            }

            Department tempDepartment = departments[number - 1];
            tempDepartment.Name = name;
            departments[number - 1] = tempDepartment;            
        }

        /// <summary>
        /// Method to Remove given Department
        /// </summary>
        /// <param name="number">Number of Department</param>
        public void RemoveDepartment(int number)
        {
            int tempEmployeeCount = this.employees.Count - 1;

            for (int i = tempEmployeeCount; i > - 1; i--)
            {
                if (employees[i].Department == departments[number - 1].Name)
                    RemoveEmployee(i + 1);
            }

            departments.Remove(departments[number - 1]);
            --departmentIndex;            
        }
        
        /// <summary>
        /// Method to MOVE Employees to given Department
        /// </summary>
        /// <param name="from">From THIS Department</param>
        /// <param name="to">To THIS Department</param>
        public void MoveToDepartment(int from, int to)
        {
            int tempEmployeeCount = this.employees.Count - 1;

            //Department tempDepartment;

            //foreach (Department dept in departments)
            //    if (dept.Name == to)
            //        tempDepartment = dept;

            for (int i = tempEmployeeCount; i > - 1; i--)
            {
                if (employees[i].Department == departments[from - 1].Name)
                {
                    Employee tempEmployee = employees[i];
                    tempEmployee.Department = departments[to -1].Name;
                    employees[i] = tempEmployee;
                }
            }          
        }

        /// <summary>
        /// Method to MOVE Employees to given Department
        /// </summary>
        /// <param name="from">From THIS Department</param>
        /// <param name="to">To THIS Department</param>
        /// 
        public void MoveToDepartment(int from, string to)
        {
            int tempEmployeeCount = this.employees.Count - 1;

            for (int i = tempEmployeeCount; i > - 1; i--)
            {
                if (employees[i].Department == departments[from - 1].Name)
                {
                    Employee tempEmployee = employees[i];
                    tempEmployee.Department = to;
                    employees[i] = tempEmployee;
                }
            }          
        }

        #endregion Methods

    }
}
