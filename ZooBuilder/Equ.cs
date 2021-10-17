using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooBuilder
{
    class Equ
    {
        public string Name { get; set; }

        private Member.Scopes _scope;
        public Member.Scopes Scope { get { return _scope; } protected set { _scope = value; } }
        public string ScopeAsString { get { return Scope.ToString().ToLower(); } protected set { Enum.TryParse(value, true, out _scope); } }
        public bool IsPrivate { get { return Scope == Member.Scopes.@private; } }
        public bool IsProtected { get { return Scope == Member.Scopes.@protected; } }
        public bool IsPublic { get { return Scope == Member.Scopes.@public; } }

        public String Value { get; set; }

        public Equ(String name, Member.Scopes scope, String value)
        {
            Name = name;
            Scope = scope;
            Value = value;
        }

        public Equ(String name, String scope, String value)
        {
            Name = name;
            Enum.TryParse(scope, true, out _scope);
            Value = value;
        }
    }

}
