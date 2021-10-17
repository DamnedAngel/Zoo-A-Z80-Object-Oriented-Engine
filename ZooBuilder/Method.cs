using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooBuilder
{
    class Method : Member
    {
        private bool _override;
        public bool Override { get { return IsInherited ? ((Method)ParentMember).Override : _override; } set { _override = value; } }

        private bool _abstract;
        public bool Abstract { get { return IsInherited ? ((Method)ParentMember).Abstract : _abstract; } set { _abstract = value; } }

        private bool _final;
        public bool Final { get { return IsInherited ? ((Method)ParentMember).Final : _final; } set { _final = value; } }

        private string _body;
        public string Body
        {
            get { return _body; }
            protected set
            {
                var i = 0;
                bool foundStart = false;
                while ((i < value.Length) && (!foundStart))
                {
                    if ((value[i] != ' ') && (value[i] != '\t') && (value[i] != '\n'))
                    {
                        i = 0;
                        foundStart = true;
                    }
                    else
                    {
                        if (value[i] == '\n')
                        {
                            foundStart = true;
                        }
                        i++;
                    }
                }

                if (i >= value.Length)
                {
                    _body = "";
                }
                else
                {
                    _body = value.Substring(i);
                }
            }
        }

        public Method(ZooClass baseClass, string name, Member.Scopes scope, bool @static, bool @override, bool @abstract, bool @final, string body)
        {
            InitMember(baseClass, name, scope, @static, 1, 2);
            Override = @override;
            Abstract = @abstract;
            Final = @final;
            Body = body;
            Size = 2;
        }

        public Method(ZooClass baseClass, string name, string scope, bool @static, bool @override, bool @abstract, bool @final, string body)
        {
            InitMember(baseClass, name, scope, @static, 1, 2);
            Override = @override;
            Abstract = @abstract;
            Final = @final;
            Body = body;
            Size = 2;
        }

        public Method(Method parentMethod)
        {
            InitMember(parentMethod);
        }
    }
}
