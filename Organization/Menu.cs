using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Organization
{
    struct Menu
    {
        Repository repository; // Link to Repository

        #region Constructor;
        /// <summary>
        /// Menu Constructor
        /// </summary>
        /// <param name="Repository">Repository for all DATA</param>
        public Menu(Repository Repository)
        {
            this.repository = Repository;
        }

        #endregion Constructor

        #region Methods;

        /// <summary>
        /// Method to START Menu
        /// </summary>
        public void StartMenu()
        {
            ShowTitle();
        }

        /// <summary>
        /// Method to SHOW Title
        /// </summary>
        private void ShowTitle()
        {
            string nameOfTheProgram = "Organization management v1.0";
            ForegroundColor = ConsoleColor.DarkRed;
            SetCursorPosition((WindowWidth - nameOfTheProgram.Length) / 2, 0);
            WriteLine(nameOfTheProgram);
            ForegroundColor = ConsoleColor.White;

            ShowMenu();
        }

        /// <summary>
        /// Method to SHOW Menu
        /// </summary>
        private void ShowMenu()
        {
            repository.ShowResults();

            WriteLine($"\n1. Load File." +
                      $"\n2. Save File." +
                      $"\n3. Add Employee." +
                      $"\n4. Edit Employee." +
                      $"\n5. Remove Employee." +
                      $"\n6. Add Department." +
                      $"\n7. Edit Department." +
                      $"\n8. Remove Department." +
                      $"\n9. Sort by 1 Field." +
                      $"\n10. Sort by 2 Fields." +
                      $"\n11. Sort by 2 Fields & Department." +
                      $"\n" +
                      $"\nInput option number : ");

            GetOptions();
        }

        /// <summary>
        /// Method to LOAD given Option
        /// </summary>
        private void GetOptions()
        {
            int optionsCount = 11;

            switch (GetOptionNumber(optionsCount))
            {
                case 1:
                    LoadFile(); //Option to LOAD file
                    break;
                case 2:
                    SaveFile(); //Option to SAVE file
                    break;
                case 3:
                    AddEmployee(); //Option to ADD Employee
                    break;
                case 4:
                    EditEmployee(); //Option to EDIT Employee
                    break;
                case 5:
                    RemoveEmployee(); //Option to REMOVE Employee
                    break;
                case 6:
                    AddDepartment(); //Option to ADD Department
                    break;
                case 7:
                    EditDepartment(); //Option to EDIT Department
                    break;
                case 8:
                    RemoveDepartment(); //Option to REMOVE Department
                    break;
                case 9:
                    SortByOneField(); //Option to SORT by one Field
                    break;
                case 10:
                    SortByTwoFields(); //Option to SORT by two Fields
                    break;
                case 11:
                    SortByThreeFields(); //Option to SORT by three Fields
                    break;
                default:
                    GetOptions();
                    break;
            }

            OptionsEnd();
        }

        /// <summary>
        /// Method to PARSE option Number from given String
        /// </summary>
        /// <param name="numberOfOptions">Number of options</param>
        /// <returns>Int Option Number if String is legit</returns>
        private int GetOptionNumber(int numberOfOptions)
        {
            int number;

            bool parse = false;

            do
            {
                //Bool to check if file text is legit
                bool result = int.TryParse(ReadLine(), out number);
                //If legit, end loop. If not, repeat
                if (result && number <= numberOfOptions && number > 0)
                    parse = true;
                else
                    WriteLine("\nThere's no available option with given number, please input another one : ");

            } while (!parse);

            return number;
        }

        /// <summary>
        /// Method to ask for Path to LOAD file
        /// </summary>
        private void LoadFile()
        {
            WriteLine($"\n" +
                      $"\nInput file path to LOAD : ");

            repository.Load(ExtensionParser());
        }

        /// <summary>
        /// Method to ask for Path to SAVE file
        /// </summary>
        private void SaveFile()
        {
            WriteLine($"\n" +
                      $"\nInput file path to SAVE : ");

            repository.Save(ExtensionParser());
        }

        /// <summary>
        /// Method to ADD Employee
        /// </summary>
        private void AddEmployee()
        {
            WriteLine("\nInput employee Name : ");
            string name = ReadLine();

            WriteLine("\nInput employee Lastname : ");
            string lastName = ReadLine();

            WriteLine("\nInput employee Age : ");
            int age = NumberParser();

            WriteLine("\nChose employee Department : ");
            int departmentIndex = 0;

            foreach (Department dept in repository.departments)
            {
                WriteLine($"{departmentIndex + 1} {dept.Name}.");
                ++departmentIndex;
            }
            string department = repository.departments[GetOptionNumber(departmentIndex) - 1].Name;

            WriteLine("\nInput employee Salary : ");
            int salary = NumberParser();

            WriteLine("\nInput employee Number of Projects : ");
            int numberOfProjects = NumberParser();

            int number = repository.employeeIndex + 1;

            repository.AddEmployee(new Employee(number, name, lastName, age, department, salary, numberOfProjects));

            repository.SortBy(1, 5);

        }

        /// <summary>
        /// Method to EDIT Employee
        /// </summary>
        private void EditEmployee()
        {
            WriteLine("\nChoose Employee # to edit : ");

            int employeeToEditNumber = GetOptionNumber(repository.employeeIndex);

            WriteLine("\nInput employee Name : ");
            string name = ReadLine();

            WriteLine("\nInput employee Lastname : ");
            string lastName = ReadLine();

            WriteLine("\nInput employee Age : ");
            int age = NumberParser();

            WriteLine("Chose employee Department : ");
            int departmentIndex = 0;

            foreach (Department dept in repository.departments)
            {
                WriteLine($"{departmentIndex + 1} {dept.Name}.");
                ++departmentIndex;
            }
            string department = repository.departments[GetOptionNumber(departmentIndex) - 1].Name;

            WriteLine("\nInput employee Salary : ");
            int salary = NumberParser();

            WriteLine("\nInput employee Number of Projects : ");
            int numberOfProjects = NumberParser();

            repository.EditEmployee(employeeToEditNumber, new Employee(employeeToEditNumber, name, lastName, age, department, salary, numberOfProjects));

            repository.SortBy(1, 5);
        }

        /// <summary>
        /// Method to REMOVE Employee
        /// </summary>
        private void RemoveEmployee()
        {
            WriteLine("\nChoose employee # to remove : ");

            int employeeToRemoveNumber = GetOptionNumber(repository.employeeIndex);

            repository.RemoveEmployee(employeeToRemoveNumber);

            repository.SortBy(1, 5);
        }

        /// <summary>
        /// Method to PARSE Number form given String
        /// </summary>
        /// <returns>Int Number if String is legit</returns>
        private int NumberParser()
        {
            int number;

            bool result = false;

            do
            {
                //Bool to check if file text is legit
                result = int.TryParse(ReadLine(), out number);
                //If legit, end loop. If not, repeat
                if (result)
                    break;
                else
                    WriteLine("\nThis is not a number, please try again : ");

            } while (!result);

            return number;
        }

        /// <summary>
        /// Method to PARSE File Extension (.xml or .json)
        /// </summary>
        /// <returns>File path if Extension is legit</returns>
        private string ExtensionParser()
        {
            bool parsed = false;

            string path;

            do
            {
                path = ReadLine();

                string pathExtension = Path.GetExtension(path);

                //If legit, end loop. If not, repeat
                if (pathExtension == ".xml" || pathExtension == ".json")
                    parsed = true;
                else
                    WriteLine("\nThe extension is wrong, try another one : ");
            } while (!parsed);

            return path;
        }

        /// <summary>
        /// Method to SHOW Menu after Action ends
        /// </summary>
        private void OptionsEnd()
        {
            WriteLine();
            ShowMenu();
        }

        /// <summary>
        /// Method to ADD Department
        /// </summary>
        private void AddDepartment()
        {
            WriteLine("\nInput department Name : ");
            string name = ReadLine();

            repository.AddDepartment(new Department(name, DateTime.Now, 0));

            WriteLine("\nAdd employee to new department?" +
                      "\n1. Yes" +
                      "\n2. No");

            if (GetOptionNumber(2) == 1)
                AddEmployee();

            repository.SortBy(1, 5);
        }

        /// <summary>
        /// Method to EDIT Department's Name
        /// </summary>
        private void EditDepartment()
        {
            WriteLine("\nChoose department # to edit : ");
            int departmentIndex = 0;
            foreach (Department dept in repository.departments)
            {
                WriteLine($"{departmentIndex + 1} {dept.Name}.");
                ++departmentIndex;
            }
            int departmentToEdit = GetOptionNumber(repository.departmentIndex);

            WriteLine("\nInput department Name : ");
            string name = ReadLine();

            repository.EditDepartment(departmentToEdit, name);

            repository.SortBy(1, 5);
        }

        /// <summary>
        /// Mehtod to REMOVE Department
        /// </summary>
        private void RemoveDepartment()
        {
            WriteLine("\nChoose department # to remove : ");
            int departmentIndex = 0;
            foreach (Department dept in repository.departments)
            {
                WriteLine($"{departmentIndex + 1} {dept.Name}.");
                ++departmentIndex;
            }
            int departmentToRemove = GetOptionNumber(repository.departmentIndex);

            WriteLine("\nChoose what to do with employees : " +
                      "\n1. Fire everyone." +
                      "\n2. Move to another department.");

            int numberOfOptions = 2;

            int employeesFate = GetOptionNumber(numberOfOptions);

            if (employeesFate == 2)
            {
                if (repository.departments.Count > 1)
                    ChooseNewDepartment(departmentToRemove);
                else
                {
                    WriteLine("\nYou should create new department for your employees : ");
                    AddDepartment();
                    ChooseNewDepartment(departmentToRemove);
                }
            }

            repository.RemoveDepartment(departmentToRemove);

            repository.SortBy(1, 5);
        }

        /// <summary>
        /// Method to SORT by 1 Filed
        /// </summary>
        private void SortByOneField()
        {
            int numberOfFields = 7;

            WriteLine("\nChoose filed to sort by :" +
                      "\n1. Number." +
                      "\n2. Name." +
                      "\n3. Lastname" +
                      "\n4. Age." +
                      "\n5. Department." +
                      "\n6. Salary." +
                      "\n7. Number of projects.");

            int fieldX = GetOptionNumber(numberOfFields);

            repository.SortBy(fieldX);
        }

        /// <summary>
        /// Method to SORT by 2 Filed
        /// </summary>
        private void SortByTwoFields()
        {
            int numberOfFields = 7;

            WriteLine("\nChoose first filed to sort by :" +
                      "\n1. Number." +
                      "\n2. Name." +
                      "\n3. Lastname" +
                      "\n4. Age." +
                      "\n5. Department." +
                      "\n6. Salary." +
                      "\n7. Number of projects.");

            int fieldX = GetOptionNumber(numberOfFields);

            WriteLine("\nChoose second filed to sort by :" +
                      "\n1. Number." +
                      "\n2. Name." +
                      "\n3. Lastname" +
                      "\n4. Age." +
                      "\n5. Department." +
                      "\n6. Salary." +
                      "\n7. Number of projects.");

            int fieldY = GetOptionNumber(numberOfFields);

            repository.SortBy(fieldX, fieldY);
        }

        /// <summary>
        /// Method to SORT by 3 Filed
        /// </summary>
        private void SortByThreeFields()
        {
            int numberOfFields = 7;

            WriteLine("\nChoose first filed to sort by :" +
                      "\n1. Number." +
                      "\n2. Name." +
                      "\n3. Lastname" +
                      "\n4. Age." +
                      "\n5. Department." +
                      "\n6. Salary." +
                      "\n7. Number of projects.");

            int fieldX = GetOptionNumber(numberOfFields);

            WriteLine("\nChoose second filed to sort by :" +
                      "\n1. Number." +
                      "\n2. Name." +
                      "\n3. Lastname" +
                      "\n4. Age." +
                      "\n5. Department." +
                      "\n6. Salary." +
                      "\n7. Number of projects.");

            int fieldY = GetOptionNumber(numberOfFields);

            WriteLine("\nChoose third filed to sort by :" +
                      "\n1. Number." +
                      "\n2. Name." +
                      "\n3. Lastname" +
                      "\n4. Age." +
                      "\n5. Department." +
                      "\n6. Salary." +
                      "\n7. Number of projects.");

            int fieldZ = GetOptionNumber(numberOfFields);

            repository.SortBy(fieldX, fieldY, fieldZ);
        }

        /// <summary>
        /// Method to CHOOSE another Department when removeing
        /// </summary>
        /// <param name="departmentToRemove">Department that will be REMOVED</param>
        private void ChooseNewDepartment(int departmentToRemove)
        {
            WriteLine("\nChoose department # to move employees : ");
            int departmentIndex = 0;

            foreach (Department dept in repository.departments)
            {
                if (dept.Name != repository.departments[departmentToRemove - 1].Name)
                {
                    WriteLine($"{departmentIndex + 1} {dept.Name}.");
                }

                ++departmentIndex;
            }

            int departmentToMove = GetOptionNumber(repository.departmentIndex);

            repository.MoveToDepartment(departmentToRemove, departmentToMove);
        }

        #endregion Methods

    }
}
