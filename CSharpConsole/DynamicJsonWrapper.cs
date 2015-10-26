using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using FSharp.Data;
using FSharp.Data.Runtime.BaseTypes;

namespace CSharpConsole
{
    public class DynamicJsonWrapper : DynamicObject
    {
        private JsonValue _data;

        public DynamicJsonWrapper(JsonValue jsonValue)
        {
            _data = jsonValue;
        }

        public DynamicJsonWrapper(IJsonDocument jsonDocument)
         {
             _data = jsonDocument.JsonValue;
         }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_data == null)
            {
                result = "[error-json-document-is-null]";
                return false;
            }

            var propertyName = binder.Name;

            if (propertyName == "Id")
                propertyName = "@id";

            if (propertyName == "Title")
                propertyName = "dc.title";


            if (propertyName == "Abstract")
                propertyName = "fabio:abstract";

            var temp = Client.getSafeJsonValue(_data, propertyName);

            if (temp is JsonValue)
            {
                result = new DynamicJsonWrapper(temp as JsonValue);
                return true;
            }

            if (temp is JsonValue[])
            {
                var array = temp as JsonValue[];
                result = array.Select(item => new DynamicJsonWrapper(item)).ToList();
                return true;
            }

            result = temp;

            return true;
        }


    }
}
