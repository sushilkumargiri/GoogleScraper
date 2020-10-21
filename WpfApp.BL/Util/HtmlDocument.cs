using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Resolvers;

namespace WpfApp.BL.Util
{
    /// <summary>An HTML page as a XDocument. Use <see cref="ParseHtml"/> to load html, <see cref="WriteHtml"/> to write</summary>
    public class HtmlDocument : XDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlDocument"/> class.
        /// </summary>
        /// <param name="other">The <see cref="T:System.Xml.Linq.XDocument" /> object that will be copied.</param>
        public HtmlDocument(XDocument other)
            : base(other)
        {
        }

        /// <summary>
        /// The namespace. Also select elements with this name (eg doc.XHtml + "body")
        /// </summary>
        public XNamespace XHtml { get; set; }

        /// <summary>
        /// Namespace manager used for XPath queries
        /// </summary>
        public XmlNamespaceManager Ns { get; internal set; }

        /// <summary>
        /// Convert a string of html into an XDocument. HTML must be well-formed xml, but could have any common Doctype including Html5 (or no DocType)
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlDocument ParseHtml(string html)
        {

            string htmlString = html;
            int index = htmlString.LastIndexOf("</div>");
            if (index > 0)
                htmlString = htmlString.Substring(0, index+ "</div>".Length);
            index = htmlString.IndexOf("<div");
            if (index > 0)
                htmlString = htmlString.Substring(index,htmlString.Length - index); // or index + 1 to keep slash

            var doc1 = XDocument.Parse(htmlString);

            //html = SanitizeHtml(html);
            //var doc2 = XDocument.Parse(html);

            using (var reader = XmlReader.Create(new StringReader(htmlString.Trim()), XmlReaderSettings))
            {
                var xDocument = Load(reader, LoadOptions.PreserveWhitespace);
                var doc = new HtmlDocument(xDocument);
                if (doc.Root != null)
                {
                    //get the xmlns (maybe absent or not official html)
                    var ns = doc.Root.Name.Namespace;
                    doc.XHtml = ns;
                }
                if (reader.NameTable != null)
                {
                    doc.Ns = new XmlNamespaceManager(reader.NameTable);
                    doc.Ns.AddNamespace("html", doc.XHtml.ToString());
                }

                //InternalSubset is an empty string and should be null (other dtd has "[]" at the end)
                if (doc.DocumentType != null && string.IsNullOrEmpty(doc.DocumentType.InternalSubset))
                {
                    doc.DocumentType.InternalSubset = null;
                }

                return doc;
            }
        }

        static string SanitizeHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) throw new ArgumentNullException("html");
            html = html.Trim();

            html = html.Replace("&", "&amp;");
            html = html.Replace("<!doctype html>", "");
            return html;
        }

        /// <summary>
        /// Writes the HTML. Only xml named entities plus nbsp are explicitly written.
        /// </summary>
        /// <returns></returns>
        public string WriteHtml()
        {
            var html = ToString();
            //does it have our marker comment?
            var startMarker = html.IndexOf("<!-- HtmlDocument-", StringComparison.OrdinalIgnoreCase);
            if (startMarker != -1)
            {
                var endMarker = html.IndexOf(">", startMarker, StringComparison.OrdinalIgnoreCase) + 1;
                var marker = html.Substring(startMarker, endMarker - startMarker);
                //remove the temporary DTD and marker comment
                html = html.Substring(endMarker).Trim();
                if (string.Equals(marker, "<!-- HtmlDocument-DOCTYPE html -->", StringComparison.OrdinalIgnoreCase))
                {
                    //reinsert the html5 doctype
                    html = "<!DOCTYPE html>\r\n" + html;
                }
                else if (string.Equals(marker, "<!-- HtmlDocument-NoDOCTYPE -->", StringComparison.OrdinalIgnoreCase))
                {
                    //no doctype
                }
            }
            //for non-breaking space only, show the named entity.
            html = html.Replace("\xA0", "&nbsp;");
            //for other entities, use EntitizeHtml method
            EntitizeHtml(html);

            return html;
        }

        /// <summary>
        /// Replaces resolved characters with the corresponding html named entity
        /// </summary>
        public static string EntitizeHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) throw new ArgumentNullException("html");

            var entities = LoadHtmlEntities();
            foreach (var keyPair in entities)
            {
                var ch = (char)keyPair.Key;
                html = html.Replace(ch.ToString(), "&" + keyPair.Value + ";");
            }
            return html;
        }

        static Dictionary<int, string> LoadHtmlEntities()
        {
            var entities = new Dictionary<int, string>();
            using (
                var stream =
                    System.Reflection.Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("Library.ParseXHtml.xhtml-entities.ent"))
            {
                if (stream == null) return entities;
                using (var sr = new StreamReader(stream))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!line.StartsWith("<!ENTITY", StringComparison.OrdinalIgnoreCase)) continue;
                        var q1 = line.IndexOf("\"&#", StringComparison.Ordinal);
                        var q2 = line.IndexOf(";\"", StringComparison.Ordinal);
                        if (q1 == -1 || q2 == -1) continue;
                        var ent = line.Substring(9, q1 - 11).Trim();
                        if (ent == "quot" || ent == "gt" || ent == "lt" || ent == "amp" || ent == "apos")
                        {
                            continue; //done automatically by the XmlWriter in ToString
                        }
                        var v = line.Substring(q1 + 3, q2 - q1 - 3);
                        var key = int.Parse(v, CultureInfo.InvariantCulture);
                        entities.Add(key, ent);
                    }
                }
            }
            return entities;
        }

        static XmlReaderSettings XmlReaderSettings
        {
            get
            {
                var readerSettings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Parse,
                    XmlResolver = new XmlPreloadedResolver(XmlKnownDtds.Xhtml10),
                };
                return readerSettings;
            }
        }
    }
}
