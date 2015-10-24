using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSharp.Data;
using FSharp.Data.Runtime.BaseTypes;

namespace CSharpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("started");



            var data = WorldBankProvider.records("us");

            var second = data[1];

            var country = WorldBankProvider.getCountry(second);


            dynamic bankObj = new DynamicJsonWrapper(country);

            var countryName = bankObj.Value;

            Console.WriteLine("Country name is " + countryName);

            

            //var countryId = WorldBankProvider.getGeneric(country, "id");

            //var countryValue = WorldBankProvider.getGeneric(country, "value");

            //Console.WriteLine("id is " + countryId + "; name is " + countryValue);

            Console.ReadKey();
        }
    }
}
