using System;
using System.Collections;
using System.Text;
using System.Web.Mvc;

namespace LicenceToBill.Web
{
    /// <summary>
    /// Result for action : CSV
    /// </summary>
    public class CsvResult : ActionResult
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CsvResult(IEnumerable enumerable, string filename = null)
        {
            // convert
            this._csv = ToCsv(enumerable);
            this._filename = filename;
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public CsvResult(string csv, string filename = null)
        {
            this._csv = csv;
            this._filename = filename;
        }
        /// <summary>
        /// content
        /// </summary>
        private string _csv = null;
        /// <summary>
        /// filename
        /// </summary>
        private string _filename = null;
        /// <summary>
        /// ActionResult override
        /// </summary>
        public override void ExecuteResult(ControllerContext context)
        {
            // get teh file name
            var filename = this._filename;
            // if any
            if(!string.IsNullOrEmpty(filename))
            {
                // if the name does not end with ".csv"
                if(!filename.ToLower().EndsWith(".csv"))
                    // then happen it
                    filename += ".csv";
            }
            // if none
            else
                // compute the filename
                filename = context.RouteData.Values["action"] + ".csv";

            // get the respons
            var response = context.HttpContext.Response;
            // define its content type
            response.ContentType = "text/csv";
            // set appropriate header
            response.AddHeader("content-disposition", "Content-Disposition: inline; filename=\"" + filename + "\"");

            // create an encoder
            var encoding = Encoding.GetEncoding("iso-8859-1");

            // get the content as bytes
            var buffer = encoding.GetBytes(this._csv);
            // write our string to the response
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        #region Csv conversion

        /// <summary>
        /// Convert given enumerable to cvs string
        /// </summary>
        public static string ToCsv(IEnumerable enumerable)
        {
            string result = null;

            // if valid
            if(enumerable != null)
            {
                // get the first item
                var enumerator = enumerable.GetEnumerator();
                if(enumerator.MoveNext())
                {
                    // get the first item
                    var item = enumerator.Current;

                    // if any
                    if (item != null)
                    {
                        // call the overload
                        result = ToCsv(enumerable, item.GetType(), ';');
                    }
                }
            }
            // call the overload
            return result;
        }
        /// <summary>
        /// Convert given enumerable to cvs string
        /// </summary>
        private static string ToCsv(IEnumerable enumerable, Type type, char separator)
        {
            var csv = new StringBuilder(2048);

            // get properties
            System.Reflection.PropertyInfo[] piz = type.GetProperties();

            // loop on those
            foreach(var pi in piz)
            {
                csv.Append(pi.Name);
                csv.Append(separator);
            }
            csv.AppendLine();

            // loop on the enumerable
            foreach(object instance in enumerable)
            {
                // loop on the properties
                foreach(var pi in piz)
                {
                    // get the value
                    object value = pi.GetValue(instance, null);
                    // if any
                    if(value != null)
                        // append it
                        csv.Append(value.ToString());
                    // if none
                    else
                        // add a blank
                        csv.Append(' ');

                    csv.Append(separator);
                }
                csv.AppendLine();
            }
            // loop on the 
            return csv.ToString();
        }

        #endregion
    }
}
