using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.ComponentModel;
using WebContextor = com.sentillion.sdkweb.webcontextor.WebContextor;
using ContextItemCollection = com.sentillion.sdkweb.webcontextor.ContextItemCollection;
using ContextItem = com.sentillion.sdkweb.webcontextor.ContextItem;

namespace CCOWTest
{
    /// <summary>
    /// Summary description for ContextValues
    /// </summary>
    [DataObject(true)]
    public static class ContextValues
    {

        /// <summary>
        /// Select object for turning a ContextItemCollection into
        /// a data table
        /// </summary>
        /// <param name="context">a context item collection</param>
        /// <returns>a data table with one row per context item</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static DataTable GetCollection(object context)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            if (context == null) return table;

            ContextItemCollection wcic = (ContextItemCollection)context;
            for (int i = 0; i < wcic.Count(); i++)
            {
                ContextItem ci = wcic.Item(i + 1);
                object[] obj = new object[2];
                obj[0] = ci.Name;
                obj[1] = ci.Value;
                table.Rows.Add(obj);
            }
            return table;
        }
    }
}
