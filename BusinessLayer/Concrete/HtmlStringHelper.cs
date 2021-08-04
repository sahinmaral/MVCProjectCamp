using System;
using System.Text.RegularExpressions;

namespace BusinessLayer.Concrete
{
    public static class HtmlStringHelper
    {
        public static string RemoveHtmlTags(string entity)
        {
            return Regex.Replace(entity, "<.*?>", String.Empty);
        }
    }
}
