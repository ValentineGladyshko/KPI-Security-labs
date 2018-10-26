using System;
using Decryption.SubstitutionDecript;

namespace Decryption
{
    internal sealed class CeasarDecrypt
    {
        public string source;

        public CeasarDecrypt(string source)
        {
            this.source = source;
        }

        //key 19

        public string Decrypt()
        {
            string result = string.Empty;

            for (int j = 0; j < 256; j++)
            {
                string text = string.Empty;

                foreach (char c in source)
                {
                    text += (char)(c ^ j);
                }

                if (CipherFitness.NewEvaluate(text) > 70)
                {
                    result += "key: " + (char)j + "\nresult string:\n" + text;
                    break;
                }        
            }
            return result;
        }
    }
}
