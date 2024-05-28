using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using PeNet;

namespace antivirusProject
{
    internal class heuristicAntivirus
    {

        public heuristicAntivirus() {

        
        }
        public static List<string> predict(string cmd, string args,List<string>filesforHeuristic)
        {
            int karantinacount = 0;
            string result="";
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami"+"\\Python37\\python.exe";
                start.Arguments = string.Format("{0} {1}", cmd, args);
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        result = reader.ReadToEnd();
                        Console.Write(result);
                        string[] words = result.Split('\n');

                        for (int i = 0; i < words.Length-1; i++)
                        {
                            // Her kelimeyi "armut" kelimesi ile karşılaştıralım
                            if (words[i].Contains("1"))
                            {//güvenli
                                filesforHeuristic[i] = "";
                            }
                            else
                            {

                                //zararlı
                                
                                karantinacount++;
                                WriteTxt(filesforHeuristic[i]);
                                
                            }
                        }
                    }
                if (karantinacount != 0)
                {

                    WriteTxtToMain( " KARANTİNA ALINAN DOSYA SAYISI    ;"+karantinacount+" ;"+ DateTime.Now);
                }
                
                }
            return filesforHeuristic;
        }
        public static void WriteTxt(string content)   // tarama sonuçlarının txt ye yazdırılması
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\karantina.txt";
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine(content);
            }
        }
        public static void WriteTxtToMain(string content)   // tarama sonuçlarının txt ye yazdırılması
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\print.txt";
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine(content);
            }
        }



        public static void getPEfile(string path)
        {
            
            string ciktiDosyaYolu = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\ana_veri.txt"; // Çıktı dosyasının yolu

            try
            {
                using (StreamWriter writer = new StreamWriter(ciktiDosyaYolu))
                {
                    writer.WriteLine("Application Name\nFEATURES");
                    PeFile peFile = new PeFile(path);
                    // İçe aktarılan DLL'leri listele
                    foreach (var importDll in peFile.ImportedFunctions.Select(i => i.DLL).Distinct())
                    {
                        // DLL içeriğindeki fonksiyonları listele
                        foreach (var function in peFile.ImportedFunctions.Where(i => i.DLL == importDll))
                        {
                            writer.WriteLine($"{function.Name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }


        }

        public static void extract()
        {

            int kacinci_app = 79;
            int a = 492;
            string filePath = @"C:\Users\mgoek\Documents\AntivirusProgrami\malvare_dataset.xls";
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Open(filePath);
            var sheet = (Worksheet)workbook.Sheets[1];
            var mathArray = new List<List<string>>();
            int row = sheet.UsedRange.Rows.Count;
            int col = sheet.UsedRange.Columns.Count;

            for (int i = 1; i <= row; i++)
            {
                var colArr = new List<string>();
                for (int j = 1; j <= col; j++)
                {
                    var cell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[i, j];
                    colArr.Add(cell.Text.ToString());
                }
                mathArray.Add(colArr);
            }

            using (var fileReader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\ana_veri.txt"))
            {
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    for (int i = 0; i < row; i++)
                    {
                        if (line.CompareTo(mathArray[i][0]) == 0)
                        {
                            mathArray[i][kacinci_app] = "1";
                            i = row;
                        }
                        else if (i == a)
                        {
                            mathArray[i + 1][0] = line;
                            mathArray[i + 1][kacinci_app] = "1";
                            a++;
                            i = row;
                        }
                    }
                }
            }

            using (var fileWriter = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\yeniozellikveri.txt"))
            {
                for (int i = 0; i < row; i++)
                {
                    fileWriter.WriteLine(mathArray[i][0]);
                }
            }

            using (var fileWriter1 = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\yenisonucveri.txt"))
            {
                fileWriter1.WriteLine("s");
                fileWriter1.WriteLine("x");
                for (int i = 0; i < row; i++)
                {
                    if (mathArray[i][kacinci_app] == "x")
                    {
                        fileWriter1.WriteLine("0");
                    }
                    else
                    {
                        fileWriter1.WriteLine(mathArray[i][kacinci_app]);
                    }
                }
            }

            workbook.Close(false);
            excelApp.Quit();
        }


        public static void csvTransfer()
        {
            // TXT dosyasını oku
            List<string> txtVerileri = new List<string>(File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\yenisonucveri.txt"));

            // CSV dosyasını oku ve mevcut verileri koru
            List<List<string>> csvVerileri = new List<List<string>>();
            using (var reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\malvare_test(malware).csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = new List<string>(line.Split(','));
                    csvVerileri.Add(values);
                }
            }

            // TXT verilerini CSV verilerine yeni bir sütun olarak ekle
            // Her satıra karşılık gelen txt satırını yeni bir sütun olarak ekliyoruz
            for (int i = 0; i < csvVerileri.Count; i++)
            {
                if (i < txtVerileri.Count)
                {
                    csvVerileri[i].Add(txtVerileri[i]);
                }
                else
                {
                    csvVerileri[i].Add("");
                }
            }

            // Güncellenmiş verileri yeni bir CSV dosyasına kaydet
            using (var writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AntivirusProgrami" + "\\malvare_test(malware).csv"))
            {
                foreach (var row in csvVerileri)
                {
                    writer.WriteLine(string.Join(",", row));
                }
            }
        }





    }
}
