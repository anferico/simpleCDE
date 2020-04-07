using System.Collections.Generic;
using System.Text;

namespace simpleCDE 
{
    public class LexicalAnalyzer 
    {
        private string inputString;
        private int cursorPosition = 0;
        private List<char?> specialCharacters = new List<char?>() {
            '-', '[', ']', ',', ':', ' ', '{', '}', null
        };
        private Token? lookahead;
        public string TokenValue;
        private string nextTokenValue;

        public LexicalAnalyzer(string inputString) 
        {
            this.inputString = inputString;
        }

        private char? Peek() 
        {
            if (inputString.Length > cursorPosition)
            {
                return inputString[cursorPosition];
            }
        
            return null;
        }

        private void BuildNextToken() 
        {
            char? currentChar = Peek();
            nextTokenValue = null;

            switch (currentChar) 
            {
                case '-': 
                    cursorPosition++; 
                    lookahead = Token.DASH;
                    break;

                case '[': 
                    cursorPosition++; 
                    lookahead = Token.SQROPEN;
                    break;

                case ']': 
                    cursorPosition++; 
                    lookahead = Token.SQRCLOSE;
                    break;

                case ',': 
                    cursorPosition++; 
                    lookahead = Token.COMMA;
                    break;

                case ':': 
                    cursorPosition++; 
                    lookahead = Token.COLON;
                    break;

                case ' ': 
                    cursorPosition++; 
                    lookahead = Token.WHITESPACE;
                    break;

                case '{': 
                    cursorPosition++; 
                    lookahead = Token.INDENTINC;
                    break;

                case '}': 
                    cursorPosition++; 
                    lookahead = Token.INDENTDEC;
                    break;

                case null: 
                    lookahead = Token.ENDOFINPUT; 
                    break;

                default:
                    var strBuilder = new StringBuilder();
                    while (currentChar != null && !specialCharacters.Contains(currentChar)) 
                    {
                        strBuilder.Append(currentChar);
                        cursorPosition++;
                        currentChar = Peek();
                    }
                    string parsedString = strBuilder.ToString();
                    switch (parsedString) 
                    {
                        case "services": 
                            lookahead = Token.SERVICES;
                            break;

                        case "agents": 
                            lookahead = Token.AGENTS;
                            break;

                        case "relations": 
                            lookahead = Token.RELATIONS;
                            break;

                        case "units": 
                            lookahead = Token.UNITS;
                            break;

                        case "class": 
                            lookahead = Token.CLASS;
                            break;

                        case "events": 
                            lookahead = Token.EVENTS;
                            break;

                        case "after": 
                            lookahead = Token.AFTER;
                            break;

                        case "handler": 
                            lookahead = Token.HANDLER;
                            break;

                        case "requires": 
                            lookahead = Token.REQUIRES;
                            break;

                        case "provides": 
                            lookahead = Token.PROVIDES;
                            break;

                        default: 
                            lookahead = Token.LITERAL; 
                            nextTokenValue = parsedString;
                            break;
                    }
                    break;
            }
        }

        public Token GetNextToken() 
        {
            Token nextToken = Lookahead;
            TokenValue = nextTokenValue;
            BuildNextToken();
            return nextToken;
        }

        public Token Lookahead 
        {
            get
            {   
                if (lookahead == null) 
                {
                    BuildNextToken();
                }
                return (Token) lookahead;
            }
        }
    }
}
