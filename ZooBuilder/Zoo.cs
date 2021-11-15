using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooBuilder
{
    class Zoo
    {
        public enum ReflectionLevels : uint
        {
            none, inheritance, namedInheritance, classStructure, namedMembers
        }
        private ReflectionLevels _reflectionLevel;
        public ReflectionLevels ReflectionLevel { get { return _reflectionLevel; } set { _reflectionLevel = value; } }
        public string ReflectionLevelAsString { get { return ReflectionLevel.ToString().ToLower(); }
            set
            {
                if (!Enum.TryParse(value, true, out _reflectionLevel))
                {
                    ZBError.Error = ZBError.Errors.INVALIDREFLECTIONLEVEL;
                }
            }
        }
        public string ReflectionLevelName
        {
            get
            {
                return "reflection.r" + (int)ReflectionLevel + "_" + ReflectionLevel.ToString();
            }
        }
        public bool NoReflection { get { return (ReflectionLevel >= ReflectionLevels.none); } }
        public bool InheritanceReflection { get { return (ReflectionLevel >= ReflectionLevels.inheritance); } }
        public bool NamedInheritanceReflection { get { return (ReflectionLevel >= ReflectionLevels.namedInheritance); } }
        public bool ClassStructureReflection { get { return ReflectionLevel >= ReflectionLevels.classStructure; } }
        public bool NamedMembersReflection { get { return ReflectionLevel >= ReflectionLevels.namedMembers; } }

        private ZBPath _path;
        public string Path { get { return _path.AbsolutePath; } set { _path = new ZBPath(value, null); } }
        public string EnginePath { get { return _path.AbsolutePath + "/engine"; }}
        public string FrameworkPath { get { return _path.AbsolutePath + "/framework"; } }

        public bool Allow(string reflectionLevel)
        {
            if (!Enum.TryParse(reflectionLevel, true, out ReflectionLevels _reflectionLevel))
            {
                ZBError.Error = ZBError.Errors.INVALIDREFLECTIONLEVEL;
                return false;
            }
            return ReflectionLevel >= _reflectionLevel; 
        }

        public string OutputPath { get; set; }
        public string OutputFile { get; set; }
        public bool IncludeAncestors { get; set; }
    }
}
