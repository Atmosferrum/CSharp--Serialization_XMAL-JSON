using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization
{
    class Program
    {
        static void Main()
        {

            string path = "1.xml";

            Repository repository = new Repository(path);

            Menu menu = new Menu(repository);

            menu.StartMenu();

            Console.ReadKey();
        }
    }
}
