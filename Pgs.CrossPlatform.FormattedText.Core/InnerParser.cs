using System.Collections.Generic;

namespace Pgs.CrossPlatform.FormattedText.Core
{
    internal static class InnerParser
    {
        internal static void Parse(HashSet<FormatParameters> styleParameters, ref string text, char tagStartChar = '<', char tagEndChar = '>')
        {
            int ignoreIt = 0;
            for (int i = 0; i < text.Length; i++)
            {
                var subNode = RecursiveParse(styleParameters, ref text, ref i, ref ignoreIt, tagStartChar, tagEndChar, true);

                if(subNode != null)
                    styleParameters.Add(subNode);
            }
            styleParameters.RemoveWhere(x => x == null);
        }

        internal static FormatParameters RecursiveParse(HashSet<FormatParameters> styleParameters, ref string text, ref int i, ref int removedTagsLength, char tagStartChar, char tagEndChar, bool isFirstIteration = false)
        {
            var currentStyleParam = CheckCharIsBeginningTag(ref text, ref i, ref removedTagsLength, tagStartChar, tagEndChar);

            if (currentStyleParam != null)
            {
                var startI = i;
                var innerRemovedTagsLenght = 0;

                while (true)
                {
                    var innerNode = RecursiveParse(styleParameters, ref text, ref i, ref innerRemovedTagsLenght, tagStartChar, tagEndChar);
                    if (innerNode != null)
                    {
                        styleParameters.Add(innerNode);
                    }
                    else if (CheckCharIsEndingTag(ref text, ref i, ref removedTagsLength, tagStartChar, tagEndChar))
                    {
                        if (isFirstIteration)
                        {
                            currentStyleParam.Length = i - startI;
                        }
                        else
                        {
                            currentStyleParam.Length = i - startI + innerRemovedTagsLenght;
                        }
                        removedTagsLength += innerRemovedTagsLenght;
                        return currentStyleParam;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return currentStyleParam;
        }

        internal static bool CheckCharIsEndingTag(ref string text, ref int i, ref int removedTagsLength, char tagStartChar, char tagEndChar)
        {
            if (i + 1 < text.Length && text[i] == tagStartChar && text[i + 1] == '/')
            {
                removedTagsLength += RemoveTag(ref i, ref text, tagEndChar);
                return true;
            }
            return false;
        }

        internal static FormatParameters CheckCharIsBeginningTag(ref string text, ref int i, ref int removedTagsLength, char tagStartChar, char tagEndChar)
        {
            if (text[i] == tagStartChar && text[i + 1] != '/')
            {
                var styleParam = new FormatParameters(GetTagName(text, i, tagEndChar), i, 0);
                removedTagsLength += RemoveTag(ref i, ref text, tagEndChar);
                return styleParam;
            }
            return null;
        }

        private static int RemoveTag(ref int i, ref string text, char tagEndChar)
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

        internal class Node
        {
            public FormatParameters StyleParameter { get; private set; }

            public List<Node> SubNodes { get; set; } = new List<Node>();

            public Node(FormatParameters styleParamter)
            {
                StyleParameter = styleParamter;
            }

            public Node()
            {

            }
        }
    }
}
