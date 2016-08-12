using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bradaz.Utils.IO;
using Bradaz.Utils;

namespace Test_CSV
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                //string myFile = @"C:\Users\Gareth\SkyDrive\CODE\Bradaz.Utils\ConsoleApplication1\testfile.txt";
                string myFile = @"C:\Git\MyGitHub\bradaz.utils\v4.0\Bradaz.Utils\test_app_v4\testfile.txt";
                CSVFile file = new CSVFile(myFile);
         
                using (file.CSVStream = new CSVReader(file.FileNameAndPath))
                {
                    file.ReadLines();

                    //sort so we get the Validated first.
   
                    foreach(CSVRow row in file.Rows)
                    {
                        Console.WriteLine("==========");
                        Console.WriteLine("Row No: " + row.RowNumber + " has " + row.NumberOfColumns + " columns. The data is " + row.OriginalData);
                        Console.WriteLine("Validation: ");
                        Console.WriteLine("Row No: " + row.RowNumber + " " + row.State);

                        foreach (RowError error in row.Errors)
                        {
                            Console.WriteLine("Error: " + error);

                        }

                        if (row.NumberOfColumns > 0)
                        {
                            foreach(CSVColumn column in row.Columns)
                            {
                                Console.WriteLine("Column No: " + column.ColumnNumber + " for row " + column.RowNumber + " " + column.ColumnValue);
                            }
                        }
 
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading file " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
