using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace antivirusProject
{
    public class FileFormat // tarama için gerekli dosya formatları
    {
        public string FName { get; set; }
        public string MD5 { get; set; }

        public FileFormat(string fName, string md5)
        {
            FName = fName;
            MD5 = md5;
        }
    }
}
