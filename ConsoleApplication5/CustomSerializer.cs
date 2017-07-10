using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace ConsoleApplication5
{
    class CustomSerializer
    {

        #region Public Method

        /// <summary>
        /// writes json to file in non-append mode
        /// </summary>
        /// <param name="json"></param>
        /// <param name="filePath">enter the path for a file or if non-existant , this method will create one</param>
        public static string serializeAndWrite<T>(T obj,string filePath)
        {
            try
            {
                string json = Serialize<T>(obj);
                System.IO.File.WriteAllText(filePath, json);
                return "File successfully written";
            }
            catch(Exception ex)
            {
               return ex.Message;
            }
        }


        /// <summary>
        /// Serializes the feilds having Customsserializememberattribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            try
            {
                string json;
                if (obj != null)
                {
                    Type type = obj.GetType();
                    FieldInfo[] members = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

                    if (members.Count() > 0)
                        json = returnJsonfromDictionary(members, obj);
                    else
                        json = "\"" + "null" + "\"";
                    return json;
                }
                else
                    return "\"" + "null" + "\"";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

           
        }
        #endregion

        #region private methods
        private static string returnJsonPair(string name, string value, bool isarray = false)
        {
            if (isarray == false)
                return "\"" + name + "\"" + ":" + "\"" + value + "\"";
            else
                return "\"" + value + "\"";

        }
        private static string returnJsonfromDictionary(object[] infos, object obj)
        {
            string json = string.Empty;
            json = "{";
            foreach (object info in infos)
            {
                if (((FieldInfo)info).CustomAttributes.ToList().Exists(x => x.AttributeType.Name.ToString().Contains("CustomSerializeClassMembersAttribute")))
                {
                    json = json + recursivelyCreateJson(info, obj);
                    json = json + ",";
                }
                else
                    return "\""+"null" + "\"";

            }

            json = json + "}";
            json = json.Replace(",}", "}");
            if(json == "{}")
            {
                return null;
            }
            return json;
        }
        private static string recursivelyCreateJson(object info, object parent)
        {
            if (info is FieldInfo)
            {
                FieldInfo feild = info as FieldInfo;
                if (feild.FieldType.FullName.StartsWith("System.String"))
                {//info is a string

                    return returnJsonPair(feild.Name, feild.GetValue(parent) != null ? feild.GetValue(parent).ToString() : "null" );

                }

                if (feild.FieldType.FullName.StartsWith("System.Collections"))
                {//info is a list of string
                    string json = string.Empty;
                    FieldInfo piTheList = (FieldInfo)info;

                    IList oTheList = piTheList.GetValue(parent) as IList;
                    string listname = piTheList.Name;
                    json = "\"" + listname + "\"" + ":";
                    json = json + "[";
                    if (oTheList != null)
                    {
                        foreach (var listItem in oTheList)
                        {
                            json = json + returnJsonPair("", listItem != null ? listItem.ToString() : "null", true);
                            json = json + ",";
                        }
                    }
                    else
                    {
                        json = json + "\"" + "null" + "\"";
                    }
                    json = json + "]";
                    json = json.Replace(",]", "]");
                    return json;
                }
                else
                {//info is a complex class
                    string json = string.Empty;

                    FieldInfo complexType = (FieldInfo)info;
                    json = json + "\"" + complexType.Name + "\"" + ":";
                    var getComplexType = Type.GetType(complexType.FieldType.Namespace + "." + complexType.FieldType.Name);
                    var serialize = typeof(CustomSerializer).GetMethod("Serialize");
                    object complexObject = complexType.GetValue(parent) as object;
                    var getGenericHandle = serialize.MakeGenericMethod(getComplexType);
                    object result = getGenericHandle.Invoke(typeof(CustomSerializer), new Object[] {
      complexObject
     });
                    json = json + (string)result;
                    return json;
                }

            }
            return null;
        }
        #endregion
    }
}