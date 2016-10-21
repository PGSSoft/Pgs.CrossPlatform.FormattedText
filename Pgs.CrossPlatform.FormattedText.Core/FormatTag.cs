using System;
using System.Reflection;

namespace Pgs.CrossPlatform.FormattedText.Core
{
    /// <summary>
    /// Delegate taken by SpanTag as stylingMethod
    /// </summary>
    /// <param name="obj">The object of type provide as OutT in Parse method in SpanParser.</param>
    /// <param name="i1">The formating start index in object's text.</param>
    /// <param name="i2">The formating end index in object's text.</param>
    public delegate void TagStylingMethod(object obj, object sourceControl, int i1, int i2);

    /// <summary>
    /// Model for passing each tag config to SpanParser
    /// </summary>
    public class FormatTag
    {
        /// <summary>
        /// Tag name
        /// </summary>
        public readonly string Tag;

        /// <summary>
        /// The method to call when this tag is requested
        /// </summary>
        public readonly TagStylingMethod MethodToCall;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatTag"/> class.
        /// Assigns tag name to method that apply that styling
        /// </summary>
        /// <param name="tag">The tag name.</param>
        /// <param name="stylingMethod">The styling method.</param>
        /// <exception cref="System.ArgumentNullException">StylingMethod cannot be null!</exception>
        public FormatTag(string tag, TagStylingMethod stylingMethod)
        {
            // TODO: it's not necessary but need to test if it does some additional problems without it; commented as it can be performance hit during Parser initialization
            //Regex r = new Regex("^[a-zA-Z0-9]*$");
            //if (String.IsNullOrEmpty(tag) || !r.IsMatch(tag))
            //    throw new ArgumentException($"Tag must be alphanumeric and not empty. Given: {tag}");

            if (stylingMethod == null)
                throw new ArgumentNullException("StylingMethod cannot be null!");

            Tag = tag;
            MethodToCall = stylingMethod; 
        }
    }
}
