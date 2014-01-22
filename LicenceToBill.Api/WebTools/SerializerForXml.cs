///////////////////////////////////////////////////////////////
// LicenceToBill - brought to you by Mose - 2012
///////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LicenceToBill.Api.Tools
{
    /// <summary>
    /// Serializer for XML
    /// 
    /// Offensive : throw an exception if serialization / deserialization failed
    /// Do not forget the XmlRoot & XmlAttribute & XmlElement attributes on your objects
    /// </summary>
    public static class SerializerForXml
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
                // get the serializer
                var serializer = GetSerializer(typeof (TContent));

                // deserialize
                result = (serializer.Deserialize(stream) as TContent);
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
                // get the serializer
                var serializer = GetSerializer(typeof(TContent));

                // initialize its settings
                var settings = new XmlWriterSettings {OmitXmlDeclaration = true, Indent = true};
                // create an empty namespace declaration to clear all the namespaces in the seralized result
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                // open a writer
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    // serialize
                    serializer.Serialize(writer, content, namespaces);
                    // flush it
                    writer.Flush();
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
                // get the serializer
                var serializer = GetSerializer(typeof(TContent));

                // initialize its settings
                var settings = new XmlWriterSettings {OmitXmlDeclaration = true, Indent = true};
                // create an empty namespace declaration to clear all the namespaces in the seralized result
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                // open a writer
                using (var writer = XmlWriter.Create(result, settings))
                {
                    // serialize
                    serializer.Serialize(writer, content, namespaces);
                    // flush it
                    writer.Flush();
                }
            }
            return result.ToString();
        }

        #endregion

        #region Inner logic

        /// <summary>
        /// Xml serializer cache at it takes some time to be created
        /// </summary>
        private static Dictionary<Type, XmlSerializer> _CacheXmlSerializer = new Dictionary<Type, XmlSerializer>();
        /// <summary>
        /// Get the serailizer for given type
        /// Optimized version (uses caching)
        /// </summary>
        private static XmlSerializer GetSerializer(Type type)
        {
            XmlSerializer result;

            // if not cached
            if(!_CacheXmlSerializer.TryGetValue(type, out result))
            {
                // hack : get the XmlRootAttribute if the item is a collection
                var root = XmlRootForCollection(type);
                // create the serializer
                result = new XmlSerializer(type, root);

                // cache it
                _CacheXmlSerializer[type] = result;
            }
            return result;
        }
        /// <summary>
        /// Get a xml root attribute if the model is a collection
        /// </summary>
        private static XmlRootAttribute XmlRootForCollection(Type type)
        {
            XmlRootAttribute result = null;

            // we have to answer the question : is this type a collection ?
            Type typeInner = null;

            // if the type is generic
            if (type.IsGenericType)
            {
                // get the first argument
                var typeGeneric = type.GetGenericArguments()[0];
                // create a collection with it
                var typeCollection = typeof (ICollection<>).MakeGenericType(typeGeneric);
                // check if the type is a ICollection of that type
                if (typeCollection.IsAssignableFrom(type))
                    // if it's one, get its type
                    typeInner = typeGeneric;
            }
                // if it's not a generic, then is it a collection
            else if (typeof (ICollection).IsAssignableFrom(type)
                // and has it elements ?
                     && type.HasElementType)
            {
                // if its does, get that element type
                typeInner = type.GetElementType();
            }

            // yeepeeh ! if we are working with a collection
            if (typeInner != null)
            {
                // get its XmlType attribute
                var attributes = typeInner.GetCustomAttributes(typeof(XmlTypeAttribute), false);
                // if any
                if(attributes.Length > 0)
                {
                    // get the name and plurilize it
                    var rootName = ((attributes[0]) as XmlTypeAttribute).TypeName + 's';
                    // create the rootname
                    result = new XmlRootAttribute(rootName);
                }
            }
            return result;
        }

        #endregion
    }
}