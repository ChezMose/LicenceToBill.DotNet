///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LicenceToBill.Api.Tools
{
    /// <summary>
    /// Serializer for JSON
    /// 
    /// Offensive : throw an exception if serialization / deserialization failed
    /// Do not forget the DataContract & DataMember attributes on your objects
    /// </summary>
    public static class SerializerForJson
    {
        #region Public

        /// <summary>
        /// Deserialize the content from given stream
        /// </summary>
        public static TContent Deserialize<TContent>(Stream stream)
            where TContent : class
        {
            TContent result = null;

            // if we got a stream
            if (stream != null)
            {
                // convert the input stream as a StreamReader
                var textReader = new StreamReader(stream);
                // open the Json Reader
                var reader = new JsonTextReader(textReader);
                // create the serializer
                var serializer = new JsonSerializer();
                // deserialize
                result = (serializer.Deserialize(reader, typeof (TContent)) as TContent);
            }
            return result;
        }
        /// <summary>
        /// Serailize the content into given stream
        /// </summary>
        public static bool Serialize<TContent>(TContent content, Stream stream)
            where TContent : class
        {
            var result = false;

            // if we got a content
            if(content != null)
            {
                // prepare the settings
                var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore
                                    };
                settings.Converters.Add(new IsoDateTimeConverter());

                // create the serializer
                var serializer = JsonSerializer.Create(settings);
                // convert the output stream as an encoded StreamWriter
                using(var textWriter = new StreamWriter(stream))
                {
                    // open the Json Writer
                    using (var writer = new JsonTextWriter(textWriter))
                    {
                        // we want indented json in output
                        writer.Formatting = Formatting.Indented;

                        // serialize
                        serializer.Serialize(writer, content);
                        // flush the writer
                        writer.Flush();
                    }
                }
                // success
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Serailize the content into given stream
        /// </summary>
        public static string Serialize<TContent>(TContent content)
            where TContent : class
        {
            var result = new StringBuilder(200);

            // if we got a content
            if(content != null)
            {
                // prepare the settings
                var settings = new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore
                                    };
                settings.Converters.Add(new IsoDateTimeConverter());

                // create the serializer
                var serializer = JsonSerializer.Create(settings);
                // convert the output stream as an encoded StreamWriter
                using(var textWriter = new StringWriter(result))
                {
                    // open the Json Writer
                    using (var writer = new JsonTextWriter(textWriter))
                    {
                        // we want indented json in output
                        writer.Formatting = Formatting.Indented;

                        // serialize
                        serializer.Serialize(writer, content);
                        // flush the writer
                        writer.Flush();
                    }
                }
            }
            return result.ToString();
        }

        #endregion
    }
}