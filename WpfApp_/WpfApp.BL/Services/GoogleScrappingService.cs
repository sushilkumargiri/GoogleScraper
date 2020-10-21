
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using WpfApp.BL.Dto;
using WpfApp.BL.Services.Interfaces;
using WpfApp.BL.Static;
using WpfApp.BL.Util;
using WpfApp.HtmlParser.HtmlDocumentStructure;

namespace WpfApp.BL.Services
{
    public class GoogleScrappingService : IGoogleScrappingService
    {
        /// <summary>
        /// construch search url and call method to scrap web page
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="searchLink"></param>
        /// <returns></returns>
        public string ScrapGoogle(string searchLink, string searchText)
        {
            string webUrl = ConstructSearchURL(searchText);

            return Scrap(webUrl, searchLink);
        }

        /// <summary>
        /// main method to scrap google and find position of a particular URL
        /// </summary>
        /// <param name="webUrl"></param>
        /// <param name="searchLink"></param>
        /// <returns></returns>
        static string Scrap(string webUrl,string searchLink)
        {
            List<string> list = new List<string>();

            //searching for links
            StringBuilder builder = new StringBuilder();

            byte[] ResultsBuffer = new byte[8192];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webUrl);
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();

            Stream resStream = resp.GetResponseStream();
            string tempString = null;

            int count = 0;
            do
            {
                count = resStream.Read(ResultsBuffer, 0, ResultsBuffer.Length);
                if (count != 0)
                {
                    tempString = Encoding.ASCII.GetString(ResultsBuffer, 0, count);
                    builder.Append(tempString);
                }
            }
            while (count > 0);

            string html = builder.ToString();

            HtmlParser.HtmlDocumentStructure.HtmlDocument docx = new HtmlParser.HtmlDocumentStructure.HtmlDocument(html);
            docx.Parse();
            var allNodes = (List<HtmlDocumentNode>)docx.RootNode.Descendants;
            HtmlDocumentNode body = allNodes
                .Where(html => html.OwnHtml.StartsWith(Constants.BODY_START)).SingleOrDefault();


            var anchors = GetLinksFromSearchResult(body);
            
            var positions = GetSearchURLPositions(anchors, searchLink);
            return string.IsNullOrEmpty(positions) ? "0" : positions;
        }

        /// <summary>
        /// Get all positions of search URL found 
        /// </summary>
        /// <param name="anchors"></param>
        /// <param name="searchLink"></param>
        /// <returns></returns>
        public static string GetSearchURLPositions(string[] anchors, string searchLink)
        {
            List<string> foundPositions = new List<string>();
            int i = 0;
            foreach(var anchor in anchors)
            {
                i++;
                if (anchor != null && anchor.Contains(searchLink))
                {
                    foundPositions.Add(i.ToString());
                }
            }
            return string.Join(',', foundPositions);
        }
        /// <summary>
        /// get links found only from search result section of the page
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static string[] GetLinksFromSearchResult(HtmlDocumentNode bodyNode)
        {
            var allLinks = bodyNode.Descendants.Where(d => d.OwnHtml.StartsWith(Constants.A_START)).ToArray();
            int i = 0;
            string[] links = new string[200];
            foreach (HtmlDocumentNode childNode in allLinks)
            {
                //Note: comment out below "if condition" to find all appearances of the searchLink(including in ads or social networking pages)
                if (GetAttributeValueByName(childNode, Constants.HREF).StartsWith(Constants.A__START_URL))
                {
                    links[i] = childNode.OuterHtml;
                    i++;
                }
            }
            return links;
        }
        /// <summary>
        /// get attribute value of a node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public static string GetAttributeValueByName(HtmlDocumentNode node, string attrName)
        {
            string val = string.Empty;
            if (node.Attributes.Any(a => a.Name == attrName))
            {
                val = node.Attributes.First(a => a.Name == attrName).Value;
            }
            return val;
        }

        /// <summary>
        /// construct the search web URL
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public string ConstructSearchURL(string searchText)
        {
            return Constants.SEARCH_URL + "search?num=" + Constants.MAX_Page_Size + "&q=" + searchText;
        }
    }

}