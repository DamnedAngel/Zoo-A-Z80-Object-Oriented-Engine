using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooBuilder
{
    class ZBError
    {
        public enum Errors : uint
        {
            SUCCESS                     = 0x00000000,

            INVALIDREFLECTIONLEVEL      = 0x00000001,
            INVALIDSCHEMA               = 0x00000002,
            INVALIDXMLFILENAME          = 0x00000003,
            INVALIDXSDFILENAME          = 0x00000004,
            INVALIDXML                  = 0x00000005,
            GENERALXMLVALERROR          = 0x00000006,
            CLASSNAMEMISMATCH           = 0x00000101,
            CLASSREDEFINITION           = 0x00000102,
            DUPLICATEEQU                = 0x00000103,
            DUPLICATEREFERENCE          = 0x00000104,
            MISSINGDEPENDENCY           = 0x00000105,
            CIRCULARINCLUSION           = 0x00000106,
            OVERRIDEFAIL                = 0x00000107,
            PROPERTYREDEFINITION        = 0x00000108,
            MISSINGOVERRIDE             = 0x00000109,
            PRIVATEMETHODREDEFINITION   = 0x0000010A,
            FINALMETHODREDEFINITION     = 0x0000010B,
            FINALABSTRACTMETHOD         = 0x0000010C,
            METHODREDEFININEDASABSTRACT = 0x0000010D,
            METHODSCOPEREDEFINITION     = 0x0000010E,
            METHODSTATICREDEFINITION    = 0x0000010F,
            NULLBASECLASS               = 0x00000110,
            NULLPARENTCLASS             = 0x00000111,
            MEMBERINDEXOVERFLOW         = 0x00000112,
        }

        private static Errors err = 0;
        public static Errors Error { get { return err; } set { err = value; } }
        public static bool IsError { get { return err!=Errors.SUCCESS; }}
    }
}
