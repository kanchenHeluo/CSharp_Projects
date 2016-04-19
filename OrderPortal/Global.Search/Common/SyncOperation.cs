using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search.Common
{
    public enum SyncOperation
    {
        /// <summary>
        /// upload: An upload command is similar to an "upsert" where the document will be inserted if it is new and updated/replaced if it exists. Note that all fields are replaced in the update case.
        /// </summary>
        upload = 0,
        /// <summary>
        /// merge: Merge updates an existing document with the specified fields. If the document doesn't exist, the merge will fail. Any field you specify in a merge will replace the existing field in the
        /// document. This includes fields of type Collection(Edm.String). For example, if the document contains a field "tags" with value ["budget"] and you execute a merge with value ["economy", "pool"] for 
        /// "tags", the final value of the "tags" field will be ["economy", "pool"]. It will not be ["budget", "economy", "pool"].
        /// </summary>
        merge = 3,
        /// <summary>
        /// delete: Delete removes the specified document from the index. Note that you cannot specify any field values in a delete operation. Attempting to do so will result in an HTTP 400 error. If you want to
        /// remove an individual field from a document, use merge instead and simply set the field explicitly to null.
        /// </summary>
        delete = 5
    }
}
