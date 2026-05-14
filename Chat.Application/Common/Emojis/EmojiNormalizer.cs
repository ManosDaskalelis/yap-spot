using Chat.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Application.Common.Emojis
{
    public static class EmojiNormalizer
    {
        private static readonly Dictionary<string, string> EmojiMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["thumbs_up"] = "\uD83D\uDC4D", // 👍
            ["thumbs_down"] = "\uD83D\uDC4E", // 👎
            ["joy"] = "\uD83D\uDE02", // 😂
            ["fire"] = "\uD83D\uDD25", // 🔥
            ["heart"] = "\u2764\uFE0F", // ❤️
            ["tada"] = "\uD83C\uDF89", // 🎉
            ["surprised"] = "\uD83D\uDE2E", // 😮
            ["sad"] = "\uD83D\uDE22"  // 😢
        };

        public static string Normalize(string input)
        {
            var normalized = input.Trim();

            if (EmojiMap.TryGetValue(normalized, out var emoji))
                return emoji;

            if (EmojiMap.ContainsValue(normalized))
                return normalized;

            throw new NotFoundException($"Emoji {input} is not supported");
        }
    }
}
