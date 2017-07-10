using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    [AttributeUsage( AttributeTargets.Field)]
    class CustomSerializeClassMembersAttribute : Attribute
    {
        
    }
}
