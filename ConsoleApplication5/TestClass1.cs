using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication5
{
    
    class TestClass1
    {
        [CustomSerializeClassMembers]
        public string namagername;
        [CustomSerializeClassMembers]
        public List<string> employeereporting;
        [CustomSerializeClassMembers]
        public TestClass2 t1;
    }
}
