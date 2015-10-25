using System.Dynamic;
using FSharp.Data;
using FSharp.Data.Runtime.BaseTypes;

namespace CSharpConsole
{
    public class DynamicJsonValueWrapper : DynamicObject
    {
        private JsonValue _data;

        public DynamicJsonValueWrapper(JsonValue jsonValue)
        {
            _data = jsonValue;
        }

        public DynamicJsonValueWrapper(IJsonDocument jsonDocument)
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

            result = Client.getGenericJsonValue(_data, propertyName);

            return true;
        }


    }
}
