using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption.SubstitutionDecript
{
    public class SubstitutionDecrypt4
    {
        private readonly char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        public List<string> key;
        public List<Dictionary<char, char>> dictionary;

        public SubstitutionDecrypt4(List<string> key)
        {
            dictionary = new List<Dictionary<char, char>>();
            this.key = key;
            for (int i = 0; i < key.Count; i++)
            {
                dictionary.Add(new Dictionary<char, char>());
                for (int j = 0; j < alphabet.Length; j++)
                {
                    dictionary[i].Add(key[i][j], alphabet[j]);
                }
            }
        }

        public string DecryptText(string source)
        {
            string decryptedText = "";
            int i = 0;
            foreach (char c in source)
            {
                if (Char.IsLetter(c))
                {
                    decryptedText += dictionary[i][c];
                }
                else
                {
                    decryptedText += c;
                }
                i++;
                if(i == key.Count)
                {
                    i = 0;
                }
            }
            return decryptedText;
        }
    }
}
