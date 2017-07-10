using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass1 t = new TestClass1()
            {  namagername = "Mr Anderson",

                employeereporting = new List<string>
                {
                   "Neo","Morpheus","Trinity"
                }
                ,
                t1 = new TestClass2()
                {
                    abc = "This is my third commit"
                }
                
                
            };

         string status =   CustomSerializer.serializeAndWrite<TestClass1>(t , @"C:\Test\Json.txt");

           
        }
    }
}
