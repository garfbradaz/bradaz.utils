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
using System.Dynamic;


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

        #region Column Settings

        private Dictionary<int, CSVColumnDescriptor> ColumnSettings = new Dictionary<int, CSVColumnDescriptor>();
        public Dictionary<int, dynamic> dynamicFile = new Dictionary<int, dynamic>();
        
        #endregion

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

        #region Methods
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
           
                    //Delimiter must not be the last character. 
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

            //Create a Dynamic Object if there is a settings file to parse.
            if(ColumnSettings.Count > 0 &&  _row.State == State.Validated)
            {
                CreateDynamicCSV(_row);
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

        /// <summary>
        /// Method to allow you to set settings for Columns.
        /// </summary>
        /// <param name="setting">setting</param>
        public void AddSetting(CSVColumnDescriptor setting)
        {
            if(setting == null)
            {
                throw new ArgumentNullException("The setting parameter is empty! you cannot add an empty setting" 
               + " it will blow things up! ");
            }

            
            CSVColumnDescriptor test;
            if(ColumnSettings.TryGetValue(setting.ColumnNumber,out test))
            {
                throw new ArgumentException("A setting with Column Number " + setting.ColumnNumber.ToString() + " already exists. Found: \n"
                    + "ColumnName- " + test.ColumnName + " ColumnNumber- " + test.ColumnNumber + " ColumnType-" + test.ColumnType.ToString());
            }

            try
            {
                ColumnSettings.Add(setting.ColumnNumber, setting);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Key is null for settings " + setting.ColumnNumber);
            }

        }

        /// <summary>
        /// Method to allow you to set a collection of settings for Columns.
        /// </summary>
        /// <param name="settings">settings.</param>
        public void AddSettings(IEnumerable<CSVColumnDescriptor> settings)
        {
            foreach (CSVColumnDescriptor setting in settings)
            {
                AddSetting(setting);
            }
        }

        /// <summary>
        /// Method to create the dynamic collection based off <see cref="CSVRow.cs"/>  data.
        /// and settings passed in by <seealso cref="CSVColumnDescriptor.cs"/>
        /// </summary>
        /// <param name="row"></param>
        protected void CreateDynamicCSV(CSVRow row)
        {

            Dictionary<string, object> propertyNamesAndValues = new Dictionary<string, object>();
            foreach (CSVColumn column in row.Columns)
            {
                //Locate PropertyName from CSVSettings.
                CSVColumnDescriptor setting;
                int lookup = 0;
                if(column.ColumnNumber > 0)
                {
                    lookup = column.ColumnNumber - 1;
                }
                if (!ColumnSettings.TryGetValue(lookup, out setting))
                {
                    throw new ArgumentException("Cannot find Column Number " + column.ColumnNumber + " for row " + row.RowNumber );
                }                                                                                                                                   
                
                object test;
                if(propertyNamesAndValues.TryGetValue(setting.ColumnName,out test))   
                {
                    throw new ArgumentException("Property Name already exists " + setting.ColumnName);
                }

                //Add the property Name and value to the Dictionary.
                propertyNamesAndValues.Add(setting.ColumnName, column.ColumnValue);                    
            }

            dynamic dyn = new CSVDynamic(propertyNamesAndValues);
            dynamicFile.Add(row.RowNumber,dyn);
        }  

        /// <summary>
        /// Method to allow you to query the CSVFile and return a dynamic POCO
        /// representation of the row.
        /// </summary>
        /// <param name="rowNumber">rowNumber to find.</param>
        /// <returns></returns>
        public dynamic GetDynamicCSVObject(int rowNumber)
        {
            dynamic d;
            return (dynamicFile.TryGetValue(rowNumber, out d)) ? d : null;
        }

        #endregion
    }

    /// <summary>
    /// Class that represents settings of a column.
    /// Things like the data type of the column.
    /// </summary>
    public class CSVColumnDescriptor
    {
        #region Properties and fields
        private int columnNumber;

        public int ColumnNumber
        {
            get { return columnNumber; }
            set { columnNumber = value; }
        }

        private Type columnType;

        public Type ColumnType
        {
            get { return columnType; }
            set { columnType = (value != null) ? value : typeof(System.String);}
        }

        private bool columnHasHeader;

        public bool ColumnHasHeader
        {
            get { return columnHasHeader; }
            set { columnHasHeader = value; }
        }

        private string columnHeader;

        public string ColumnHeader
        {
            get { return columnHeader; }
            set 
            { 
                columnHeader = (!string.IsNullOrWhiteSpace(value)) ? value : string.Empty;
                columnHasHeader = (!string.IsNullOrWhiteSpace(value)) ? true : false;
            }
        }

        private bool columnCanBeEmpty;

        public bool ColumnCanBeEmpty
        {
            get { return columnCanBeEmpty; }
            set { columnCanBeEmpty = value; }
        }

        private string columnName;

        public string ColumnName
        {
            get { return columnName; }
            set { columnName = (!string.IsNullOrWhiteSpace(value)) ? value : string.Empty; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="columnNumber">Column Number in file (Zero based indexing)</param>
        /// <param name="columnType">CLR Type (like String etc)</param>
        /// <param name="columnPropertyName">Name of the property you will reference in your object</param>
        public CSVColumnDescriptor(int columnNumber, Type columnType, string columnPropertyName)
        {
            Init(columnNumber, columnType, false, null, true, columnPropertyName);
        }
        #endregion

        #region Methods
        private void Init(int columnNumber = 0, Type columnType = null, bool columnHasHeader = false,
            string columnHeader = null, bool columnCanBeEmpty = true, string columnName = null)
        {
            this.ColumnNumber = columnNumber; 
            this.ColumnType = columnType;
            this.ColumnHasHeader = columnHasHeader;
            this.ColumnHeader = columnHeader;
            this.ColumnCanBeEmpty = columnCanBeEmpty;
            this.ColumnName = columnName;
           
        }
        #endregion

    }

    /// <summary>
    /// Dynamic CSV Object. 
    /// <remarks>Each CSVDynamic represents a row with <see cref="CSVFile.cs"/> and each property of
    /// CSVDynamic represents a <see cref="CSVColumn.cs"/> with the row.</remarks>
    /// </summary>
    public sealed class CSVDynamic : DynamicObject
    {
        private readonly Dictionary<string, object> properties;
        public CSVDynamic (Dictionary<string,object> properties)
        {
            this.properties = properties;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return properties.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (properties.ContainsKey(binder.Name))
            {
                result = properties[binder.Name];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (properties.ContainsKey(binder.Name))
            {
                properties[binder.Name] = value;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
