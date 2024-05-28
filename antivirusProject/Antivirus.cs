using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;


namespace antivirusProject
{
	public interface Antivirus // heuristik ve imza tabanlı tarama için temel çatı sınıf
	{
		public void RunScan();
		public bool IsMalwareFile(string curFile);

    }
}