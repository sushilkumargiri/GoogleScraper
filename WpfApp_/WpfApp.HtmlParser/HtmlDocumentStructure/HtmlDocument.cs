namespace WpfApp.HtmlParser.HtmlDocumentStructure
{
    public class HtmlDocument
    {
        public HtmlDocumentNode RootNode { get; private set; }
        public string DocumentHtml { get; }
        public HtmlParserConfiguration Configuration { get; set; }
        private HtmlParser _htmlParser;

        public HtmlDocument(string documentHtml)
        {
            DocumentHtml = documentHtml;
            Configuration = new HtmlParserConfiguration();
        }

        public void Parse()
        {
            _htmlParser = new HtmlParser(DocumentHtml, Configuration);
            _htmlParser.ParseHtml();
            RootNode = _htmlParser.RootNode;
        }
    }
}
