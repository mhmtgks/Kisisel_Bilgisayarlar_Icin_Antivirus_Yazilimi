using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace antivirusProject
{
    internal class Results
    {
        public List<FileFormat> allSystemFiles { get; set; }
        public List<string> MFinSystem { get; set; }
        public List<FileFormat> MalwareFiles { get; set; }



        public Results(List<FileFormat> allSystemFiles, List<string> MFinSystem, List<FileFormat> MalwareFiles)
        {
            this.allSystemFiles = allSystemFiles;
            this.MFinSystem = MFinSystem;
            this.MalwareFiles = MalwareFiles;
        }

    }
}
