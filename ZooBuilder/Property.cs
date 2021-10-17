using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooBuilder
{
    class Property : Member
    {
        private string _type;
        public string Type { get { return IsInherited ? ((Property)ParentMember).Type : _type; } protected set { _type = value; } }
        public bool IsByte { get { return Type.CompareTo("@byte") == 0; } }

        public string Value { get; set; }

        public Property(ZooClass baseClass, string name, Member.Scopes scope, bool @static, UInt16 count, UInt16 size, string type, string value)
        {
            InitMember(baseClass, name, scope, @static, count, size);
            Value = value;
            Type = type;
        }

        public Property(ZooClass baseClass, string name, string scope, bool @static, UInt16 count, UInt16 size, string type, string value)
        {
            InitMember(baseClass, name, scope, @static, count, size);
            Value = value;
            Type = type;
        }

        public Property(Property parentProperty)
        {
            InitMember(parentProperty);
        }
    }
}
