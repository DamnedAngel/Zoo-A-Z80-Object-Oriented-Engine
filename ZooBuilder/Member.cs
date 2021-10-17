using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooBuilder
{
    abstract class Member
    {
        public enum Scopes : uint
        {
            @private, @protected, @public
        }

        public Member ParentMember { get; protected set; }
        public bool IsInherited { get { return ParentMember != null; } }

        private ZooClass _baseClass;
        public ZooClass BaseClass { get { return IsInherited ? ParentMember.BaseClass : _baseClass; } protected set { _baseClass = value; } }

        private string _name;
        public string Name { get { return IsInherited ? ParentMember.Name : _name; } protected set { _name = value; } }

        private Scopes _scope;
        public Scopes Scope { get { return IsInherited ? ParentMember.Scope : _scope; } protected set { _scope = value; } }
        public string ScopeAsString { get { return Scope.ToString().ToLower(); } protected set { Enum.TryParse(value, true, out _scope); } }
        public bool IsPrivate { get { return Scope == Scopes.@private; } }
        public bool IsProtected { get { return Scope == Scopes.@protected; } }
        public bool IsPublic { get { return Scope == Scopes.@public; } }

        private bool _static;
        public bool Static { get { return IsInherited ? ParentMember.Static : _static; } set { _static = value; } }

        private UInt16 _size;
        public UInt16 Size { get { return IsInherited ? ParentMember.Size : _size; } protected set { _size = value; } }

        private UInt16 _count;
        public UInt16 Count { get { return IsInherited ? ParentMember.Count : _count; } protected set { _count = value; } }
        public UInt16 TotalSize { get { return (UInt16)(Count * Size); } }

        private Int16 _offset;
        public Int16 Offset { get { return IsInherited ? ParentMember.Offset : _offset; } set { _offset = value; } }

        protected void InitMember(ZooClass baseClass, string name, Member.Scopes scope, bool @static, UInt16 count, UInt16 size)
        {
            ParentMember = null;
            BaseClass = baseClass;
            Name = name;
            Scope = scope;
            Static = @static;
            Count = count;
            Size = size;
        }

        protected void InitMember(ZooClass baseClass, string name, string scope, bool @static, UInt16 count, UInt16 size)
        {
            ParentMember = null;
            BaseClass = baseClass;
            Name = name;
            ScopeAsString = scope;
            Static = @static;
            Count = count;
            Size = size;
        }

        protected void InitMember(Member parentMember)
        {
            ParentMember = parentMember;
        }
    }
}
