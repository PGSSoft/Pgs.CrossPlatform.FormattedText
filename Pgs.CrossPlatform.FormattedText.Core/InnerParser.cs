using System.Collections.Generic;
using System.Text;

namespace Pgs.CrossPlatform.FormattedText.Core
{
    internal static class InnerParser
    {
        internal static void Parse(HashSet<FormatParameters> styleParameters, ref string text, char tagStartChar = '<', char tagEndChar = '>')
        {
            int ignoreIt = 0;
            var stringBuilder = new StringBuilder(text);
            for (int i = 0; i < text.Length; i++)
            {
                var subNode = RecursiveParse(styleParameters, stringBuilder, ref i, ref ignoreIt, tagStartChar, tagEndChar, true);

                if(subNode != null)
                    styleParameters.Add(subNode);
            }
            styleParameters.RemoveWhere(x => x == null);
            text = stringBuilder.ToString();
        }

        private static int _recursiveLvl = 0;
        internal static FormatParameters RecursiveParse(HashSet<FormatParameters> styleParameters, StringBuilder text, ref int i, ref int removedTagsLength, char tagStartChar, char tagEndChar, bool isFirstIteration = false)
        {
            if (i >= text.Length)
                return null;

            var currentStyleParam = CheckCharIsBeginningTag(text, ref i, ref removedTagsLength, tagStartChar, tagEndChar);

            if (currentStyleParam == null)
                return null;

            var startI = i;
            var innerRemovedTagsLenght = 0;
            _recursiveLvl++;

            while (true)
            {
                var innerNode = RecursiveParse(styleParameters, text, ref i, ref innerRemovedTagsLenght, tagStartChar, tagEndChar);
                if (innerNode != null)
                {
                    styleParameters.Add(innerNode);
                }
                else if (CheckCharIsEndingTag(text, ref i, ref removedTagsLength, tagStartChar, tagEndChar))
                {
                    if (isFirstIteration)
                    {
                        currentStyleParam.Length = i - startI;
                    }
                    else
                    {
                        currentStyleParam.Length = i - startI + (_recursiveLvl > 1 ? 0 : innerRemovedTagsLenght);
                    }
                    removedTagsLength += innerRemovedTagsLenght;
                    _recursiveLvl--;
                    return currentStyleParam;
                }
                else
                {
                    i++;
                }
            }
        }

        internal static bool CheckCharIsEndingTag(StringBuilder text, ref int i, ref int removedTagsLength, char tagStartChar, char tagEndChar)
        {
            if (i + 1 < text.Length && text[i] == tagStartChar && text[i + 1] == '/')
            {
                removedTagsLength += RemoveTag(ref i, text, tagEndChar);
                return true;
            }
            return false;
        }

        internal static FormatParameters CheckCharIsBeginningTag(StringBuilder text, ref int i, ref int removedTagsLength, char tagStartChar, char tagEndChar)
        {
            if (text[i] == tagStartChar && text[i + 1] != '/')
            {
                var styleParam = new FormatParameters(GetTagName(text.ToString(), i, tagEndChar), i, 0);
                removedTagsLength += RemoveTag(ref i, text, tagEndChar);
                return styleParam;
            }
            return null;
        }

        private static int RemoveTag(ref int i, StringBuilder text, char tagEndChar)
        {
            var startI = i;
            while (text[++i] != tagEndChar) { }
            i++; // to skip tagEndChar

            // tag deleting
            var removedCharsCount = i - startI;
            text = text.Remove(startI, removedCharsCount);
            i = startI;
            return removedCharsCount;
        }

        internal static string GetTagName(string text, int startI, char tagEndChar)
        {
            int i = startI;
            while (i < text.Length && text[i] != tagEndChar)
            {
                i++;
            }
            var returnText = text.Substring(startI + 1, i - startI - 1);
            startI = i + 1 == text.Length ? i : i + 1; // + 1 to skip ']' char
            return returnText;
        }
    }
}
