using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMail
{
    public class Utils
    {
        public static string DecodeMime(string encoded)
        {
            string decoded = string.Empty;

            // Überprüfen, ob der Text MIME-kodiert ist
            if (encoded.StartsWith("=?") && encoded.EndsWith("?="))
            {
                var parts = encoded.Split(new[] { "?= =?" }, StringSplitOptions.None);

                foreach (string part in parts)
                {
                    var match = System.Text.RegularExpressions.Regex.Match(part, @"=\?([^?]+)\?([bq])\?(.+)\?=");
                    if (match.Success)
                    {
                        string charset = match.Groups[1].Value; // Zeichensatz (z.B. UTF-8)
                        string encoding = match.Groups[2].Value; // Kodierung (B = Base64, Q = Quoted-Printable)
                        string text = match.Groups[3].Value; // Kodierter Inhalt

                        if (encoding.ToLower() == "b") // Base64-Decoder
                        {
                            decoded += Encoding.GetEncoding(charset).GetString(Convert.FromBase64String(text));
                        }
                        else if (encoding.ToLower() == "q") // Quoted-Printable-Decoder
                        {
                            decoded += DecodeQuotedPrintable(text, charset);
                        }
                    }
                }
            }
            else
            {
                decoded = encoded;
            }

            return decoded;
        }

        public static string DecodeQuotedPrintable(string input, string charset)
        {
            Encoding enc = Encoding.GetEncoding(charset);
            var output = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '=' && i + 2 < input.Length)
                {
                    string hex = input.Substring(i + 1, 2);
                    output.Append((char)Convert.ToInt32(hex, 16));
                    i += 2;
                }
                else if (input[i] == '_')
                {
                    output.Append(' ');
                }
                else
                {
                    output.Append(input[i]);
                }
            }
            return enc.GetString(enc.GetBytes(output.ToString()));
        }
    }
}
