using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Import
{
    public class SourceCodeReader
    {
        public string ReadClass(string fqdn)
        {
            string path = this.BuildPathFromFQDN(fqdn);
            return File.ReadAllText(path, Encoding.UTF8);
        }

        private string BuildPathFromFQDN(string fqdn)
        {
            string path = Path.Combine(Application.streamingAssetsPath, "src");
            string[] split = fqdn.Split('.');

            for (int i = 0; i < split.Length - 1; i++)
            {
                path = Path.Combine(path, split[i]);
            }

            int subClassIndex = split.Last<string>().IndexOf("$");
            if (subClassIndex != -1)
            {
                path = Path.Combine(path, split.Last().Substring(0, subClassIndex));
            }
            else
            {
                path = Path.Combine(path, split.Last());
            }

            return path + ".java";
        }
    }
}
