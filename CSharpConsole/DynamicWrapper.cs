﻿using System.Dynamic;
using FSharp.Data.Runtime.BaseTypes;
using FSharp.Data;

namespace CSharpConsole
{
    public class DynamicJsonWrapper : System.Dynamic.DynamicObject
    {
        private IJsonDocument _data;


        public DynamicJsonWrapper(IJsonDocument data)
        {
            _data = data;
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

            result = Client.getGenericJsonValue(_data.JsonValue, propertyName);

            return true;
        }

    }
}
