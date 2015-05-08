﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bradaz.Utils.XML
{
    //------------------------------------------------------------------------------
    // <auto-generated>
    //     This code was generated by a tool.
    //     Runtime Version:4.0.30319.34014
    //
    //     Changes to this file may cause incorrect behavior and will be lost if
    //     the code is regenerated.
    // </auto-generated>
    //------------------------------------------------------------------------------

    

    // 
    // This source code was auto-generated by xsd, Version=4.0.30319.33440.
    // 

    #region XMLCSVFileSettings
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLCSVColumn.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/XMLCSVColumn.xsd", IsNullable = false)]
    public partial class CSVSettings
    {

        private string numberOfColumnsField;

        private bool headerRecordIncludedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string numberOfColumns
        {
            get
            {
                return this.numberOfColumnsField;
            }
            set
            {
                this.numberOfColumnsField = value;
            }
        }

        /// <remarks/>
        public bool headerRecordIncluded
        {
            get
            {
                return this.headerRecordIncludedField;
            }
            set
            {
                this.headerRecordIncludedField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLCSVColumn.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/XMLCSVColumn.xsd", IsNullable = false)]
    public partial class CSVColumn
    {

        private string columnNumberField;

        private string valueTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        public string columnNumber
        {
            get
            {
                return this.columnNumberField;
            }
            set
            {
                this.columnNumberField = value;
            }
        }

        /// <remarks/>
        public string valueType
        {
            get
            {
                return this.valueTypeField;
            }
            set
            {
                this.valueTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLCSVColumn.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/XMLCSVColumn.xsd", IsNullable = false)]
    public partial class CSVSettingsFile
    {

        private CSVSettingsFileHeader headerField;

        private CSVColumn[] cSVColumnField;

        /// <remarks/>
        public CSVSettingsFileHeader header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CSVColumn")]
        public CSVColumn[] CSVColumn
        {
            get
            {
                return this.cSVColumnField;
            }
            set
            {
                this.cSVColumnField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/XMLCSVColumn.xsd")]
    public partial class CSVSettingsFileHeader
    {

        private CSVSettings cSVSettingsField;

        /// <remarks/>
        public CSVSettings CSVSettings
        {
            get
            {
                return this.cSVSettingsField;
            }
            set
            {
                this.cSVSettingsField = value;
            }
        }
    }

}
    #endregion
