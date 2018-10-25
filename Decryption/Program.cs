using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Decryption.SubstitutionDecript;
using Decryption.PolySubstitutionDecrypt;

namespace Decryption
{
    class Program
    {
        #region To Do List
        
        // To DO List:

        // 1. Maybe needed to refactor code

        // 2. Decrypt string 6 by some way

        #endregion

        static void Main(string[] args)
        {

            #region Task2

            CeasarDecrypt ceasarDecrypt = new CeasarDecrypt
                (File.ReadAllText("../../../Decryption/SubstitutionDecript/tasks/task2.txt"));
            string result2 = ceasarDecrypt.Decrypt();
            Console.WriteLine("Ceasar Decrypt:\n" + result2 + "\n");

<<<<<<< HEAD
            CeasarDecrypt ceasarDecrypt = new CeasarDecrypt(ReadFromFile("task1.txt"));
            string result1 = ceasarDecrypt.Decrypt();
            WriteToFile("result1.txt", result1);
=======
>>>>>>> GeneticAlgoTesting
            #endregion

            #region Task3

<<<<<<< HEAD
            //string task2 = "1c41023f564b2a130824570e6b47046b521f3f5208201318245e0e6b40022643072e13183e51183f" +
            //    "5a1f3e4702245d4b285a1b23561965133f2413192e571e28564b3f5b0e6b50042643072e4b023f4a4b24554b3f5" +
            //    "b0238130425564b3c564b3c5a0727131e38564b245d0732131e3b430e39500a38564b27561f3f5619381f4b385c" +
            //    "4b3f5b0e6b580e32401b2a500e6b5a186b5c05274a4b79054a6b67046b540e3f131f235a186b5c052e131922540" +
            //    "33f130a3e470426521f22500a275f126b4a043e131c225f076b431924510a295f126b5d0e2e574b3f5c4b3e400e" +
            //    "6b400426564b385c193f13042d130c2e5d0e3f5a086b52072c5c192247032613433c5b02285b4b3c5c1920560f6" +
            //    "b47032e13092e401f6b5f0a38474b32560a391a476b40022646072a470e2f130a255d0e2a5f0225544b24414b2c" +
            //    "410a2f5a0e25474b2f56182856053f1d4b185619225c1e385f1267131c395a1f2e13023f13192254033f1305244" +
            //    "4476b4a043e131c225f076b5d0e2e574b22474b3f5c4b2f56082243032e414b3f5b0e6b5d0e33474b245d0e6b52" +
            //    "186b440e275f456b710e2a414b225d4b265a052f1f4b3f5b0e395689cbaa186b5d046b401b2a500e381d61";

            //WriteToFile("task2.txt", task2);

           

            VigenereDecrypt vigenereDecrypt = new VigenereDecrypt(DecodeHexToUTF8(ReadFromFile("task2.txt")), null);
            string result2 = vigenereDecrypt.Decrypt();
            WriteToFile("result2.txt", result2);
            #endregion
            

            string task3 = "EFFPQLEKVTVPCPYFLMVHQLUEWCNVWFYGHYTCETHQEKLPVMSAKSPVPAPVYWMVHQLUSPQLYWLASLFVWPQLMVHQLUPLRPSQLULQESPBLWPCSVRVWFLHLWFLWPUEWFYOTCMQYSLWOYWYETHQEKLPVMSAKSPVPAPVYWHEPPLUWSGYULEMQTLPPLUGUYOLWDTVSQETHQEKLPVPVSMTLEUPQEPCYAMEWWYOYULULTCYWPQLSEOLSVOHTLUYAPVWLYGDALSSVWDPQLNLCKCLRQEASPVILSLEUMQBQVMQCYAHUYKEKTCASLFPYFLMVHQLUPQVSHEUEDUEHQBVTTPQLVWFLRYGMYVWMVFLWMLSPVTTBYUNESESADDLSPVYWCYAMEWPUCPYFVIVFLPQLOLSSEDLVWHEUPSKCPQLWAOKLUYGMQEUEMPLUSVWENLCEWFEHHTCGULXALWMCEWETCSVSPYLEMQYGPQLOMEWCYAGVWFEBECPYASLQVDQLUYUFLUGULXALWMCSPEPVSPVMSBVPQPQVSPCHLYGMVHQLUPQLWLRPHEUEDUEHQMYWPEVWSSYOLHULPPCVWPLULSPVWDVWGYUOEPVYWEKYAPSYOLEFFVPVYWETULBEUF";
            string task4 = "MULDCLZWKLZTWTXHJCXEDQLHLXIHBLLJDQWHDQUZSOBCKMEKWXRABUGADQZOBLQNNXMJSYXDWYZFNKINEUICNJNHSUTTNQZTQNGFCYYJCNEANNXHNKSADQZVDSEDCLRSWRZSAQMFMXOJSYCFSYETSQZSBOSNMSIVNCBCWRXVBGIBAGKFMLZTWTBBFUOJESIIBGEPBRNAKMYTYOSCWTZSBYSPCCZFCMBBXTIUBSXTNMENRNKADQRFCYQZVLKFUZYZNNJZMYKONUKFWAGFCYYJUCQNNYSCENBVKNTSWOSKCMRHFYYJKLXHECSKBZIAKJSHJNKWMAWFSNXHWRICYYYJESIIBYYTFHENNNLKCMRHBOZJZYINDHOFFYZSBKEPYNXHVUNKDTDJNZIHJQIGEHSVWHWZKEYZINZTMGIVJNQNAYNHAHIIBQXFQNZJMKLTLXSNNUMPAWSHJNCPVEZZSLKONCCZRLWBCUOHJNQTTKZQASSKNCBCQCXNENKABOTFCOXGDTNZKHWKJLLDBOCSDSEKNNQNINGFCNSCMSWVNCBCQNECATDABQXKWDFTSLZTWTXFUHSHNNQNATCAXOSQVUNGDMTZSYZFFNEPMKGFCLTZVYBWFNUCWZLVKLICNNJHDYZZMAENVUNKDTDWBQXNWDSEWSMNWSSUBTRSCLXJFRQFQYYJKLXHELQ";


            //SimpleSubstitutionDecrypt decryptor1 = new SimpleSubstitutionDecrypt(task3);
            //Console.WriteLine(decryptor1.Decrypt());
            SubstitutonPolyALphDecryptor decryptor2 = new SubstitutonPolyALphDecryptor(task4, 4);
            Console.WriteLine(decryptor2.Decrypt());

            Console.WriteLine("Press any key to continue...");
=======
            VigenereDecrypt vigenereDecrypt = new VigenereDecrypt(DecodeHexToUTF8
                (File.ReadAllText("../../../Decryption/SubstitutionDecript/tasks/task3.txt")), null);
            string result3 = vigenereDecrypt.Decrypt();
            Console.WriteLine("Vigenere Decrypt:\n" + result3 + "\n");
            Console.WriteLine("Press any key to start mono substitution decrypt");
>>>>>>> GeneticAlgoTesting
            Console.ReadKey();

<<<<<<< HEAD
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
=======
            #endregion

            #region Task4

            Console.Clear();
            Console.WriteLine("Initializing dictionaries...");

            Stopwatch task4Stopwatch = new Stopwatch();
            task4Stopwatch.Start();

            string task4 = File.ReadAllText("../../../Decryption/SubstitutionDecript/tasks/task4.txt").ToLower();
            
            GeneticModel gm = new GeneticModel(task4);
            string result4 = gm.Run();
            task4Stopwatch.Stop();

            Console.WriteLine("Result:\n");
            List<string> output = Decryption.WordNinja.WordNinja.Split(result4);
            foreach (var elem in output)
>>>>>>> GeneticAlgoTesting
            {
                Console.Write(elem + " ");
            }

<<<<<<< HEAD
            return result;
        }
=======
            Console.WriteLine("\n");
            Console.WriteLine("Execution time: " + (task4Stopwatch.ElapsedMilliseconds / 1000.0).ToString("F3") + " seconds\n");
            
>>>>>>> GeneticAlgoTesting

            Console.WriteLine("Press any key to start poly substitution decrypt");
            Console.ReadKey();
            #endregion

            #region Task5

            Console.Clear();
            Console.WriteLine("Initializing dictionaries...");

            Stopwatch task5Stopwatch = new Stopwatch();
            task5Stopwatch.Start();

            int[] ngrams = { 2, 3, 4 };
            var dict = DictionaryNGrams.GetDictionaryNGrams().Dictionary;
            var words = WordDictionary.GetWordDictionary().Dictionary;
            var smallWords = WordDictionary.GetWordDictionary().SmallDictionary;
            string task5 = File.ReadAllText("../../../Decryption/SubstitutionDecript/tasks/task5.txt").ToLower();

            string result5 = NewGeneticAlgo.GeneticAlgo(task5, 6, ngrams, dict, words, smallWords);

            task5Stopwatch.Stop();

            Console.WriteLine(result5);

            Console.WriteLine("Execution time: " + task5Stopwatch.Elapsed.Minutes + " minutes " + task5Stopwatch.Elapsed.Seconds + " seconds");
            #endregion
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
<<<<<<< HEAD

=======
>>>>>>> GeneticAlgoTesting

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
