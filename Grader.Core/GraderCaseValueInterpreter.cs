using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class GraderCaseValueInterpreter
    {
        private readonly GraderSyntaxTree _syntaxTree;
        private readonly GraderParser _parser;

        public GraderCaseValueInterpreter()
        {
            _syntaxTree = new GraderSyntaxTree();
            _parser = new GraderParser();
        }

        public IList<string> ParseInput(string input)
        {
            var result = _parser.Parse(input);
            var treeResult = _syntaxTree.CheckSyntax(result);
            return treeResult.Select(p => p.ValueToMatch).ToList();
        }

        public IList<CaseValue> ParseOutput(string output)
        {
            var result = _parser.Parse(output);
            var treeResult = _syntaxTree.CheckSyntax(result);
            return treeResult;
        }
    }
}