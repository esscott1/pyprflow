using System;

namespace pyprflow.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to pyprflow CLI where workflows can be create, managed, and executed from command line commands");
            var cliResonse = Console.ReadLine();

            Console.WriteLine(Cli(cliResonse));
            Console.ReadLine();
;        }

        static string Cli(string sInput)
        {
            string response = "error";
            if(sInput.StartsWith("pflow"))
                response =  "I did something";

            return response;
        }
    }
}