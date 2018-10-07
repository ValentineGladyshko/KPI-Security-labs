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
        public static Dictionary<string, double> trigrams;
        public static Dictionary<string, double> bigrams;
        public static IEnumerable<char> startDic;
        public static bool recalculated = false;

        public SimpleSubstitutionDecrypt(string source)
        {
            this.source = source;
        }

        public string Decrypt()
        {
            trigrams = GetTrigrams();
            bigrams = GetBigrams();
            startDic = GetAmount(source).OrderBy(x => x.Value).Select(x => x.Key);

            var result = TransformByHillСlimbing(source);
            Console.WriteLine(result);
            Console.ReadKey();
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

        public static string TransformByHillСlimbing(string text)
        {
            double coef = 0;
            var key = GenerateRandomKey();
            var tempKey = key;
            var bestKey = tempKey;
            double bestKeyCoef = 0;
            int keyCounter = 0;
            int exiter = 0;
            int exiter2 = 0;
            string temp = null;
            int stage = 1;

            while (exiter < 25)
            {
                temp = TransformByFrequency(text, tempKey);

                Console.SetCursorPosition(0, 0);
                Console.WriteLine(temp);
                Console.WriteLine("Coef    : " + coef + "             ");
                Console.WriteLine("Exiter  : " + exiter + "           ");
                if (stage == 1)
                {
                    Console.WriteLine("Stage   : " + stage + "           ");
                    Console.WriteLine("Exiter2 : " + exiter2 + "           ");
                }
                else if (!recalculated)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Bigram stage Done");
                    Console.ResetColor();
                }

                if (exiter2 >= 700)
                {
                    stage = 2;
                    exiter = 0;
                    coef = GetCoefficient(temp, stage);
                    exiter2 = 0;
                }
                var tempCoef = GetCoefficient(temp, stage);

                if (coef < tempCoef)
                {
                    if (bestKeyCoef < tempCoef)
                    {
                        bestKey = tempKey;
                        bestKeyCoef = tempCoef;
                    }
                    if (keyCounter >= 100)
                    {
                        key = tempKey;
                        coef = tempCoef;
                        exiter = 0;
                        exiter2 = 0;
                        //Console.WriteLine(coef);
                        keyCounter = 0;
                    }
                }
                else if (coef == tempCoef)
                {
                    if (stage == 2)
                        exiter++;
                    else
                        exiter2++;
                }

                tempKey = ChangeKey(key);
                keyCounter++;
            }

            return temp;
        }

        public static double GetCoefficient(string text, int stage)
        {
            text = text.ToUpper();
            //переробити на правильне
            double res = 0;

            var trigs = trigrams/*GetTrigrams()*/;

            if (stage == 1)
                trigs = bigrams;
            else
                trigs = trigrams;

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

        public static Dictionary<string, double> GetBigrams()
        {
            string t = null;
            using (FileStream fstream = File.OpenRead(@"../../../Decryption/SubstitutionDecript/english_bigrams.txt"))
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

            foreach (var item in quadgrams)
            {
                var i = item.Split(' ');

                result.Add(i[0], Convert.ToDouble(i[1]));
            }

            return result;
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
            }

            return result;
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

        public static string TransformByFrequency(string text, IEnumerable<char> key)
        {
            var abcByFreaquency = key;/* abc.OrderBy(x => x.Key).Select(x => x.Value);*/
            var textABCFreaquency = startDic;

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
