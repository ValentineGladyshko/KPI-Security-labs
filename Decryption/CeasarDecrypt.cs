using System;

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

        public void Decrypt()
        {
            string result = string.Empty;

            for (int j = 0; j < 256; j++)
            {
                result = string.Empty;

                foreach (char c in source)
                {
                    result += (char)(c ^ j);
                }

                int letterCount = 0;
                int spaceCount = 0;

                foreach (char c in result)
                {
                    if (Char.IsLetter(c) || c == '=')
                        letterCount++;
                    if (Char.IsWhiteSpace(c))
                        spaceCount++;
                }

                if ((((double)letterCount / result.Length) > 0.7) 
                    && (((double)spaceCount / result.Length) > 0.12) 
                    && (((double)(letterCount + spaceCount) / result.Length) > 0.9))
                {
                    Console.WriteLine("key: " + (char)j + "\nresult string:\n" + result);
                }
            }
        }
    }
}
