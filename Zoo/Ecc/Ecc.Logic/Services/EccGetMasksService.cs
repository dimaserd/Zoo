using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Croco.Core.Contract;

namespace Ecc.Logic.Services
{
    public class EccGetMasksService
    {
        static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text[(pos + search.Length)..];
        }

        public static string ProcessTextViaFunctions(string text, string interactionId, ICrocoAmbientContextAccessor ambientContext, Dictionary<string, IEccTextFunctionInvoker> funcs)
        {
            var replacings = GetReplacings(text);

            foreach (var replacing in replacings)
            {
                if (replacing.Func != null && funcs.ContainsKey(replacing.Func.Name))
                {
                    var func = funcs[replacing.Func.Name];

                    text = ReplaceFirst(text, replacing.TextToReplace, func.ProccessText(interactionId, replacing));
                }
            }

            return text;
        }

        static List<string> GetMasks(string text)
        {
            var regex = new Regex("{{.*?}}", RegexOptions.Compiled);

            var matches = regex.Matches(text);

            return matches.Cast<Match>().Select(match => match.Value).ToList();
        }

        static List<EccReplacing> GetReplacings(string text)
        {
            var masks = GetMasks(text);

            return masks.Select(ToReplacing).ToList();
        }

        public static EccReplacing ToReplacing(string matchedText)
        {
            var substr = matchedText[2..^2].Trim();

            var isFunc = substr.Contains("(") && substr.Contains(")");

            var res = new EccReplacing
            {
                TextToReplace = matchedText
            };

            if (isFunc)
            {
                var funcName = substr.Substring(0, substr.IndexOf('('));

                var str = substr[(substr.IndexOf('(') + 1)..];
                str = str.Substring(0, str.IndexOf(')'));

                var args = str.Split(',').Select(x => x.Trim()).ToList();

                res.Func = new EccTextFunc
                {
                    Args = args.ToList(),
                    Name = funcName
                };
            }

            return res;
        }
    }
}