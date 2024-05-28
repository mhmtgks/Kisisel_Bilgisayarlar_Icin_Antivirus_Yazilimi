using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace antivirusProject
{
    internal class signatureAntivirus:Antivirus  // imza Tabanlı tarama sınıfı
    {
        private List<FileFormat> allSystemFiles;
        private List<string> MFinSystem; //veri tabanındaki hashleri topladığımız liste
        private List<FileFormat> MalwareFiles; // tarama sonucu bulunan virüslerin listesi
        public Results results;
        private String path;


        public signatureAntivirus()
        {
            allSystemFiles = new List<FileFormat>();
            MFinSystem = new List<string>();
            MalwareFiles = new List<FileFormat>();

        }

        public signatureAntivirus(scanTypes type)
        {
            allSystemFiles = new List<FileFormat>();
            MFinSystem = new List<string>();
            MalwareFiles = new List<FileFormat>();
            if (type == scanTypes.FAST)
            {//hızlı tarama için

                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToString(), "Downloads");
                // f.RunScan(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            }
            else if (type == scanTypes.FULL)//tam tarama için
            {
                path = "C:\\";
                //disk ismi gönder
            }
        }
        public signatureAntivirus(string path) // dosya taraması için constructer
        {
            allSystemFiles = new List<FileFormat>();
            MFinSystem = new List<string>();
            MalwareFiles = new List<FileFormat>();
            this.path= path;
        }
        public void RunScan()  // taranacak dosya yolu gönderilip tarama işlemini başlatma
        {
            SetMalwareDatas();
            string currentDir;
            if (this.path == null)
            {
                currentDir = "C:\\Users\\mgoek\\Desktop\\"; // TODO: uyarı versin
            }
            else
            {
                currentDir = this.path;
            }
            DisplayDirectoryContents(currentDir);
            CleanAll();
            results=new Results(this.allSystemFiles,this.MFinSystem,this.MalwareFiles);  
        }

        public void DisplayDirectoryContents(string dir) // verilen dizindeki tüm alt dosyaların consola ve tarama işlemi için dizine aktarılması
        {
            try
            {
                string[] files = Directory.GetFiles(dir);
                string[] directories = Directory.GetDirectories(dir);

                foreach (string file in files)
                {
                    Console.Write(file);
                    AddFile(file);
                }

                foreach (string subdirectory in directories)
                {
                    Console.Write(subdirectory);
                    DisplayDirectoryContents(subdirectory);
                }
            }
            catch (Exception e)
            {
                // Handle exception
            }
        }

        public void WriteTxt(string content)   // tarama sonuçlarının txt ye yazdırılması
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt";
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine(content);
            }
        }

        public void CleanAll() // eğer bir malware bulunduysa (yani malwareFiles dosyasında bir dosya dizini ismi varsa) onu silip, dosya yolunu tarama sonucuna yazması ve eğer sistem güvenli ise safe yazısının basılması
        {
            if (MalwareFiles.Count >= 1)
            {
                int count = MalwareFiles.Count;
                string dateFormat = "yyyy/MM/dd HH:mm:ss";
                DateTime date = DateTime.Now;
                string content;
                foreach (FileFormat malwareFile in MalwareFiles)
                {
                    DeleteFile(malwareFile.FName);
                    content = " **SİLİNDİ! " + " Dosya Yolu; " + malwareFile.FName;
                    WriteTxt(content);
                    
                }
                content = "Son Tarama Tarihi " + date.ToString(dateFormat) + "\t;BULUNAN VİRÜSLERİN SAYISI:" + count.ToString(); 
                WriteTxt(content);
            }
            else
            {
                string dateFormat = "yyyy/MM/dd HH:mm:ss";
                DateTime date = DateTime.Now;
                WriteTxt(" SİSTEM GÜVENLİ !; " + "Son Tarama Tarihi " + date.ToString(dateFormat));
            }
        }

        public void AddFile(string fPat) // her bulunan dosya yolunun msd5 hash inin çıkarılıp check edildikten sonra malwareFiles dosyasına eklenip eklenmeyeceği belirleniyor.
        {
            string md5 = GetMD5(fPat);
            bool MFcheck = IsMalwareFile(md5);
            if (MFcheck)
            {
                MalwareFiles.Add(new FileFormat(fPat, md5));
            }
            allSystemFiles.Add(new FileFormat(fPat, md5));
        }

        public bool IsMalwareFile(string curFile) //gelen dosyayı database ile karşılaştırma için bir abstract(soyut) dosya
        {
            return IsElementOfMF(curFile);
        }

        public bool IsElementOfMF(string newFMd5) //gelen dosyayı database ile karşılaştırma için bir concrete(somut) dosya
        {
            foreach (string temp in MFinSystem)
            {
                if (temp == newFMd5)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetMD5(string path) // dosyalarıjn MD5 değerlerini çıkartan fonksiyom
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

        public int GetNumberofAllFiles() // prod da olmayacak sadece dev görecek şekilde taranan dosya sayısını görüntülemek için gerekli kod bloğu
        {
            return allSystemFiles.Count;
        }

        public int GetNumberofMFFiles() // prod da olmayacak sadece dev görecek şekilde sistemdeki malware sayısını görüntülemek için gerekli kod bloğu
        {
            return MFinSystem.Count;
        }

        public void UpdateMF() // ileride heuristik için imza tabanlı veri setine ekleme yapılmak istenirse eklenebilecek fonksiyon
        {
            string directoryPath = "C:\\Users\\mgoek\\Desktop";
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        Console.WriteLine(file);
                    }
                }
                else
                {
                    Console.WriteLine("Dizin boş.");
                }
            }
            else
            {
                Console.WriteLine("Dizin bulunamadı veya bir dizin değil.");
            }
            try
            {
                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        string md5 = GetMD5(file);
                        MFinSystem.Add(md5);
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exception
            }
        }

        public void WriteMalwareData()  // ileride heuristik için imza tabanlı veri setine ekleme yapılmak istenirse eklenebilecek fonksiyon
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\MALWARES.txt";
            UpdateMF();
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                foreach (string md5 in MFinSystem)
                {
                    file.WriteLine(md5);
                }
            }
        }

        public void SetMalwareDatas()  // Tarama işlemi için veritabanından okuma yapıp sistemi imza veri setini uygulamaya çekme işlemi
        {
            string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\HashDB"; // SQLite veritabanı dosyasının yolu

            string connectionString = $"Data Source={databasePath};";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "select hash from HashDB"; // Tablo adını güncelleyin
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string hash = reader.GetString(0); // İkinci sütunu string olarak alıyoruz
                            MFinSystem.Add(hash);
                            Console.WriteLine($"Hash: {hash}");
                        }
                    }
                }

                connection.Close();

            }
        }

        public bool DeleteFile(string filePath) // malware olan datayı silmek için gerekli fonksiyon
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
