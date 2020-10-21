﻿using WpfApp.HtmlParser.HtmlDocumentStructure;

namespace WpfApp.HtmlParser.HtmlParsers
{
    public class SpecialTagParser : ITagParser
    {
        
        public HtmlDocumentNode ParsedNode {get;set;}

        private readonly TextFormatter _source;
        private readonly SpecialTagParserConfiguration _configuration;
        private readonly string _name; 

        public SpecialTagParser(string name, TextFormatter source, SpecialTagParserConfiguration configuration)
        {
            _name = name;
            _source = source;
            _configuration = configuration;
        }

        public bool CanParse()
        {
            return _source.IsTextOnCurrentPosition(_configuration.TagOpener, _configuration.CaseSensitive);
        }

        public void Parse()
        {
            ParsedNode = new HtmlDocumentNode
            {
                Name = _name,
                Position = _source.Position,
                Line = _source.Line
            };

            ParsedNode.Flags.Add(Flags.SpecialTag);

            AddAndSkipTagOpener();          
            AddAndSkipTagContent();
            AddAndSkipTagCloser();
        }

        public void AddAndSkipTagOpener()
        {
            ParsedNode.OuterHtml = _configuration.TagOpener;
            _source.SkipText(_configuration.TagOpener, _configuration.CaseSensitive);
        }

        public void AddAndSkipTagCloser()
        {
            ParsedNode.OuterHtml += _configuration.TagCloser;
            _source.SkipText(_configuration.TagCloser, _configuration.CaseSensitive);
        }

        public void AddAndSkipTagContent()
        {
            ParsedNode.OwnText += _source.GetTextFromCurrentPositionToAnyStopString(_configuration.TagCloser);
            ParsedNode.OuterHtml += ParsedNode.OwnText;
        }
    }

    public class SpecialTagParserConfiguration
    {
        public string TagOpener {get;set;}
        public string TagCloser {get;set;}

        public bool CaseSensitive {get;set;}

        public SpecialTagParserConfiguration(string tagOpener, string tagCloser, bool caseSensitive)
        {
            TagOpener = tagOpener;
            TagCloser = tagCloser;
            CaseSensitive = caseSensitive;
        }
    }
}
