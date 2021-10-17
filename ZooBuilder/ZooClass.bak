using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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

        public string Name { get; set; }
        public ZooClass ParentClass { get; set; }
        public IDictionary<string, Equ> Dependencies { get; set; }
        public List<ZInclude> ZIncludes { get; set; }
        public IDictionary<string, Equ> Equs { get; set; }
        public OrderedDictionary Properties { get; set; }
        public OrderedDictionary Methods { get; set; }
        public UInt16 ObjectSize { get; set; }

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


        public static ZooClass getZooClass(string fileName, Zoo zoo)
        {
            string currentPath = Path.GetDirectoryName(fileName);
            string className = Path.GetFileNameWithoutExtension(fileName);
            return getZooClass(className, currentPath, zoo, "");
        }

        private static ZooClass getZooClass(string name, string classPath, Zoo zoo, string logPrefix)
        {
            if (zooClasses.ContainsKey(name))
            {
                ZBConsole.Debug("{0}Class {1} found.", logPrefix, name);
                return zooClasses[name];
            }
            else
            {
                return new ZooClass(name, classPath, zoo, logPrefix);
            }
        }

        private ZooClass(string className, string currentPath, Zoo zoo, string logPrefix)
        {
            ZBConsole.Debug("{0}Processing class {1}... ", logPrefix, className);
            logPrefix += "  ";
            ZBConsole.Debug("{0}Current path: {1}", logPrefix, currentPath);
            var currentFile = className + ".xml";
            ZBConsole.Debug("{0}Current file: {1}", logPrefix, currentFile);
            var fileName = Path.Combine(currentPath, currentFile);

            if (!File.Exists(fileName))
            {
                ZBConsole.Print("{0}Invalid XML file name: {1} ", logPrefix, fileName);
                ZBError.Error = ZBError.Errors.INVALIDXMLFILENAME;
            }
            else
            {
                ZBConsole.Debug("{0}XML file found.", logPrefix);
                string schemaFile = Path.Combine(zoo.Path, "zoo.class.xsd");
                if (!File.Exists(schemaFile))
                {
                    ZBConsole.Print("{0}Invalid XSD file name: {1} ", logPrefix, schemaFile);
                    ZBError.Error = ZBError.Errors.INVALIDXSDFILENAME;
                }
                else
                {
                    ZBConsole.Debug("{0}XSD file found.", logPrefix);
                    try
                    {
                        XMLValidator.Validate(XMLValidationCallBack, fileName, schemaFile);
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

                            var document = XDocument.Load(fileName);
                            Name = document.Root.Attribute("name").Value;
                            if (Name.CompareTo(className) != 0)
                            {
                                ZBError.Error = ZBError.Errors.CLASSNAMEMISMATCH;
                                ZBConsole.Print("{0}Class name mismatch: file name = {1}, XML = {2}", logPrefix, className, Name);
                            }
                            else
                            {
                                ZBConsole.Debug("{0}[{1}]Class name validated.", logPrefix, Name);

                                if (zooClasses.ContainsKey(Name))
                                {
                                    ZBError.Error = ZBError.Errors.CLASSREDEFINITION;
                                    ZBConsole.Print("{0}[{1}]Class redefinition.", logPrefix, Name);
                                }
                                else
                                {
                                    ZBConsole.Debug("{0}[{1}]New class.", logPrefix, Name);
                                    womb.Add(Name);

                                    Properties = new OrderedDictionary();
                                    Methods = new OrderedDictionary();
                                    string parentClassName = null;
                                    string parentClassPath = null;

                                    /*
                                     * Includes
                                     */
                                    ZIncludes = new List<ZInclude>();
                                    var zincludes = document.Root.Descendants("include");
                                    foreach (var zinclude in zincludes)
                                    {
                                        var path = zinclude.Attribute("path");
                                        var pathStr = (path != null) ? path.Value : ".";
                                        ZIncludes.Add(new ZInclude(zinclude.Value, pathStr));
                                        ZBConsole.Debug("{0}[{1}]Added Include {1}.", logPrefix, Name, zinclude.Value);
                                    }

                                    /*
                                     * EQUs
                                     */
                                    Equs = new Dictionary<string, Equ>();
                                    var equs = document.Root.Descendants("equ");
                                    foreach (var equ in equs)
                                    {
                                        var scope = equ.Attribute("scope");
                                        var scopeStr = (scope != null) ? scope.Value : "Private";
                                        var name = equ.Attribute("name").Value;
                                        if (Equs.ContainsKey(name))
                                        {
                                            ZBError.Error = ZBError.Errors.EQUREDEFINITION;
                                            ZBConsole.Print("{0}[{1}]EQU {2} redefinition.", logPrefix, Name, name);
                                            break;
                                        }
                                        else
                                        {
                                            Equs.Add(name, new Equ(name, scopeStr, equ.Value));
                                            ZBConsole.Debug("{0}[{1}]Added EQU {2}.", logPrefix, Name, name);
                                        }
                                    }

                                    if (!ZBError.IsError)
                                    {
                                        /*
                                         * Parent
                                         */
                                        var parents = document.Root.Descendants("parent");
                                        if (parents.Count() == 0)
                                        {
                                            if (Name.CompareTo("Object") == 0)
                                            {
                                                ZBConsole.Debug("{0}[{1}]Base Object class detected. No parent applicable.", logPrefix, Name);
                                            }
                                            else
                                            {
                                                parentClassName = "Object";
                                                parentClassPath = zoo.Path;
                                                ZBConsole.Debug("{0}[{1}]Undefined parent. Zoo's Object class assumed.", logPrefix, Name);
                                            }
                                        }
                                        else
                                        {
                                            XElement parent = parents.ElementAt(0);
                                            parentClassName = parent.Value;
                                            ZBConsole.Debug("{0}[{1}]Parent class {2} detected.", logPrefix, Name, parentClassName);
                                            var path = parent.Attribute("path");
                                            if (path == null)
                                            {
                                                parentClassPath = currentPath;
                                            }
                                            else
                                            {
                                                parentClassPath = path.Value;
                                                if ((parentClassPath.Length < 2) || (parentClassPath[1] != ':'))
                                                {
                                                    parentClassPath = Path.Combine(currentPath, parentClassPath);
                                                }
                                            }
                                        }

                                        ObjectSize = 0;
                                        if (parentClassName != null)
                                        {
                                            ParentClass = getZooClass(parentClassName, parentClassPath, zoo, logPrefix);
                                            if (!ZBError.IsError)
                                            {
                                                foreach (Property p in ParentClass.Properties.Values)
                                                {
                                                    addProperty(p);
                                                    ZBConsole.Debug("{0}[{1}]Parent property {2} inherited.", logPrefix, Name, p.Name);
                                                }
                                                foreach (Method m in ParentClass.Methods.Values)
                                                {
                                                    addMethod(m);
                                                    ZBConsole.Debug("{0}[{1}]Parent method {2} inherited.", logPrefix, Name, m.Name);
                                                }
                                            }
                                        }

                                        if (!ZBError.IsError)
                                        {
                                            ZBConsole.Debug("{0}[{1}]Ended processing parent class {2}.", logPrefix, Name, parentClassName);

                                            /*
                                             * Properties
                                             */
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
                                                                var propClass = getZooClass(parentClassName, parentClassPath, zoo, logPrefix);
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
                                                    addProperty(new Property(name, scope, @static, size));
                                                }
                                            }

                                            if (!ZBError.IsError)
                                            {
                                                ZBConsole.Debug("{0}[{1}]Ended processing parent class {2}.", logPrefix, Name, parentClassName);

                                                /*
                                                 * Method
                                                 */
                                                var methods = document.Root.Descendants("method");
                                                foreach (var method in methods)
                                                {
                                                    var name = method.Attribute("name").Value;
                                                    Method parentMethod = null;
                                                    if (!(ParentClass is null))
                                                    {
                                                        parentMethod = (Method) ParentClass.Methods[name];
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

                                                    if (Methods.Contains(name))
                                                    {
                                                        ZBError.Error = ZBError.Errors.METHODREDEFINITION;
                                                        ZBConsole.Print("{0}[{1}]Method {2} redefinition.", logPrefix, Name, name);
                                                        break;
                                                    }
                                                    else
                                                    {
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

                                                        addMethod(new Method(name, scope, @static, @override, @abstract, @final));
                                                    }
                                                }

                                                if (!ZBError.IsError)
                                                {
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
                }
            }
        }

/*
 *      public ZooClass (string name, ZooClass parentClass)
        {
            Name = name;
            ParentClass = parentClass;
            ObjectSize = 0;
            if (ParentClass!=null)
            {
                foreach (Property p in ParentClass.Properties.Values)
                {
                    addProperty(p);
                }
                foreach (Method m in ParentClass.Methods.Values)
                {
                    addMethod(m);
                }
            }
        }
*/

        public void addMethod(Method method)
        {
            Methods.Add(method.Name, method);
        }

        public void addProperty(Property property)
        {
            property.Offset = ObjectSize;
            Properties.Add(property.Name, property);
            ObjectSize += property.Size;
        }
    }
}
