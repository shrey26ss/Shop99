using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Data
{
    public static class ObjectToDictionay
    {
        public static Dictionary<string, dynamic> ToDictionary(this object someObject)
        {
            try
            {
                //var res = someObject.GetType()
                //.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                //.Where(prop => prop.GetIndexParameters().Length == 0) // Check if property is indexed
                //.ToDictionary(prop => prop.Name, prop => (dynamic)prop.GetValue(someObject, null));
                someObject = someObject.ToString();
                Dictionary<string, dynamic> dictionary = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(someObject.ToString());
                return dictionary;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
