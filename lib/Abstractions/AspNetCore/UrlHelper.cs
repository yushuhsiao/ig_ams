using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Mvc.Routing
{
    public static class UrlHelperExtensions
    {
        private static readonly char[] ValidAttributeWhitespaceChars = new[] { '\t', '\n', '\u000C', '\r', ' ' };

        public static bool TryResolveUrl(this ActionContext context, string url, out string resolvedUrl)
        {
            resolvedUrl = null;
            var start = FindRelativeStart(url);
            if (start == -1)
            {
                return false;
            }

            var trimmedUrl = CreateTrimmedString(url, start);

            IUrlHelperFactory urlHelperFactory = context.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();

            var urlHelper = urlHelperFactory.GetUrlHelper(context);

            resolvedUrl = urlHelper.Content(trimmedUrl);

            return true;
        }

        private static int FindRelativeStart(string url)
        {
            if (url == null || url.Length < 2)
            {
                return -1;
            }

            var maxTestLength = url.Length - 2;

            var start = 0;
            for (; start < url.Length; start++)
            {
                if (start > maxTestLength)
                {
                    return -1;
                }

                if (!IsCharWhitespace(url[start]))
                {
                    break;
                }
            }

            // Before doing more work, ensure that the URL we're looking at is app-relative.
            if (url[start] != '~' || url[start + 1] != '/')
            {
                return -1;
            }

            return start;
        }

        private static string CreateTrimmedString(string input, int start)
        {
            var end = input.Length - 1;
            for (; end >= start; end--)
            {
                if (!IsCharWhitespace(input[end]))
                {
                    break;
                }
            }

            var len = end - start + 1;

            // Substring returns same string if start == 0 && len == Length
            return input.Substring(start, len);
        }

        private static bool IsCharWhitespace(char ch)
        {
            for (var i = 0; i < ValidAttributeWhitespaceChars.Length; i++)
            {
                if (ValidAttributeWhitespaceChars[i] == ch)
                {
                    return true;
                }
            }
            // the character is not white space
            return false;
        }
    }
}
