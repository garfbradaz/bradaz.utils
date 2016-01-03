using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bradaz.Utils.IO;
using Bradaz.Utils;

namespace Test_CSV_WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string myFile = @"C:\Users\Gareth\SkyDrive\CODE\Bradaz.Utils\ConsoleApplication1\testfile.txt";
                CSVFile file = new CSVFile(myFile);
                int index = 0;
                
                using (file.CSVStream = new CSVReader(file.FileNameAndPath))
                {
                    file.ReadLines();
                    dataGridView1.ColumnCount = file.NumberOfColumns;


                    foreach (CSVRow row in file.Rows)
                    {
                        if (row.State == State.Validated)
                        {
                            DataGridViewRow dataGridRow = new DataGridViewRow();
                            dataGridRow.CreateCells(dataGridView1);

                            foreach (CSVColumn column in row.Columns)
                            {
                                ///Set the columns out.
<<<<<<< HEAD
                                ///Todo: Presumption made Row 1 is the/a header record - we need to change that to check somehow.
=======
>>>>>>> refs/remotes/origin/csvreader
                                if (column.RowNumber == 1)
                                {
                                    dataGridView1.Columns[column.ColumnNumber - 1].Name = column.ColumnValue;
                                }
                                else
                                {
                                    dataGridRow.Cells[column.ColumnNumber - 1].Value = column.ColumnValue;
                                }
                                
                      
                            }

                            if(row.RowNumber > 1)
                            {
                                dataGridView1.Rows.Add(dataGridRow);
                            }
                          
                        }
                    }
                }     
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file " + ex.Message + " - ");
            }
        }

        }
}
