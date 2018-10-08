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

        // 1. Write effective algorithm to find text is normal or not
        // (based on dictionary of common words and n-grams)

        // 2. Maybe needed to refactor code

        // 3. Write a code to attack some simple substitution cipher. 
        //To reduce the complexity of this one we will use only uppercase letters, so the keyspace is only 26! 
        //To get this one right automatically you will probably need to use some sort of genetic algorithm (which worked the best last year),
        //simulated annealing or gradient descent. Seriously, write it right now, you will need it to decipher the next one as well. 
        //Bear in mind, therea??s no spaces.

        // 4. Decrypt strings 4, 5, 6 by some way

        // 5. Write genetic algorithm.
        // For this we need:
        //   a) the dictionary of n-grams (sequence of letters with length n) with frequency of coincidence for each n-gram
        //   b) write function that will define fitness for individual algo
        //   c) write function that will crossover 2 individuals 
        //      (each individual defined by string of unique letters so we must reposition letters in some way)
        //   d) and maybe something else :)))
        #endregion

        static void Main(string[] args)
        {

            #region Task1
            ////string task1 = @"]|d3gaj3r3avcvrgz}t>xvj3K\A3pzc{va=3V=t=3zg3`{|f.w3grxv3r3`gaz}t31{v..|3d|a.w13r" +
            ////    "}w?3tzev}3g{v3xvj3z`31xvj1?3k|a3g{v3uza`g3.vggva31{13dzg{31x1?3g{v}3k|a31v13dzg{31v1?3g{v}3" +
            ////    "1.13dzg{31j1?3r}w3g{v}3k|a3}vkg3p{ra31.13dzg{31x13rtrz}?3g{v}31|13dzg{31v13r}w3`|3|}=3J|f3~" +
            ////    "rj3f`v3z}wvk3|u3p|z}pzwv}pv?3[r~~z}t3wz`gr}pv?3Xr`z`xz3vkr~z}rgz|}?3`grgz`gzpr.3gv`g`3|a3d{" +
            ////    "rgveva3~vg{|w3j|f3uvv.3d|f.w3`{|d3g{v3qv`g3av`f.g=";

            ////WriteToFile("task1.txt", task1);

            //CeasarDecrypt ceasarDecrypt = new CeasarDecrypt(ReadFromFile("task1.txt"));
            //string result1 = ceasarDecrypt.Decrypt();
            //WriteToFile("result1.txt", result1);
            #endregion

            #region Task2

            ////string task2 = "1c41023f564b2a130824570e6b47046b521f3f5208201318245e0e6b40022643072e13183e51183f" +
            ////    "5a1f3e4702245d4b285a1b23561965133f2413192e571e28564b3f5b0e6b50042643072e4b023f4a4b24554b3f5" +
            ////    "b0238130425564b3c564b3c5a0727131e38564b245d0732131e3b430e39500a38564b27561f3f5619381f4b385c" +
            ////    "4b3f5b0e6b580e32401b2a500e6b5a186b5c05274a4b79054a6b67046b540e3f131f235a186b5c052e131922540" +
            ////    "33f130a3e470426521f22500a275f126b4a043e131c225f076b431924510a295f126b5d0e2e574b3f5c4b3e400e" +
            ////    "6b400426564b385c193f13042d130c2e5d0e3f5a086b52072c5c192247032613433c5b02285b4b3c5c1920560f6" +
            ////    "b47032e13092e401f6b5f0a38474b32560a391a476b40022646072a470e2f130a255d0e2a5f0225544b24414b2c" +
            ////    "410a2f5a0e25474b2f56182856053f1d4b185619225c1e385f1267131c395a1f2e13023f13192254033f1305244" +
            ////    "4476b4a043e131c225f076b5d0e2e574b22474b3f5c4b2f56082243032e414b3f5b0e6b5d0e33474b245d0e6b52" +
            ////    "186b440e275f456b710e2a414b225d4b265a052f1f4b3f5b0e395689cbaa186b5d046b401b2a500e381d61";

            ////WriteToFile("task2.txt", task2);

            //VigenereDecrypt vigenereDecrypt = new VigenereDecrypt(DecodeHexToUTF8(ReadFromFile("task2.txt")), null);
            //string result2 = vigenereDecrypt.Decrypt();
            //WriteToFile("result2.txt", result2);
            #endregion
            Thread thread = new Thread(Gh);
            thread.Start();
            Thread.Sleep(500);
            Thread thread1 = new Thread(Gh);
            thread1.Start();
            //DictionaryNGrams dict = DictionaryNGrams.GetDictionaryNGrams();
            string encryptedText = File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text3.txt").ToLower();
            CipherFitness.Show(encryptedText);
            //Console.WriteLine(CipherFitness.Fitness(encryptedText));
            encryptedText = File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text2.txt").ToLower();
            CipherFitness.Show(encryptedText);
            //Console.WriteLine(CipherFitness.Fitness(encryptedText));
            encryptedText = File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text1.txt").ToLower();
            CipherFitness.Show(encryptedText);
            //Console.WriteLine(CipherFitness.Fitness(encryptedText));
            encryptedText = File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text.txt").ToLower();
            CipherFitness.Show(encryptedText);
            //Console.WriteLine(CipherFitness.Fitness(encryptedText));

            GeneticModel gm = new GeneticModel(encryptedText);
            string fd = gm.Run();
            CipherFitness.Show(fd);
            Console.WriteLine("\n==================\n"+fd);
            thread.Join();
            thread1.Join();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void Gh()
        {
            string encryptedText = File.ReadAllText("../../../Decryption/SubstitutionDecript/training_text.txt").ToLower();
            CipherFitness.Show(encryptedText);
            Thread.Sleep(500);
            GeneticModel gm = new GeneticModel(encryptedText);
            string fd = gm.Run();
            CipherFitness.Show(fd);
            Console.WriteLine("\n==================\n" + fd);
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
