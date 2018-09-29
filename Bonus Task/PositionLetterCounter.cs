using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonus_Task
{
    internal sealed class PositionLetterCounter
    {
        public List<char>[] list;
        public string[] text;

        public PositionLetterCounter(string[] cipherText)
        {
            text = cipherText;
            int maxLen = 0;

            for (int i = 0; i < cipherText.Length; i++)
            {
                if (maxLen < cipherText[i].Length)
                {
                    maxLen = cipherText[i].Length;
                }
            }

            list = new List<char>[maxLen];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = new List<char>();
            }

            foreach (string str in cipherText)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    foreach (char c in list[i])
                    {
                        if (c == str[i])
                            break;
                    }

                    list[i].Add(str[i]);
                }
            }
        }

        public void Show()
        {
            for (int i = 0; i < list.Length; i++)
            {
                foreach(char c in list[i])
                {
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }

        public bool IsLetter(char c)
        {
            if((c > 96) && (c < 123))
            {
                return true;
            }
            if ((c > 64) && (c < 91))
            {
                return true;
            }
            if (c == ' ')
            {
                return true;
            }
            if (c == '‘')
            {
                return true;
            }
            if (c == ';')
            {
                return true;
            }
            if (c == ':')
            {
                return true;
            }
            return false;
        }
        public void Decode()
        {
            int[] key = new int[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                char[] result = list[i].ToArray();
                for (int j = 0; j < 256; j++)
                {
                    string res = string.Empty;
                    foreach (char c in result)
                    {
                        res += (char)(c ^ j);
                    }
                    int count = 0;

                    foreach (char c in res)
                    {
                        if (IsLetter(c))
                            count++;
                    }
                    if (count == res.Length)
                    {
                        if (key[i] == 0)
                        {
                            key[i] = j;
                        }
                        else
                            key[i] = -1;
                    }
                }
            }

            for (int i = 0; i < key.Length; i++)
            {
                
                    for (int j = 0; j < text.Length; j++)
                    {
                        if (i < text[j].Length)
                        {
                            char[] buffer = text[j].ToArray();
                            if (key[i] != 0 && key[i] != -1)
                            {
                                buffer[i] = (char)(buffer[i] ^ key[i]);
                            }
                            else
                            {
                                buffer[i] = '_';
                            }
                            text[j] = new string(buffer);
                        }
                    }

            }
            Console.WriteLine();
            for (int i = 0; i < text.Length; i++)
            {
                Console.WriteLine(text[i]);
            }
        }
    }
}
