using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApp1
{
    public class SignatureGeneratorV2
    {
        public static string GenerateSignature(SignatureModel model, string tpToken)
        {
            var queryString = GetQueryString(model, tpToken);

            // Use input string to calculate MD5 hash
            Span<byte> bytes = queryString.Length < 1024 ? stackalloc byte[queryString.Length * 2] : new byte[queryString.Length * 2]; // x2 in case ScenceId is Chinese string, but read through the code, ScenceId just not used.
            int byteLen = Encoding.UTF8.GetBytes(queryString, bytes);
            bytes = bytes[0..byteLen];

            Span<byte> hashBytes = stackalloc byte[16];
            System.Security.Cryptography.MD5.HashData(bytes, hashBytes);

            ref decimal bValue = ref MemoryMarshal.AsRef<decimal>(hashBytes);

            // Convert the byte array to hexadecimal string
            var signature = string.Create(32, bValue, static (chars, state) =>
            {
                var tempSpan = MemoryMarshal.CreateReadOnlySpan(ref state, 1);
                var bytes = MemoryMarshal.Cast<decimal,byte>(tempSpan);
                ReadOnlySpan<char> format = "x2";
                for (int i = 0; i < bytes.Length; i++)
                {
                    var slice = chars[(i * 2)..];
                    bytes[i].TryFormat(slice, out int written, format);
                }
            });
            return signature;
        }

        private static string GetQueryString(SignatureModel model, string tpToken)
        {
            var sortedDict = new SortedDictionary<string, string>()
            {
                ["game_id"] = model.UserAppId.ToString(),
                ["auth_type"] = model.AuthType.ToString(),
                ["source"] = model.Source.ToString()
            };
            if (model.NeedTag != 0)
            {
                sortedDict.Add("need_tag", model.NeedTag.ToString());
            }
            if (!string.IsNullOrEmpty(model.SceneId))
            {
                sortedDict.Add("scene_id", model.SceneId);
            }

            var sb = new StringBuilder();
            foreach (var key in sortedDict.Keys)
            {
                if (sb.Length != 0)
                {
                    sb.Append("&");
                }
                sb.Append(key);
                sb.Append("=");
                sb.Append(sortedDict[key]);
            }

            sb.Append("&tp_token=");
            sb.Append(tpToken);

            return sb.ToString();
        }
    }
}
