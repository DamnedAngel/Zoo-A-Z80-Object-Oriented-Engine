using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooBuilder
{
    class ZBPath
    {
        public string Path { private set; get; }
        public ZBPath ReferencePath { private set; get; }
        public string AbsolutePath { private set; get; }

        public ZBPath (string path, ZBPath parentPath)
        {
            if (path == null)
            {
                Path = ".";
            }
            else
            {
                Path = path.Replace('\\', '/');
            }
            ReferencePath = parentPath;
            if ((ReferencePath == null) || ((Path.Length > 1) && (Path[1]==':')))
            {
                AbsolutePath = System.IO.Path.GetFullPath(Path).Replace('\\', '/');
            }
            else
            {
                AbsolutePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(ReferencePath.AbsolutePath, Path)).Replace('\\', '/');
            }
        }
    }
}
