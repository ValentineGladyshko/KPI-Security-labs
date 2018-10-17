using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using Decryption.SubstitutionDecript;

namespace Decryption
{
    class Program
    {
        #region To Do List
        // To get task files uncomment intialization of task strings, WriteToFile("task1.txt", task1); and WriteToFile("task2.txt", task2);
        // To DO List:

        // 1. Maybe needed to refactor code

        // 2. Decrypt strings 5, 6 by some way

        #endregion

        static void Main(string[] args)
        {

            #region Task1

            //CeasarDecrypt ceasarDecrypt = new CeasarDecrypt(ReadFromFile("task1.txt"));
            //string result1 = ceasarDecrypt.Decrypt();
            //Console.WriteLine("Ceasar Decrypt:\n" + result1 + "\n");
            //WriteToFile("result1.txt", result1);

            #endregion

            #region Task2

            //VigenereDecrypt vigenereDecrypt = new VigenereDecrypt(DecodeHexToUTF8(ReadFromFile("task2.txt")), null);
            //string result2 = vigenereDecrypt.Decrypt();
            //Console.WriteLine("Vigenere Decrypt:\n" + result2 + "\n");
            //WriteToFile("result2.txt", result2);

            #endregion

            #region Task3

            //DictionaryNGrams.GetDictionaryNGrams();
            //Console.WriteLine("Press any key to start substitution decrypt");
            //Console.ReadKey();
            CipherFitness4.NewEvaluateShow(File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text.txt").ToLower());
            CipherFitness4.NewEvaluateShow(File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text1.txt").ToLower());
            CipherFitness4.NewEvaluateShow(File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text2.txt").ToLower());
            CipherFitness4.NewEvaluateShow(File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text3.txt").ToLower());
            CipherFitness4.NewEvaluateShow(File.ReadAllText("../../../Decryption/SubstitutionDecript/bigtext.txt").ToLower());

            Thread thread1 = new Thread(Gh);
            thread1.Start();
            //Thread.Sleep(100);
            //Thread thread2 = new Thread(Gh);
            //thread2.Start();
            //Thread.Sleep(100);
            //Thread thread3 = new Thread(Gh);
            //thread3.Start();
            //Thread.Sleep(500);

            thread1.Join();
            //thread2.Join();
            //thread3.Join();

            #endregion
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void Gh()
        {
            string encryptedText = File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text.txt").ToLower();
            Thread.Sleep(100);
            GeneticModel gm = new GeneticModel(encryptedText);
            string result3 = gm.Run();
            Console.WriteLine("\n======================\n\nSubstitution Decrypt:\n" + result3 + "\n\n======================\n");
        }

        public static string ReadFromFile(string filepath)
        {
            string result = string.Empty;

            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public static void WriteToFile(string filepath, string result)
        {
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.Write(result);
            }
        }


        public static string DecodeHexToUTF8(string task)
        {
            string result = string.Empty;
            for (int i = 0; i < task.Length; i += 2)
            {
                result += (char)int.Parse(task.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return result;
        }
    }

}
