using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    public class SignatureGenerator
    {
        public static string GenerateSignature(SignatureModel model, string tpToken)
        {
            var queryString = GetQueryString(model, tpToken);

            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(queryString);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private static string GetQueryString(SignatureModel model, string tpToken)
        {
            var sb = new StringBuilder();
            var sortedDict = new SortedDictionary<string, string>();
            PropertyInfo[] properties = typeof(SignatureModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var displayName = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single().DisplayName;
                var value = property.GetValue(model);
                if (displayName.Equals("need_tag") && value.ToString().Equals("0")) continue;
                if (value == null || string.IsNullOrEmpty(value.ToString())) continue;

                sortedDict.Add(displayName, value.ToString());
            }


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
