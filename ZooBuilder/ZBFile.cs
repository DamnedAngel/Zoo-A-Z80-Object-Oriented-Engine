using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooBuilder
{
    class ZBFile
    {
        public ZBPath Path { get; private set; }
        public string AbsolutePath { get { return Path.AbsolutePath; } }

        private string _fileName = "";
        private string _extension = "";

        public string FileName
        {
            private set
            {
                if (value == null)
                {
                    _fileName = "";
                }
                else
                {
                    _fileName = value;
                }
            }
            get
            {
                return _fileName;
            }
        }
        public string Extension
        {
            private set
            {
                if (value == null)
                {
                    _extension = "";
                }
                else
                {
                    _extension = value;
                }
            }
            get
            {
                return _extension;
            }
        }

        public string FullFileName
        {
            get
            {
                if (Extension.Length > 0)
                {
                    return FileName + "." + Extension;
                }
                else
                {
                    return FileName;
                }
            }
            private set
            {
                var parts = value.Split('.');
                if (parts.Length > 1)
                {
                    Extension = parts.Last();
                    FileName = value.Substring(0, value.Length - Extension.Length - 1);
                }
                else
                {
                    Extension = "";
                    FileName = value;
                }
            }
        }


        public string URI
        {
            get
            {
                return System.IO.Path.Combine(AbsolutePath, FullFileName).Replace('\\', '/');
            }

        }

        private string SetFileNameAndReturnPath (string URI)
        {
            string result;
            var _URI = URI.Replace('\\', '/');

            var parts = _URI.Split('/');
            if (parts.Length > 1)
            {
                FullFileName = parts.Last();
                result = _URI.Substring(0, _URI.Length - FullFileName.Length - 1);
            }
            else
            {
                FullFileName = _URI;
                result = ".";
            }
            return result;
        }

        public bool Exists { get { return System.IO.File.Exists(URI); } }

        public ZBFile(string fileName, string extension, string path, ZBPath referencePath)
        {
            FileName = fileName;
            Extension = extension;
            Path = new ZBPath(path, referencePath);
        }

        public ZBFile(string fullFileName, string path, ZBPath referencePath)
        {
            FullFileName = fullFileName;
            Path = new ZBPath(path, referencePath);
        }

        public ZBFile(string URI, ZBPath referencePath)
        {
            var path = SetFileNameAndReturnPath(URI);
            Path = new ZBPath(path, referencePath);
        }

        public ZBFile(string URI)
        {
            var path = SetFileNameAndReturnPath(URI);
            Path = new ZBPath(path, null);
        }
    }
}
