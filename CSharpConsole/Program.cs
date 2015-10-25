using System;

namespace CSharpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("started");

            var ebscoData = Client.ebscoSample;

            dynamic ebscoObj = new DynamicJsonValueWrapper(ebscoData);

            var id = ebscoObj.Id;

            //needs to return an array of DynamicJsonWrapper objects
            var members = ebscoObj.members;

            foreach (var item in members)
            {
                dynamic d = new DynamicJsonValueWrapper(item);

                Console.WriteLine("Title : " + d.Title);
                Console.WriteLine("Abstract : " + d.Abstract);
               
           
                Console.WriteLine("--------");
            }

             //push the dynamic part into f#. have function that returns dynamic objects.

            Console.ReadKey();
        }
    }
}

