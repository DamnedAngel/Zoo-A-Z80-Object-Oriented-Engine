using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ZooBuilder
{
    class ZooClass
    {
        public static IDictionary<string, ZooClass> zooClasses = new Dictionary<string, ZooClass>();
        private static List<string> womb = new List<string>();

        public OrderedDictionary ObjectReflectionData { get; set; }
        public OrderedDictionary ClassReflectionData { get; set; }

        private ZBFile ClassFile { get; set; }

        public string Name { get { return ClassFile.FileName; } }
        public string AbsolutePath { get { return ClassFile.AbsolutePath; } }
        public string FileName { get { return ClassFile.FullFileName; } }
        public string URI { get { return ClassFile.URI; } }

        public ZooClass ParentClass { get; set; }
        public IDictionary<string, ZBFile> Dependencies { get; set; }
        public IDictionary<string, ZBFile> ZIncludes { get; set; }
        public IDictionary<string, Equ> Equs { get; set; }

        public OrderedDictionary Properties { get; set; }
        public OrderedDictionary ExtendedProperties { get; set; }
        public OrderedDictionary Methods { get; set; }
        public OrderedDictionary ExtendedMethods { get; set; }
        public OrderedDictionary SProperties { get; set; }
        public OrderedDictionary ExtendedSProperties { get; set; }
        public OrderedDictionary SMethods { get; set; }
        public OrderedDictionary ExtendedSMethods { get; set; }
        public string PublicPropertyList
        {
            get
            {
                string result = "";
                foreach (DictionaryEntry prop in Properties)
                {
                    Property p = (Property) (prop.Value);
                    if (p.IsPublic)
                    {
                        if (result.Length > 0)
                        {
                            result += ", ";
                        }
                        result += p.Name;
                    }
                }
                return result;
            }
        }
        public bool HasPublicProperties { get { return PublicPropertyList.Length > 0; } }
        public UInt16 ObjectSize { get { return (UInt16)(getMemberListSize(Properties) + getMemberListSize(ExtendedProperties) + getMemberListSize(ObjectReflectionData)); } }

        public void SetReflectionProperties(Zoo.ReflectionLevels reflectionLevel)
        {
            ObjectReflectionData = new OrderedDictionary();
            ClassReflectionData = new OrderedDictionary();

            if (reflectionLevel >= Zoo.ReflectionLevels.inheritance)
            {
                addMember (new Property(this, "pClass", Member.Scopes.@public, false, 1, 2, "@pointer", ".dw " + Name), ObjectReflectionData, null, null);

                addMember (new Property(this, "pParentClass", Member.Scopes.@public, true, 1, 2, "@pointer", ".dw #0"), ClassReflectionData, null, null);       // to be updated after class building
                addMember (new Property(this, "pSProperties", Member.Scopes.@public, true, 1, 2, "@pointer", ".dw " + Name + "._SProperties"), ClassReflectionData, null, null);
                addMember (new Property(this, "pSMethods", Member.Scopes.@public, true, 1, 2, "@pointer", ".dw " + Name + "._SMethods"), ClassReflectionData, null, null);
                addMember (new Property(this, "ObjectSize", Member.Scopes.@public, true, 1, 1, "@byte", ".db #0"), ClassReflectionData, null, null);         // to be updated after class building

                if (reflectionLevel >= Zoo.ReflectionLevels.namedInheritance)
                {
                    addMember (new Property(this, "pName", Member.Scopes.@public, false, 1, 2, "@pointer", ".dw object'.Name"), ObjectReflectionData, null, null);

                    addMember (new Property(this, "pName", Member.Scopes.@public, true, 1, 2, "@pointer", ".dw " + Name + ".Name"), ClassReflectionData, null, null);


                    if (reflectionLevel >= Zoo.ReflectionLevels.classStructure)
                    {
                        addMember (new Property(this, "pSisterClass", Member.Scopes.@public, true, 1, 2, "@pointer", ".dw #0"), ClassReflectionData, null, null);
                        addMember (new Property(this, "pChildChain", Member.Scopes.@public, true, 1, 2, "@pointer", ".dw #0"), ClassReflectionData, null, null);
                                            
                        if (reflectionLevel >= Zoo.ReflectionLevels.namedMembers)
                        {
                            addMember (new Property(this, "PropertiesCount", Member.Scopes.@public, true, 1, 1, "@byte", ".db #0"), ClassReflectionData, null, null);
                            addMember (new Property(this, "MethodsCount", Member.Scopes.@public, true, 1, 1, "@byte", ".db #0"), ClassReflectionData, null, null);
                            addMember (new Property(this, "SPropertiesCount", Member.Scopes.@public, true, 1, 1, "@byte", ".db #0"), ClassReflectionData, null, null);
                            addMember (new Property(this, "SMethodsCount", Member.Scopes.@public, true, 1, 1, "@byte", ".db #0"), ClassReflectionData, null, null);
                            addMember (new Property(this, "pPropertiesNames", Member.Scopes.@public, true, 1, 2, "@pointer", ".dw " + Name + "._PropertiesNames"), ClassReflectionData, null, null);
                            addMember (new Property(this, "pSPropertiesNames", Member.Scopes.@public, true, 1, 2, "@pointer", ".dw " + Name + "._SPropertiesNames"), ClassReflectionData, null, null);
                        }
                    }
                }
            }
        }


        public void UpdateReflectionProperties(Zoo.ReflectionLevels reflectionLevel)
        {
            if (reflectionLevel >= Zoo.ReflectionLevels.inheritance)
            {
                if (ParentClass != null)
                {
                    ((Property) ClassReflectionData["pParentClass"]).Value = ".dw " + ParentClass.Name;
                }

                ((Property) ClassReflectionData["ObjectSize"]).Value = ".db #" + ObjectSize.ToString();

                if (reflectionLevel >= Zoo.ReflectionLevels.namedInheritance)
                {
                    if (reflectionLevel >= Zoo.ReflectionLevels.classStructure)
                    {
                        if (reflectionLevel >= Zoo.ReflectionLevels.namedMembers)
                        {
                            ((Property) ClassReflectionData["PropertiesCount"]).Value = ".db #" + Properties.Count;
                            ((Property) ClassReflectionData["MethodsCount"]).Value = ".db #" + Methods.Count;
                            ((Property) ClassReflectionData["SPropertiesCount"]).Value = ".db #" + SProperties.Count;
                            ((Property) ClassReflectionData["SMethodsCount"]).Value = ".db #" + SMethods.Count;
                        }
                    }
                }
            }
        }

        //Display any warnings or errors.
        private static void XMLValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                ZBConsole.Print("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
                ZBError.Error = ZBError.Errors.INVALIDSCHEMA;
            }
            else
            {
                ZBConsole.Print("\tValidation error: " + args.Message);
                ZBError.Error = ZBError.Errors.INVALIDXML;
            }

        }

        private static UInt16 getMemberListSize (OrderedDictionary memberList)
        {
            UInt16 result = 0;
            foreach (DictionaryEntry m in memberList)
            {
                if (m.Value is Method)
                {
                    result += ((Method)m.Value).Size;
                }
                else
                {
                    UInt16 s = ((Property)m.Value).Size;
                    UInt16 c = ((Property)m.Value).Count;
                    result += (UInt16) (s * c);
                }
            }
            return result;
        }


        public static ZooClass getZooClass(string fileName, Zoo zoo)
        {
            string currentPath = Path.GetDirectoryName(fileName);
            string className = Path.GetFileNameWithoutExtension(fileName);
            return getZooClass(className, currentPath, null, zoo, "");
        }

        private static ZooClass getZooClass(string name, string classPath, ZBPath referencePath, Zoo zoo, string logPrefix)
        {
            if (zooClasses.ContainsKey(name))
            {
                ZBConsole.Debug("{0}Class {1} found.", logPrefix, name);
                return zooClasses[name];
            }
            else
            {
                return new ZooClass(name, classPath, referencePath, zoo, logPrefix);
            }
        }

        private XDocument getDocument(Zoo zoo, string logPrefix)
        {
            XDocument result = null;
            if (! ClassFile.Exists)
            {
                ZBConsole.Print("{0}Invalid XML file name: {1} ", logPrefix, URI);
                ZBError.Error = ZBError.Errors.INVALIDXMLFILENAME;
            }
            else
            {
                ZBConsole.Debug("{0}XML file found.", logPrefix);
                var schemaFile = new ZBFile("zoo.class.xsd", zoo.FrameworkPath, null);
                if (! schemaFile.Exists)
                {
                    ZBConsole.Print("{0}Invalid XSD file name: {1} ", logPrefix, schemaFile.URI);
                    ZBError.Error = ZBError.Errors.INVALIDXSDFILENAME;
                }
                else
                {
                    ZBConsole.Debug("{0}XSD file found.", logPrefix);
                    try
                    {
                        XMLValidator.Validate(XMLValidationCallBack, URI, schemaFile.URI);
                    }
                    catch (Exception e)
                    {
                        ZBConsole.Print("{0}Error validating XML: {1} ", logPrefix, e.Message);
                        ZBError.Error = ZBError.Errors.GENERALXMLVALERROR;
                    }
                    if (!ZBError.IsError)
                    {
                        if (!ZBError.IsError)
                        {
                            ZBConsole.Debug("{0}XML Validated against schema.", logPrefix);

                            result = XDocument.Load(URI);
                            var fName = result.Root.Attribute("name").Value;
                            if (fName.CompareTo(Name) != 0)
                            {
                                ZBError.Error = ZBError.Errors.CLASSNAMEMISMATCH;
                                ZBConsole.Print("{0}Class name mismatch: file name = {1}, XML = {2}", logPrefix, Name, fName);
                            }
                            else
                            {
                                ZBConsole.Debug("{0}[{1}]Class name validated.", logPrefix, Name);

                                if (zooClasses.ContainsKey(Name))
                                {
                                    ZBError.Error = ZBError.Errors.CLASSREDEFINITION;
                                    ZBConsole.Print("{0}[{1}]Class redefinition.", logPrefix, Name);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private IDictionary<string, ZBFile> getReferences(XDocument document, string tag, bool allowDuplicates, Zoo zoo, string logPrefix)
        {
            IDictionary<string, ZBFile> result = new Dictionary<string, ZBFile>();
            var references = document.Root.Descendants(tag);
            foreach (var reference in references)
            {
                var name = reference.Value;
                var path = reference.Attribute("path");
                var pathStr = (path != null) ? path.Value : ".";

                if (!allowDuplicates && result.ContainsKey(name))
                {
                    ZBError.Error = ZBError.Errors.DUPLICATEREFERENCE;
                    ZBConsole.Print("{0}[{1}]Duplicate reference({2}) {3}.", logPrefix, Name, tag, name);
                    break;
                }
                else
                {
                    var file = new ZBFile(name, pathStr, ClassFile.Path);
                    if (! file.Exists)
                    {
                        ZBConsole.Debug("{0}[{1}]WARNING: Reference({2}) file {3} does not exist!", logPrefix, Name, tag, name);
                    }
                    result.Add(name, file);
                    ZBConsole.Debug("{0}[{1}]Added reference({2}) {3}.", logPrefix, Name, tag, name);
                }
            }
            return result;
        }

        private IDictionary<string, Equ> getEqus(XDocument document, Zoo zoo, string logPrefix)
        {
            IDictionary<string, Equ> result = new Dictionary<string, Equ>();
            var equs = document.Root.Descendants("equ");
            foreach (var equ in equs)
            {
                var name = equ.Attribute("name").Value;
                var scope = equ.Attribute("scope");
                var scopeStr = (scope != null) ? scope.Value : "Private";
                if (result.ContainsKey(name))
                {
                    ZBError.Error = ZBError.Errors.DUPLICATEEQU;
                    ZBConsole.Print("{0}[{1}]Duplicate EQU {2}.", logPrefix, Name, name);
                    break;
                }
                else
                {
                    result.Add(name, new Equ(name, scopeStr, equ.Value));
                    ZBConsole.Debug("{0}[{1}]Added EQU {2}.", logPrefix, Name, name);
                }
            }
            return result;
        }

        private void ProcessParent (XDocument document, Zoo zoo, string logPrefix)
        {
            ParentClass = null;
            var parents = document.Root.Descendants("parent");
            if (parents.Count() == 0)
            {
                if (Name.CompareTo("Object") == 0)
                {
                    ZBConsole.Debug("{0}[{1}]Base Object class detected. No parent applicable.", logPrefix, Name);
                }
                else
                {
                    ZBConsole.Debug("{0}[{1}]Undefined parent. Zoo's Object class assumed.", logPrefix, Name);
                    ParentClass = getZooClass("Object", zoo.FrameworkPath, null, zoo, logPrefix);
                }
            }
            else
            {
                XElement parent = parents.ElementAt(0);
                var parentClassName = parent.Value;
                ZBConsole.Debug("{0}[{1}]Parent class {2} detected.", logPrefix, Name, parentClassName);
                var path = parent.Attribute("path");
                if (path == null)
                {
                    ParentClass = getZooClass(parentClassName, null, ClassFile.Path, zoo, logPrefix);
                }
                else
                {
                    ParentClass = getZooClass(parentClassName, path.Value, ClassFile.Path, zoo, logPrefix);
                }
            }

            if ((! ZBError.IsError) && (ParentClass != null))
            {
                foreach (DictionaryEntry de in ParentClass.Properties)
                {
                    Property p = (Property)de.Value;
                    addProperty(new Property(p));
                    ZBConsole.Debug("{0}[{1}]Parent property {2} inherited.", logPrefix, Name, p.Name);
                }
                foreach (DictionaryEntry de in ParentClass.ExtendedProperties)
                {
                    Property p = (Property)de.Value;
                    addProperty(new Property(p));
                    ZBConsole.Debug("{0}[{1}]Parent property {2} inherited.", logPrefix, Name, p.Name);
                }

                foreach (DictionaryEntry de in ParentClass.Methods)
                {
                    Method m = (Method)de.Value;
                    addMethod(new Method(m));
                    ZBConsole.Debug("{0}[{1}]Parent method {2} inherited.", logPrefix, Name, m.Name);
                }
                foreach (DictionaryEntry de in ParentClass.ExtendedMethods)
                {
                    Method m = (Method)de.Value;
                    addMethod(new Method(m));
                    ZBConsole.Debug("{0}[{1}]Parent method {2} inherited.", logPrefix, Name, m.Name);
                }

                foreach (DictionaryEntry de in ParentClass.SProperties)
                {
                    Property p = (Property)de.Value;
                    addProperty(new Property(p));
                    ZBConsole.Debug("{0}[{1}]Parent sproperty {2} inherited.", logPrefix, Name, p.Name);
                }
                foreach (DictionaryEntry de in ParentClass.ExtendedSProperties)
                {
                    Property p = (Property)de.Value;
                    addProperty(new Property(p));
                    ZBConsole.Debug("{0}[{1}]Parent sproperty {2} inherited.", logPrefix, Name, p.Name);
                }

                foreach (DictionaryEntry de in ParentClass.SMethods)
                {
                    Method m = (Method)de.Value;
                    addMethod(new Method(m));
                    ZBConsole.Debug("{0}[{1}]Parent smethod {2} inherited.", logPrefix, Name, m.Name);
                }
                foreach (DictionaryEntry de in ParentClass.ExtendedSMethods)
                {
                    Method m = (Method)de.Value;
                    addMethod(new Method(m));
                    ZBConsole.Debug("{0}[{1}]Parent smethod {2} inherited.", logPrefix, Name, m.Name);
                }

                ZBConsole.Debug("{0}[{1}]Ended processing parent class {2}.", logPrefix, Name, ParentClass.Name);
            }
        }

        public void addMember(Member m, OrderedDictionary main, OrderedDictionary extended, OrderedDictionary reflectionData)
        {
            if (m.IsInherited)
            {
                if (m.Offset >= 0)
                {
                    main.Add(m.Name, m);
                }
                else
                {
                    extended.Add(m.Name, m);
                }
            }
            else
            {
                OrderedDictionary dest = main;
                m.Offset = (Int16)(getMemberListSize(main) + (reflectionData == null ? 0 : getMemberListSize(reflectionData)));
                if (((extended != null) && (extended.Count > 0)) || (m.Offset + m.TotalSize > 127))
                {
                    dest = extended;
                    m.Offset = (Int16)(-getMemberListSize(extended) - m.TotalSize);
                }

                if (m.Offset < -128)
                {
                    ZBError.Error = ZBError.Errors.MEMBERINDEXOVERFLOW;
                }
                else
                {
                    dest.Add(m.Name, m);
                }
            }
        }

        public void addProperty(Property property)
        {
            if (property.Static)
            {
                addMember(property, SProperties, ExtendedSProperties, null);
            }
            else
            {
                addMember(property, Properties, ExtendedProperties, ObjectReflectionData);
            }
        }

        public void addMethod(Method method)
        {
            if (method.Static)
            {
                addMember(method, SMethods, ExtendedSMethods, null);
            }
            else
            {
                addMember(method, Methods, ExtendedMethods, ClassReflectionData);
            }
        }

        public void overrideMethod(Method method)
        {
            var dest = Methods;
            var extDest = ExtendedMethods;
            if (method.Static)
            {
                dest = SMethods;
                extDest = ExtendedSMethods;
            }

            if (dest.Contains(method.Name))
            {
                method.Offset = ((Method)dest[method.Name]).Offset;
                dest[method.Name] = method;
            }
            else if (extDest.Contains(method.Name))
            {
                method.Offset = ((Method)extDest[method.Name]).Offset;
                extDest[method.Name] = method;
            }
            else
            {
                ZBError.Error = ZBError.Errors.OVERRIDEFAIL;
            }
        }

        private void ProcessProperties(XDocument document, Zoo zoo, string logPrefix)
        {
            var properties = document.Root.Descendants("property");
            foreach (var property in properties)
            {
                var name = property.Attribute("name").Value;
                if (Properties.Contains(name))
                {
                    ZBError.Error = ZBError.Errors.PROPERTYREDEFINITION;
                    ZBConsole.Print("{0}[{1}]Property {2} redefinition.", logPrefix, Name, name);
                    break;
                }
                else
                {
                    UInt16 count = 1;
                    var pcount = property.Attribute("count");
                    if (pcount != null)
                    {
                        count = UInt16.Parse(pcount.Value);
                    }
                    UInt16 size = 2;
                    var type = property.Attribute("type").Value;
                    if (type.CompareTo("@byte") == 0)
                    {
                        size = 1;
                    }
                    else if ((type.CompareTo("@word") != 0) && (type.CompareTo("@pointer") != 0))
                    {

                        if (womb.Contains(type))
                        {
                            ZBError.Error = ZBError.Errors.CIRCULARINCLUSION;
                            ZBConsole.Print("{0}[{1}]Circular inclusion involving class {2}.", logPrefix, Name, type);
                            break;
                        }
                        else
                        {
                            if (!Dependencies.ContainsKey(type))
                            {
                                ZBError.Error = ZBError.Errors.MISSINGDEPENDENCY;
                                ZBConsole.Print("{0}[{1}]Missing dependency {2}.", logPrefix, Name, type);
                                break;
                            }
                            else
                            {
                                var dependency = Dependencies[type];
                                var propClass = getZooClass(dependency.FileName, dependency.AbsolutePath, null, zoo, logPrefix);
                                size = propClass.ObjectSize;
                            }
                        }
                    }
                    string scope = "Private";
                    var pscope = property.Attribute("scope");
                    if (pscope != null)
                    {
                        scope = pscope.Value;
                    }
                    bool @static = false;
                    var pstatic = property.Attribute("static");
                    if (pstatic != null)
                    {
                        @static = bool.Parse(pstatic.Value);
                    }
                    addProperty(new Property(this, name, scope, @static, count, size, type, null));
                }
            }
        }

        private void ProcessMethods(XDocument document, Zoo zoo, string logPrefix)
        {
            var methods = document.Root.Descendants("method");
            foreach (var method in methods)
            {
                var name = method.Attribute("name").Value;
                Method parentMethod = null;
                if (!(ParentClass is null))
                {
                    parentMethod = (Method)ParentClass.Methods[name];
                }

                bool @override = false;
                var poverride = method.Attribute("override");
                if (poverride != null)
                {
                    @override = bool.Parse(poverride.Value);
                    if (@override && parentMethod == null)
                    {
                        ZBError.Error = ZBError.Errors.OVERRIDEFAIL;
                        ZBConsole.Print("{0}[{1}]No method {2} to override in parent Class.", logPrefix, Name, name);
                        break;
                    }
                }
                bool @abstract = false;
                var pabstract = method.Attribute("abstract");
                if (pabstract != null)
                {
                    @abstract = bool.Parse(pabstract.Value);
                }

                bool @final = false;
                var pfinal = method.Attribute("final");
                if (pfinal != null)
                {
                    @final = bool.Parse(pfinal.Value);
                }

                string scope = "Private";
                var pscope = method.Attribute("scope");
                if (pscope != null)
                {
                    scope = pscope.Value;
                }
                bool @static = false;
                var pstatic = method.Attribute("static");
                if (pstatic != null)
                {
                    @static = bool.Parse(pstatic.Value);
                }
                string body = method.Value;

                if (parentMethod != null)
                {
                    if (parentMethod.Scope == Member.Scopes.@private)
                    {
                        ZBError.Error = ZBError.Errors.PRIVATEMETHODREDEFINITION;
                        ZBConsole.Print("{0}[{1}]Private method {2} redefinition.", logPrefix, Name, name);
                        break;
                    }
                    else
                    {
                        if (parentMethod.Final)
                        {
                            ZBError.Error = ZBError.Errors.FINALMETHODREDEFINITION;
                            ZBConsole.Print("{0}[{1}]Final method {2} redefinition.", logPrefix, Name, name);
                            break;
                        }
                        else
                        {
                            if (!@override)
                            {
                                ZBError.Error = ZBError.Errors.MISSINGOVERRIDE;
                                ZBConsole.Print("{0}[{1}]Missing override in method {2} redefinition.", logPrefix, Name, name);
                                break;
                            }
                            else
                            {
                                if (@abstract)
                                {
                                    ZBError.Error = ZBError.Errors.METHODREDEFININEDASABSTRACT;
                                    ZBConsole.Print("{0}[{1}]Method {2} redefined as abstract.", logPrefix, Name, name);
                                    break;
                                }
                                else
                                {
                                    if (scope.ToString().CompareTo(parentMethod.Scope.ToString()) != 0)
                                    {
                                        ZBError.Error = ZBError.Errors.METHODSCOPEREDEFINITION;
                                        ZBConsole.Print("{0}[{1}]Method {2}'s scope redefinition.", logPrefix, Name, name);
                                        break;
                                    }
                                    else
                                    {
                                        if (@static.ToString().CompareTo(parentMethod.Static.ToString()) != 0)
                                        {
                                            ZBError.Error = ZBError.Errors.METHODSTATICREDEFINITION;
                                            ZBConsole.Print("{0}[{1}]Static method {2} redefined as non-static (or vice-versa).", logPrefix, Name, name);
                                            break;
                                        }
                                        else
                                        {
                                            overrideMethod(new Method(this, name, scope, @static, @override, @abstract, @final, body));
                                            if (ZBError.IsError)
                                            {
                                                ZBConsole.Print("{0}[{1}]Strangely, I lost method {2} to override in parent Class.", logPrefix, Name, name);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (final && @abstract)
                    {
                        ZBError.Error = ZBError.Errors.FINALABSTRACTMETHOD;
                        ZBConsole.Print("{0}[{1}]Abstract method {2} defined as final.", logPrefix, Name, name);
                        break;
                    }
                    else
                    {
                        addMethod(new Method(this, name, scope, @static, @override, @abstract, @final, body));
                    }
                }
            }
        }

        private ZooClass(string name, string path, ZBPath referencePath, Zoo zoo, string logPrefix)
        {
            ClassFile = new ZBFile(name, "xml", path, referencePath);

            ZBConsole.Debug("{0}Processing class {1}... ", logPrefix, Name);
            logPrefix += "  ";
            ZBConsole.Debug("{0}Current path: {1}", logPrefix, AbsolutePath);
            ZBConsole.Debug("{0}Current file: {1}", logPrefix, FileName);

            var document = getDocument(zoo, logPrefix);
            if (!ZBError.IsError)
            {
                ZBConsole.Debug("{0}[{1}]New class.", logPrefix, Name);
                womb.Add(Name);

                Dependencies = getReferences(document, "dependency", false, zoo, logPrefix);
                ZIncludes = getReferences(document, "include", true, zoo, logPrefix);
                Equs = getEqus(document, zoo, logPrefix);

                if (!ZBError.IsError)
                {
                    Properties = new OrderedDictionary();
                    Methods = new OrderedDictionary();

                    Properties = new OrderedDictionary();
                    ExtendedProperties = new OrderedDictionary();
                    Methods = new OrderedDictionary();
                    ExtendedMethods = new OrderedDictionary();
                    SProperties = new OrderedDictionary();
                    ExtendedSProperties = new OrderedDictionary();
                    SMethods = new OrderedDictionary();
                    ExtendedSMethods = new OrderedDictionary();

                    SetReflectionProperties(zoo.ReflectionLevel);

                    ProcessParent(document, zoo, logPrefix);
                    if (!ZBError.IsError)
                    {
                        ProcessProperties(document, zoo, logPrefix);
                        if (!ZBError.IsError)
                        {
                            ProcessMethods(document, zoo, logPrefix);
                            if (!ZBError.IsError)
                            {
                                UpdateReflectionProperties(zoo.ReflectionLevel);
                                womb.Remove(Name);
                                zooClasses.Add(this.Name, this);
                            }
                        }
                    }
                }
            }
        }
    }
}
