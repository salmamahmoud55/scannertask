using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace task
{
    internal class task_scanner
    {
        //TOKEN LIST 
        static List<(string type, string pattern)> tokenSpecification = new List<(string, string)>
        {
          ("COMMENT",          @"//.*"),                                      //single line comment
          ("KEYWORDS",         @"\b(int|float|printf|for|string|if)\b"),      //keywords
          ("IDENTIFIERS",      @"\b[a-zA-Z_][a-zA-Z_0-9]*\b"),                //identifers
          ("NUMCONSTANTS",     @"\b\d+(\.\d+)?(e[+-]?\d+)?\b"),               //Numeric constants
          ("OPERATORS",        @"[+\-*/=]"),                                  //Operators
          ("SPECIALCHAR",      @"[{}();]"),                                   //Special characters
          ("WHITESPACE",       @"\s+"),
          
        };


        public static List<(string type, string value)> Scan(string code)
        {
            List<(string type, string value)> tokens = new List<(string, string)>();
            string combinedPattern = string.Join("|", tokenSpecification.ConvertAll(spec => $"(?<{spec.type}>{spec.pattern})"));

            foreach (Match match in Regex.Matches(code, combinedPattern))
            {
                foreach (var spec in tokenSpecification)
                {
                    //ignore whitespace
                    if (match.Groups[spec.type].Success && spec.type != "WHITESPACE")            
                    {
                        tokens.Add((spec.type, match.Value));
                        break;
                    }
                }
            }

            return tokens;
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Enter a statement to scan for tokens:");
            string statement = Console.ReadLine();             //for ( count = 1 ; count = x2 + 3.4e+6 ; count = count + 1 ) //outer loop مثال من المحاضرة

            var tokens = Scan(statement);

            foreach (var token in tokens)
            {
                Console.WriteLine($"( {token.type} , {token.value} )");
            }
        }



    }
}
