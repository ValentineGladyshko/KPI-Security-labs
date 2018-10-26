using System;

namespace Decryption
{
    public class VigenereDecrypt
    {
        public string source;
        public int keyLength;
        public int[] key;

        public VigenereDecrypt(string source, int? keyLength)
        {
            this.source = source;
            this.keyLength = keyLength != null ? Convert.ToInt32(keyLength) : FindKeyLength();
            key = new int[this.keyLength];
        }

        private int FindKeyLength()
        {
            int maxIndex = -1;
            double indexOfConcidence = -1;
            double curIndex;

            for (int i = 1; i < 50; i++)
            {
                LetterCounter letterCounter = new LetterCounter(source, i);
                curIndex = letterCounter.IndexOfConcidence();
                if (curIndex > indexOfConcidence)
                {
                    if(curIndex/ indexOfConcidence > 1.4)
                    {
                        maxIndex = i;
                        return maxIndex;
                    }
                    indexOfConcidence = letterCounter.IndexOfConcidence();
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

        public string Decrypt()
        {
            key = new int[keyLength];
            string result = string.Empty;

            for (int i = 0; i < keyLength; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    result = string.Empty;

                    for (int k = 0; k < source.Length - keyLength + 1; k += keyLength)
                    {
                        result += Convert.ToChar(source[k + i] ^ j);
                    }

                    if (Evaluate(result) > 0)
                    {                      
                        key[i] = j;
                        break;
                    }
                }
            }

            result = string.Empty;

            result += "key: ";
            for (int i = 0; i < keyLength; i++)
            {
                result += Convert.ToChar(key[i]);
            }

            result += "\nresult string:\n";
            for (int i = 0; i < source.Length - keyLength + 1; i += keyLength)
            {
                for (int j = 0; j < keyLength; j++)
                {
                    result += Convert.ToChar(source[i + j] ^ key[j]);
                }
            }

            return result;
        }

        public static double Evaluate(string text)
        {
            double score = 0.0;

            foreach (char c in text)
            {
                if (!(char.IsLower(c) || char.IsDigit(c) || char.IsWhiteSpace(c)))
                {
                    score -= 2;
                }

                if (char.IsLower(c))
                {
                    score += 0.2;
                }

                if (char.IsWhiteSpace(c))
                {
                    score += 0.1;
                }
            }

            return score;
        }
    }
}
