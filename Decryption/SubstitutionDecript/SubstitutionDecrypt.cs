using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption.SubstitutionDecript
{
    public class SubstitutionDecrypt
    {
        public String source;

        public SubstitutionDecrypt(String source)
        {
            this.source = source;
        }

        public string DecryptText(String key)
        {
            string decryptedText = "";
            Dictionary<char, int> indexInKey = new Dictionary<char, int>();
            for (int i = 0; i < key.Length; i++)
            {
                indexInKey[key[i]] = i;
            }
            foreach (char c in source)
            {
                if (Char.IsLetter(c))
                {
                    decryptedText += (char)('A' + indexInKey[c]);
                }
                else
                {
                    decryptedText += c;
                }
            }
            return decryptedText;
        }
    }
}
