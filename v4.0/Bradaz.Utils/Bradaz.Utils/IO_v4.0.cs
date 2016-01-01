using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bradaz.Utils;
using Bradaz.Utils.Interfaces;
using Bradaz.Utils.Strings;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


///Class will encapsulate IO functions.
namespace Bradaz.Utils.IO
{
    ///--Todo:
    /// a) Async version of the Stream
    
 /// <summary>
 /// Class to act as a Stream for CSV Files.
 /// </summary>
    public class CSVReader : StreamReader
    {
        protected string fileName;
        protected Stream stream;
        /// <summary>
        /// Initializes a CSVReader if a Stream has already been created
        /// in the application.
        /// </summary>
        /// <param name="stream"></param>
        public CSVReader (Stream stream)
            : base(stream)
        {    
            this.fileName = "";
            this.stream = stream;
        }
        /// <summary>
        /// Initializes a CSVReader using a filename,
        /// </summary>
        /// <param name="fileName"></param>
        public CSVReader(string fileName)
            : base(fileName)
        { 
            if(!File.Exists(fileName))
                throw new FileNotFoundException("File Not Found fileName " + fileName);

            this.fileName = fileName;
        }
    }

    /// <summary>
    /// Class that represents a row within a CSV file.
    /// </summary>
    public class CSVRow
    {
        /// <summary>
        /// Describes if the row has been validated or not.
        /// </summary>
        protected State state;
        /// <summary>
        /// List of (if any) errors why the row would of been rejected.
        /// </summary>
        protected List<RowError> errors = new List<RowError>();
        /// <summary>
        /// Describes the column.
        /// </summary>
        protected List<CSVColumn> columns = new List<CSVColumn>();
        
        protected int numberOfColumns;
        protected int rowNumber;
        private string originalRow;
        public int NumberOfColumns 
        {
            get { return numberOfColumns; }
            internal set { numberOfColumns = value; }
        }
        public int RowNumber 
        { 
              get {return rowNumber;}
            internal set { rowNumber = value; }
        }

        /// <summary>
        /// Data passed in.
        /// </summary>
        public string OriginalData
        {
            get { return originalRow; }
            internal set { originalRow = value; }
        }

      
        public State State
        {
            get { return state; }
            set { state = value; }
        }

        public List<RowError> Errors
        {
            get { return errors; }
            set { errors = value; }
        }

        public List<CSVColumn> Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        /// <summary>
        /// Default contructor.
        /// </summary>
        public CSVRow ()
        { }
        public CSVRow(string line)
        {
            originalRow = line;
        }

    }

    /// <summary>
    /// Object to describe a CSV Column within a CSV Row (CSVRow) within a CSV File (CSVFile).
    /// </summary>
    public class CSVColumn
    {
        protected int columnNumber;
        protected int rowNumber;
        protected string columnValue;
        public int ColumnNumber
        {
            get { return columnNumber; }
            internal set { columnNumber = value; }
        }

        public int RowNumber
        {
            get { return rowNumber; }
            internal set { rowNumber = value; }
        }

        public string ColumnValue
        {
            get { return columnValue; }
            internal set { columnValue = value; }
        }

        /// <summary>
        /// Intialises a CSV Column.
        /// </summary>
        /// <param name="columnValue"></param>
        /// <param name="columnNumber"></param>
        /// <param name="rowNumber"></param>
        public CSVColumn (string columnValue, int columnNumber, int rowNumber)
        {
            this.columnValue = columnValue;
            this.columnNumber = columnNumber;
            this.rowNumber = rowNumber;
        }

    }
    /// <summary>
    /// Class that represents a CSV File.
    /// </summary>
    public class CSVFile : ICSV, IDisposable
    {
        protected string lineBuffer;
        internal List<CSVRow> rows = new List<CSVRow>();
        public CSVReader CSVStream { get; set; }
        public int NumberOfColumns { get; private set;}
        private int previousNumberOfColumns;
        public int NumberOfRows { get; private set; }
        public char Delimiter { get; set; }
        public List<CSVRow> Rows
        {
            get { return rows; }
        }
 
        public string FileNameAndPath { get; set; }

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        /// <summary>
        /// Default constructor for a CSV File. Defaults the delimiter to
        /// a comma (,).
        /// </summary>
        /// <param name="fileNameAndPath"></param>
        public CSVFile(string fileNameAndPath) 
        {
            if (!File.Exists(fileNameAndPath))
                throw new FileNotFoundException("File Not Found fileName " + fileNameAndPath);
            Delimiter = ',';
            FileNameAndPath = fileNameAndPath;
        }
        /// <summary>
        /// Initilizes a CSV file. You can specify a delimiter.
        /// </summary>
        /// <param name="fileNameAndPath"></param>
        /// <param name="delimiter"></param>
        public CSVFile(string fileNameAndPath, char delimiter) 
        {
            if (!File.Exists(fileNameAndPath))
                throw new FileNotFoundException("File Not Found fileName " + fileNameAndPath);
            Delimiter = delimiter;
            FileNameAndPath = fileNameAndPath;
            CSVReader CSVStream = new CSVReader(fileNameAndPath);
        }

        /// <summary>
        /// Initlizes a CSV file using an exising Stream.
        /// </summary>
        /// <param name="stream"></param>
        public CSVFile (Stream stream)
        {

            CSVReader CSVStream = new CSVReader(stream);
            Delimiter = ',';
        }

        /// <summary>
        /// Initlizes a CSV file using an exising Stream and passes a delimiter.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="delimiter"></param>
        public CSVFile(Stream stream, char delimiter)
        {

            CSVReader CSVStream = new CSVReader(stream);
            Delimiter = delimiter;

        }

        /// <summary>
        /// Read the line into a buffer area and create a row
        /// </summary>insta
        public void ReadLines()
        {
            ReadLines(false);
        }

        /// <summary>
        /// Read the line into a buffer area and print out the line count
        /// </summary>
        /// <param name="printNumberOfRows"></param>
        public void ReadLines(bool printNumberOfRows)
        {
            
            try
            {
                while ((lineBuffer = CSVStream.ReadLine()) != null)
                {
                    NumberOfRows++;
                    var r = ParseRow(lineBuffer,NumberOfRows);
                    if (NumberOfColumns == 0)
                    {
                        NumberOfColumns = r.NumberOfColumns;
                    }
                    r.OriginalData.Insert(0, lineBuffer);
                    rows.Add(r);
                                  
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The Read line of " + FileNameAndPath + " has failed due to: " + e.ToString());
            }
        }
        /// <summary>
        /// Parses a possible CSV string using the rules stated in
        /// the proposed RFC4180:
        /// https://tools.ietf.org/html/rfc4180#ref-2
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private CSVRow ParseRow(string row, int rowNumber)
        {
            
            CSVRow _row = new CSVRow(row);
            _row.NumberOfColumns = 1;
            _row.State = State.Validated;
            if(String.IsNullOrEmpty(row)) 
            {
                _row.State = State.Failed;
                _row.Errors.Add(RowError.EmptyRow);
                _row.NumberOfColumns = 0;
                _row.RowNumber = rowNumber;
            }
 
            
            //We need to count the columns.
            int positionInRow = 0;
            bool foundEndDoubleQuote = false;
            StringBuilder columnData = new StringBuilder();
 
            while (positionInRow < row.Length && !String.IsNullOrEmpty(row))
            {
                foundEndDoubleQuote = false;
                int startPosition = positionInRow;

                //--Remember, delimiters within embedded double quotes are allowed.
                //--Rules: If there is a '"' then increment the positionInRow until the last
                //--       '"' is found.
              
                if (row[positionInRow] == '"')
                {
                    while (!foundEndDoubleQuote)
                    {
                        positionInRow++;
                        columnData.Append(row[positionInRow]);
                        if (row[positionInRow] == '"' && row[positionInRow + 1] != '"')
                        {
                            foundEndDoubleQuote = true;
                        }
                    }
                }
                else
                {
                    if (row[positionInRow] != Delimiter)
                    {
                        if (positionInRow == 0 || columnData.Length == 0)
                        {
                            columnData.Insert(0, row[positionInRow]);
                        }
                        else
                        {
                            columnData.Append(row[positionInRow]);
                        }
                    }
                }

                if(row[positionInRow] == Delimiter)
                {
           
                    //Comma must not be the last character. 
                    if (positionInRow == (row.Length - 1))
                    {
                        _row.Errors.Add(RowError.LastFieldFollowedByComma);
                        _row.State = State.Failed;
                    }
                    else
                    {
                        CSVColumn col = new CSVColumn(columnData.ToString(), _row.NumberOfColumns, rowNumber);
                        _row.NumberOfColumns++;
                        columnData.Clear();
                        _row.Columns.Add(col);
                    }             
                }
                else if (positionInRow == (row.Length - 1))
                {
                    CSVColumn col = new CSVColumn(columnData.ToString(), _row.NumberOfColumns, rowNumber);
                    columnData.Clear();
                    _row.Columns.Add(col);
                }

                positionInRow++;
                foundEndDoubleQuote = false;
                _row.RowNumber = rowNumber;           
            }

            if (rowNumber == 1 && _row.NumberOfColumns > 0)
            {
                //First rows column number we will take.
                previousNumberOfColumns = _row.NumberOfColumns;
            }

            ///Column number must match throughout the file.
            if (_row.NumberOfColumns != previousNumberOfColumns)
            {
                _row.State = State.Failed;
                _row.Errors.Add(RowError.MoreColumnsThanPreviousRows);
            }


            return _row;
        }

        /// <summary>
        /// Method to remove the delimiter from the line buffer.
        /// </summary>
        protected void RemoveDelimiterFromLine()
        {
            ReplaceSubStrings replace = new ReplaceSubStrings(Delimiter, ' ');
            lineBuffer = Regex.Replace(lineBuffer, replace.SearchFor, replace.ReplaceWith);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            CSVStream.Dispose();
            disposed = true;

        }


    }
}
