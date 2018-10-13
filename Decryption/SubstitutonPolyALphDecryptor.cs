using Decryption.SubstitutionDecript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryption
{
    public class SubstitutonPolyALphDecryptor
    {
        public string source;
        public static Dictionary<string, double> trigrams;
        public static Dictionary<string, double> bigrams;
        public static IEnumerable<char> startDic;
        public static bool recalculated = false;
        static int stage = 1;
        static int exiter = 0;
        static int exiter2 = 0;
        static bool speedy2 = true;
        static bool speedy = true;
        public int keyCount;

        public SubstitutonPolyALphDecryptor(string source, int keyCount)
        {
            this.source = source;
            this.keyCount = keyCount;
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
            var key1 = GenerateRandomKey();
            var key2 = GenerateRandomKey();
            var key3 = GenerateRandomKey();
            var key4 = GenerateRandomKey();
            var tempKey1 = key1;
            var tempKey2 = key2;
            var tempKey3 = key3;
            var tempKey4 = key4;
            var bestKey1 = tempKey1;
            var bestKey2 = tempKey2;
            var bestKey3 = tempKey3;
            var bestKey4 = tempKey4;
            double bestKeyCoef = 0;
            int keyCounter = 0;

            int exiter2 = 0;
            string temp = null;

            while (exiter < 100)
            {
                temp = TransformByFrequency(text, tempKey1, tempKey2, tempKey3, tempKey4);

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
                CipherFitness fit = new CipherFitness();
                
                if (exiter2 >= 50)
                {
                    stage = 2;
                    exiter = 0;
                    coef = GetCoefficient(temp, stage);
                    //coef = fit.dictionaryStatisticFitness(temp);
                    exiter2 = 0;
                }

                //var tempCoef = GetCoefficient(temp, stage); 
                double tempCoef;
                tempCoef = GetCoefficient(temp, 2);

                if (coef < tempCoef)
                {
                    if (bestKeyCoef < tempCoef)
                    {
                        bestKey1 = tempKey1;
                        bestKey2 = tempKey2;
                        bestKey3 = tempKey3;
                        bestKey4 = tempKey4;
                        bestKeyCoef = tempCoef;
                    }
                    if (keyCounter >= 20)
                    {
                        key1 = tempKey1;
                        key2 = tempKey2;
                        key3 = tempKey3;
                        key4 = tempKey4;
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

                var evolution = new Random().Next(1, 40);
                //if (evolution < 5) tempKey1 = ChangeKey(key1);
                //if(evolution>2 && evolution <6) tempKey2 = ChangeKey(key2);
                //if (evolution > 3 && evolution < 7) tempKey3 = ChangeKey(key3);
                //if (evolution > 6) tempKey4 = ChangeKey(key4);

                if(evolution<17) tempKey1 = ChangeKey(key1);
                if (evolution < 25) tempKey2 = ChangeKey(key2);
                if (evolution < 20) tempKey3 = ChangeKey(key3);
                if (evolution < 19) tempKey4 = ChangeKey(key4);
                keyCounter++;
            }

            return temp;
        }

        public static double GetCoefficient(string text, int stage)
        {
            text = text.ToUpper();
            //переробити на правильне
            double res = 0;

            var bigs = bigrams;
            var trigs = trigrams;

            foreach (var tg in bigs)
            {
                int counter = text.Split(new string[] { tg.Key }, StringSplitOptions.None).Count() - 1;
                res += Math.Log10(tg.Value) * counter;
            }

            foreach (var tg in trigs)
            {
                int counter = text.Split(new string[] { tg.Key }, StringSplitOptions.None).Count() - 1;
                res += Math.Log10(tg.Value) * counter;
            }

            res += new CipherFitness().dictionaryStatisticFitness(text);

            return res;
        }

        private static IEnumerable<char> ChangeKey(IEnumerable<char> input)
        {
            var list = input.ToList();
            Random random = new Random();
            if (exiter2 > 30) speedy2 = false;
            if (exiter > 100) speedy = false;
            if (stage == 1 && speedy2)
            {
                int countSwap;
                countSwap = random.Next(1, 6);
                //else countSwap = random.Next(1, 2);

                int first;
                int second;

                List<int> listOfSwaps = new List<int>();
                for (int i = 0; i < countSwap; i++)
                {
                    do
                    {
                        first = random.Next(list.Count);
                        second = random.Next(list.Count);
                    } while (listOfSwaps.Contains(first) || listOfSwaps.Contains(second));
                    var temp = list[first];
                    list[first] = list[second];
                    list[second] = temp;
                }
            }
            else if (speedy)
            {
                int countSwap;
                countSwap = random.Next(1, 4);

                int first;
                int second;
                List<int> listOfSwaps = new List<int>();

                for (int i = 0; i < countSwap; i++)
                {
                    do
                    {
                        first = random.Next(list.Count);
                        second = random.Next(list.Count);
                    } while (listOfSwaps.Contains(first) || listOfSwaps.Contains(second));
                    var temp = list[first];
                    list[first] = list[second];
                    list[second] = temp;
                }
            }
            else
            {
                int countSwap;
                countSwap = random.Next(1, 8);
                //else countSwap = random.Next(1, 2);

                int first;
                int second;

                List<int> listOfSwaps = new List<int>();
                for (int i = 0; i < countSwap; i++)
                {
                    do
                    {
                        first = random.Next(list.Count);
                        second = random.Next(list.Count);
                    } while (listOfSwaps.Contains(first) || listOfSwaps.Contains(second));
                    var temp = list[first];
                    list[first] = list[second];
                    list[second] = temp;
                }
            }

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

        public static string TransformByFrequency(string text, IEnumerable<char> key1, IEnumerable<char> key2, IEnumerable<char> key3, IEnumerable<char> key4)
        {
            var abcByFreaquency1 = key1;
            var abcByFreaquency2 = key2;
            var abcByFreaquency3 = key3;
            var abcByFreaquency4 = key4;
            var textABCFreaquency = startDic;
            char[] textCh = new char[1000000];
            textCh = text.ToArray();
            Stack<char> stack1 = new Stack<char>();
            Stack<char> stack2 = new Stack<char>();
            Stack<char> stack3 = new Stack<char>();
            Stack<char> stack4 = new Stack<char>();

            foreach (var i in abcByFreaquency1)
            {
                stack1.Push(i);
            }
            foreach (var i in abcByFreaquency2)
            {
                stack2.Push(i);
            }
            foreach (var i in abcByFreaquency3)
            {
                stack3.Push(i);
            }
            foreach (var i in abcByFreaquency4)
            {
                stack4.Push(i);
            }

            foreach (var l in textABCFreaquency)
            {
                //text = text.Replace(l, stack.Pop());
                var res1 = stack1.Pop();
                var res2 = stack2.Pop();
                var res3 = stack3.Pop();
                var res4 = stack4.Pop();

                var rest = textCh.Length % 4;
                for (int i = 3; i < textCh.Length; i+=4)
                {
                    if (textCh[i] == l) textCh[i] = res4;
                    if (textCh[i-1] == l) textCh[i-1] = res3;
                    if (textCh[i-2] == l) textCh[i-2] = res2;
                    if (textCh[i-3] == l) textCh[i-3] = res1;
                }
                if(rest==1 && textCh[textCh.Length - 1] == l) textCh[textCh.Length-1] = res1;
                else if (rest == 2)
                {
                    if (textCh[textCh.Length - 2] == l) textCh[textCh.Length - 2] = res1;
                    if (textCh[textCh.Length - 1] == l) textCh[textCh.Length - 1] = res2;
                }
                else if (rest == 3)
                {
                    if (textCh[textCh.Length - 3] == l) textCh[textCh.Length - 3] = res1;
                    if (textCh[textCh.Length - 2] == l) textCh[textCh.Length - 2] = res2;
                    if (textCh[textCh.Length - 1] == l) textCh[textCh.Length - 1] = res3;
                }
            }

            return new string(textCh);
        }
    }
}
