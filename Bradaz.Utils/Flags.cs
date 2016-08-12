using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bradaz.Utils
{

    /// <summary>
    /// State of a record.
    /// </summary>
    [Flags]
    public enum State
    {
        Validated = 0,
        Failed = 1,
        LogicallyDeleted = 2

    }

    /// <summary>
    /// These Row Errors will be found during Parsing the row.
    /// They are based off the rules of the RFC4180.
    /// </summary>
    /// 
    [Flags]
    public enum RowError
    {
        RFC4180Passed = 0,
        MoreColumnsThanPreviousRows = 1,
        LastFieldFollowedByComma = 2,
        ImbeddedDelimiterIssue = 3,
        ImeddedDoubleQuoteIssue = 4,
        EmptyRow = 5


    }
        
    [Flags]
    public enum ValueType
    {
        String = 0,
        Date = 1,
        Time = 2,
        Integer = 3,
        Boolean = 4,
        Decimal = 5

    }

}
