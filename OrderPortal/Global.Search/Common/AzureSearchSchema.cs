using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search.Common
{
    public static class MyExtensions
    {
        /// <summary>
        /// Extension method to retrieve the REST API friendly property value
        /// </summary>
        /// <param name="en">the enum</param>
        /// <returns>property value</returns>
        public static string GetDescription(this DataType en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return en.ToString();

        }
    }

    [DataContract]
    public sealed class AzureSearchSchema
    {
        public AzureSearchSchema()
        {
            Fields = new List<SearchColumn>();            
        }
        [DataMember(Name="name")]
        public string Name { get; set; }
        [DataMember(Name = "fields")]
        internal List<SearchColumn> Fields { get; set; }

        /// <summary>
        /// Adds a column to the schema
        /// </summary>
        /// <param name="column">the column</param>
        public void Add(SearchColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }
            Fields.Add(column);
        }

        /// <summary>
        /// Get the key column for a schema
        /// </summary>
        /// <returns>search column containing the key</returns>
        public SearchColumn GetKey()
        {
            return Fields.Single(r => r.Key == true);
        }

        /// <summary>
        /// Add a list of columns to a schema
        /// </summary>
        /// <param name="columns">columns</param>
        public void Add(List<SearchColumn> columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }
            foreach (var column in columns)
            {
                Add(column);
            }
        }

        /// <summary>
        /// removes the Column from the schema
        /// </summary>
        /// <param name="name">the name of the column</param>
        public void RemoveColumn(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            Fields.Remove(Fields.Single(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
        }
    }
   
    [DataContract]
    public sealed class SearchColumn
    {
        public SearchColumn(DataType type)
        {
            if (type == DataType.String || type == DataType.CollectionOfString)
            {
                Searchable = true;
            }
            
            Type = type;
            Filterable = true;
            if (type == DataType.CollectionOfString)
            {
                Sortable = false;
            }
            if (type != DataType.GeographyPoint)
            {
                Facetable = true;
            }
            Retrievable = true;
        }

        /// <summary>
        /// Name of column
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "type")]
        public DataType Type { get; set; }


        private bool _Key;

        /// <summary>
        /// default is false, only String fields can be keys
        /// </summary>
        [DataMember(Name = "key")]
        public bool Key
        {
            get { return _Key; }
            set 
            {
                if (Type != DataType.String && value)
                {
                    throw new ArgumentException("The Type must be a String for to be a key.");
                }
                _Key = value; 
            }
        }
        

        private bool _Searchable;
        /// <summary>
        /// default is true if data type is either: String and CollectionOfString since these fields are searchable
        /// </summary>
        [DataMember(Name = "searchable")]
        public bool Searchable
        {
            get { return _Searchable; }
            set 
            {
                if ( (Type != DataType.CollectionOfString && Type != DataType.String ) && value) 
                {
                    throw new ArgumentException("Only String and CollectionOfString are searchable.");
                }
                _Searchable = value; 
            }
        }

        /// <summary>
        /// default is true
        /// </summary>
        [DataMember(Name = "filterable")]
        public bool Filterable { get; set; }

        private bool _Sortable;
        /// <summary>
        /// default is true if data type is not Collection of String
        /// </summary>
        [DataMember(Name = "sortable")]
        public bool Sortable
        {
            get { return _Sortable; }
            set 
            {
                if (Type == DataType.CollectionOfString && value)
                {
                    throw new ArgumentException("CollectionOfString cannot be sorted.");
                }
                _Sortable = value; 
            }
        }



        private bool _Facetable;
        /// <summary>
        /// default is true, Geography Points are not facetable.
        /// </summary>
        [DataMember(Name = "facetable")]
        public bool Facetable
        {
            get { return _Facetable; }
            set 
            {
                if (Type == DataType.GeographyPoint && value)
                {
                    throw new ArgumentException("GeographyPoint is not Facetable");
                }
                _Facetable = value; 
            }
        }
        

        /// <summary>
        /// default is true
        /// </summary>
        [DataMember(Name = "retrievable")]
        public bool Retrievable { get; set; }

        private bool _Suggestions;
        /// <summary>
        /// default is false, only String and Collection of String can be used for suggestions
        /// </summary>
        [DataMember(Name = "suggestions")]
        public bool Suggestions
        {
            get { return _Suggestions; }
            set 
            {
                if ((Type != DataType.CollectionOfString && Type != DataType.String) && value)
                {
                    throw new ArgumentException("String and CollectionOfString can only be used for suggestions.");
                }
                _Suggestions = value; 
            }
        }

     
        

    }

    public enum DataType
    {
        [Description("Edm.String")]
        String = 0,
        
        [Description("Edm.Double")]
        Double = 1,
        
        [Description("Edm.DateTimeOffset")]
        DateTimeOffset = 3,
        
        [Description("Edm.Int32")]
        Integer = 5,

        [Description("Collection(Edm.String)")]
        CollectionOfString = 7,
        
        [Description("Edm.Boolean")]
        Boolean = 9,

        [Description("Edm.GeographyPoint")]
        GeographyPoint = 11
       
    }

    


}
