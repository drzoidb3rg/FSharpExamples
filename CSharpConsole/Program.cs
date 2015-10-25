using System;

namespace CSharpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("started");

            dynamic ebscoObj = new DynamicJsonWrapper(Client.ebscoSample);

            Console.WriteLine("Id is " + ebscoObj.Id) ;

            var members = ebscoObj.members;

            foreach (var item in members)
            {
                Console.WriteLine("Title : " + item.Title);
                Console.WriteLine("Abstract : " + item.Abstract);
           
                Console.WriteLine("--------");
            }

             //push the dynamic part into f#. have function that returns dynamic objects.

            Console.ReadKey();
        }
    }
}

