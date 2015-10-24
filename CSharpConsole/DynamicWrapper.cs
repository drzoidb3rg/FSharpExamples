using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSharp.Data;
using FSharp.Data.Runtime.BaseTypes;

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

            var propertyName = binder.Name.ToLower();
            
            result = WorldBankProvider.getGeneric(_data, propertyName);

            return true;
        }


    }
}
