using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption
{
    public class SimpleSubstitutionDecrypt
    {
        public string source;
        public static Dictionary<string, double> initedQuads;

        public SimpleSubstitutionDecrypt(string source)
        {
            this.source = source;
        }

        public string Decrypt()
        {
            initedQuads = GetTrigrams();

            var result = TransformByHillСlimbing(source);


            return result;
        }

        public static Dictionary<char, int> GetAmount(string text)
        {
            Dictionary<char, int> result = new Dictionary<char, int>();

            foreach (var ch in text)
            {
                if (result.ContainsKey(ch))
                {
                    result[ch] = result[ch] + 1;
                }
                else
                {
                    result.Add(ch, 1);
                }
            }

            return result;
        }

        public static Dictionary<int, char> GetDefaultABC()
        {
            Dictionary<int, char> abc = new Dictionary<int, char>();
            abc.Add(1, 'e');
            abc.Add(2, 't');
            abc.Add(3, 'a');
            abc.Add(4, 'i');

            return abc;
        }

        public static string TransformByHillСlimbing(string text)
        {
            double coef = 0;
            var key = GenerateRandomKey();
            var tempKey = key;
            var bestKey = tempKey;
            double bestKeyCoef = 0;
            int keyCounter = 0;
            int exiter = 0;
            string temp = null;

            while (exiter < 75/*coef < 4450*/)
            {
                temp = TransformByFrequency(text, tempKey);

                var tempCoef = GetCoefficient(temp);

                if (coef < tempCoef)
                {
                    if (bestKeyCoef < tempCoef)
                    {
                        bestKey = tempKey;
                        bestKeyCoef = tempCoef;
                    }

                    if (keyCounter >= 26)
                    {
                        key = tempKey;
                        coef = tempCoef;
                        exiter = 0;
                        Console.WriteLine(coef);
                        keyCounter = 0;
                    }
                }
                else if (coef == tempCoef)
                {
                    exiter++;

                    if (exiter == 50)
                    {
                        Console.WriteLine(exiter);
                    }

                    if (exiter == 100)
                    {
                        Console.WriteLine(exiter);
                    }

                    if (exiter == 250)
                    {
                        Console.WriteLine(exiter);
                    }
                }

                tempKey = ChangeKey(key);
                keyCounter++;
            }

            return temp;
        }

        public static double GetCoefficient(string text)
        {
            text = text.ToUpper();
            //переробити на правильне
            double res = 0;
            var trigs = initedQuads/*GetTrigrams()*/;

            foreach (var tg in trigs)
            {
                int counter = text.Split(new string[] { tg.Key }, StringSplitOptions.None).Count() - 1;
                res += Math.Log10(tg.Value) * counter;
            }

            return res;
        }

        private static IEnumerable<char> ChangeKey(IEnumerable<char> input)
        {
            var list = input.ToList();
            Random random = new Random();
            int first = random.Next(list.Count);
            int second = random.Next(list.Count);

            var temp = list[first];
            list[first] = list[second];
            list[second] = temp;

            return list;
        }

        public static Dictionary<string, double> GetTrigrams()
        {
            string t = null;
            using (FileStream fstream = File.OpenRead(@"../../../Decryption/SubstitutionDecript/english_quadgrams.txt"))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                t = System.Text.Encoding.Default.GetString(array);
            }

            var quadgrams = t.Split('\n');

            var result = new Dictionary<string, double>();

            //string tempT = null;
            //double tempDouble = 0;

            foreach (var item in quadgrams)
            {
                var i = item.Split(' ');

                result.Add(i[0], Convert.ToDouble(i[1]));
                //if (item.Count() == 4)
                //{
                //    result.Add(item, 13168375);
                //}
                //else
                //{
                //    tempT = item.Skip(item.Length - 4).ToString();
                //    result.Add(tempT, tempDouble);
                //    tempDouble = Convert.ToDouble(item.Remove(item.Length - 4));
                //}
            }

            //result.Add("TION", 13168375);
            //result.Add("NTHE", 11234972);
            //result.Add("THER", 10218035);
            //result.Add("THAT", 8980536);
            //result.Add("OFTH", 8132597);

            return result;

            //return new List<string>()
            //{
            //    //"the", "and", "ing", "her", "tha", "ere",
            //    //"hat", "eth", "ent", "nth", "for", "his",
            //    //"thi", "ter", "int", "dth", "you", "all",
            //    //"hes", "ion", "ith", "oth", "est", "tth",
            //    //"oft", "ver", "sth", "ers", "fth", "rea"
            //    "add", "the", "ability", "abi", "ili", "ity", "lity", "to", "decipher", "dec", "deci",
            //    "cip", "ciph", "eci", "ecip", "decip", "deciph", "pher", "ecipher", "any", "kind", "ind", "nd", "kin",
            //    "of", "poly", "tic", "etic", "betic", "alp", "alph", "phab",
            //    "polyalphabetic", "substitution", "ciphers", "one", "used", "in",
            //    "cipher", "texts", "here", "has", "independent", "randomly", "chosen",
            //    "monoalphabetic", "substitution", "patterns",
            //    "foreach", "letter", "from", "english", "alphabet", "it", "is", "clear", "that", "you", "can", "no",
            //    "more"
            //};
        }

        public static IEnumerable<char> GenerateRandomKey()
        {
            var abc = "abcdefghijklmnopqrstyvwxuz";
            var list = abc.ToList();
            Random random = new Random();

            var data = new List<char>();

            for (int i = 0; i < list.Count; i++)
            {
                int j = random.Next(data.Count + 1);
                if (j == data.Count)
                {
                    data.Add(list[i]);
                }
                else
                {
                    data.Add(data[j]);
                    data[j] = list[i];
                }
            }

            return data;
        }

        public static string TransformByFrequency(string text)
        {
            var abc = GetDefaultABC();

            var startDic = GetAmount(text);

            var abcByFreaquency = abc.OrderBy(x => x.Key).Select(x => x.Value);
            var textABCFreaquency = startDic.OrderBy(x => x.Value).Select(x => x.Key);

            //var testForBP = startDic.OrderByDescending(x => x.Value);

            Stack<char> stack = new Stack<char>();

            foreach (var i in textABCFreaquency)
            {
                stack.Push(i);
            }

            foreach (var l in abcByFreaquency)
            {
                text = text.Replace(stack.Pop(), l);
            }

            return text;
        }

        public static string TransformByFrequency(string text, IEnumerable<char> key)
        {
            var abc = GetDefaultABC();

            var startDic = GetAmount(text);

            var abcByFreaquency = key;/* abc.OrderBy(x => x.Key).Select(x => x.Value);*/
            var textABCFreaquency = startDic.OrderBy(x => x.Value).Select(x => x.Key);

            //var testForBP = startDic.OrderByDescending(x => x.Value);

            Stack<char> stack = new Stack<char>();

            foreach (var i in abcByFreaquency)
            {
                stack.Push(i);
            }

            foreach (var l in textABCFreaquency)
            {
                text = text.Replace(l, stack.Pop());
            }

            return text;
        }

    }
}
