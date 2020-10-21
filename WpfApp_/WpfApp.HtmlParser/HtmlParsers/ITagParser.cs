using WpfApp.HtmlParser.HtmlDocumentStructure;

namespace WpfApp.HtmlParser.HtmlParsers
{
    public interface ITagParser
    {
        HtmlDocumentNode ParsedNode {get;  set;}
        bool CanParse();
        void Parse();
    }
}