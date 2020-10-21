namespace WpfApp.HtmlParser
{
    public class HtmlParserConfiguration
    {   
        public bool IncludeClosingTagsInNodeTree {get;set;}

        public HtmlParserConfiguration()
        {
            IncludeClosingTagsInNodeTree = false;
        }
    }
}