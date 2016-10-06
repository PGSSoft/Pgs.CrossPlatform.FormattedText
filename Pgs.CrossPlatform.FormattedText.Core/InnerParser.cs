using System.Collections.Generic;

namespace Pgs.CrossPlatform.FormattedText.Core
{
    internal static class InnerParser
    {
        internal static void Parse(HashSet<StyleParameters> styleParameters, ref string text, char tagStartChar = '<', char tagEndChar = '>')
        {
            Node tree = new Node();

            int ignoreIt = 0;
            for (int i = 0; i < text.Length; i++)
            {
                var subNode = RecursiveParse(ref text, ref i, ref ignoreIt, tagStartChar, tagEndChar, true);

                if(subNode != null)
                    tree.SubNodes.Add(subNode);
            }

            TreeToListRecursive(tree, styleParameters);
            styleParameters.RemoveWhere(x => x == null);
        }

        internal static void TreeToListRecursive(Node node, HashSet<StyleParameters> styleParameters)
        {
            styleParameters.Add(node.StyleParameter);
            foreach (var subNode in node.SubNodes)
            {
                TreeToListRecursive(subNode, styleParameters);
            }
        }

        internal static Node RecursiveParse(ref string text, ref int i, ref int removedTagsLength, char tagStartChar, char tagEndChar, bool isFirstIteration = false)
        {
            var node = CheckCharIsBeginningTag(ref text, ref i, ref removedTagsLength, tagStartChar, tagEndChar);

            if (node != null)
            {
                var startI = i;
                var innerRemovedTagsLenght = 0;

                while (true)
                {
                    var innerNode = RecursiveParse(ref text, ref i, ref innerRemovedTagsLenght, tagStartChar, tagEndChar);
                    if (innerNode != null)
                    {
                        node.SubNodes.Add(innerNode);
                    }
                    else if (CheckCharIsEndingTag(ref text, ref i, ref removedTagsLength, tagStartChar, tagEndChar))
                    {
                        if (isFirstIteration)
                        {
                            node.StyleParameter.Length = i - startI;
                        }
                        else
                        {
                            node.StyleParameter.Length = i - startI + innerRemovedTagsLenght;
                        }
                        removedTagsLength += innerRemovedTagsLenght;
                        return node;
                    }
                    else
                    {
                        i++;
                    }

                }
            }
            return node;
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

        internal static Node CheckCharIsBeginningTag(ref string text, ref int i, ref int removedTagsLength, char tagStartChar, char tagEndChar)
        {
            if (text[i] == tagStartChar && text[i + 1] != '/')
            {
                var node = new Node(new StyleParameters(GetTagName(text, i, tagEndChar), i, 0));
                removedTagsLength += RemoveTag(ref i, ref text, tagEndChar);
                return node;
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
            public StyleParameters StyleParameter { get; private set; }

            public List<Node> SubNodes { get; set; } = new List<Node>();

            public Node(StyleParameters styleParamter)
            {
                StyleParameter = styleParamter;
            }

            public Node()
            {

            }
        }
    }
}
