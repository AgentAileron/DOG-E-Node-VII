/* String Utility methods for DN7
 * DOG-E Node VII
 * AgentAileron 2018
*/

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Reg{

    public class StrUtil{
        
        // Format string as inline codeblock
        public static string InlineCode(string instring){
            return ("`" + instring + "`");
        }

        // Format string as multiline codeblock
        public static string CodeBlock(string instring){
            return ("``` " + instring + "\n```");
        }

        // Format string as italic
        public static string Italic(string instring){
            return ("*" + instring + "*");
        }

        // Format string as bold
        public static string Bold(string instring){
            return ("**" + instring + "**");
        }

        // Format string as bold + italic
        public static string BoldItalic(string instring){
            return ("***" + instring + "***");
        }

        // Format string with strikethrough
        public static string strikethrough(string instring){
            return ("~~" + instring + "~~");
        }

        // Format string with underline
        public static string Underline(string instring){
            return ("__" + instring + "__");
        }

        // Format string as diff codeblock
        public static string DiffBlock(string instring){
            return ("```diff\n" + instring + "\n```");
        }

        // Format string as css codeblock
        public static string CSSBlock(string instring){
            return ("```css\n" + instring + "\n```");
        }

        // Format string as markdown codeblock
        public static string MdBlock(string instring){
            return ("```md\n" + instring + "\n```");
        }

        // Format string as fix codeblock
        public static string FixBlock(string instring){
            return ("```fix\n" + instring + "\n```");
        }

        // Format string as xl codeblock
        public static string XlBlock(string instring){
            return ("```xl\n" + instring + "\n```");
        }

    } // Class boundary
} // NameSpace boundary