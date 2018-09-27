using System;
using System.Collections.Generic;
using System.IO;

namespace Decryption
{
    class Program
    {
        // To DO List:

        // 1. Write effective algorithm to find text is normal or not
        // (based on dictionary of common words and n-grams)

        // 2. Maybe need to read source strings from files
        // and write results to files

        // 3. Maybe needed to refactor code

        // 4. Write Vinegere Decryption - DONE

        // 5. Write a code to attack some simple substitution cipher. 
        //To reduce the complexity of this one we will use only uppercase letters, so the keyspace is only 26! 
        //To get this one right automatically you will probably need to use some sort of genetic algorithm (which worked the best last year),
        //simulated annealing or gradient descent. Seriously, write it right now, you will need it to decipher the next one as well. 
        //Bear in mind, therea??s no spaces.

        static void Main(string[] args)
        {

            #region Task1
            string task1 = @"]|d3gaj3r3avcvrgz}t>xvj3K\A3pzc{va=3V=t=3zg3`{|f.w3grxv3r3`gaz}t31{v..|3d|a.w13r" +
                "}w?3tzev}3g{v3xvj3z`31xvj1?3k|a3g{v3uza`g3.vggva31{13dzg{31x1?3g{v}3k|a31v13dzg{31v1?3g{v}3" +
                "1.13dzg{31j1?3r}w3g{v}3k|a3}vkg3p{ra31.13dzg{31x13rtrz}?3g{v}31|13dzg{31v13r}w3`|3|}=3J|f3~" +
                "rj3f`v3z}wvk3|u3p|z}pzwv}pv?3[r~~z}t3wz`gr}pv?3Xr`z`xz3vkr~z}rgz|}?3`grgz`gzpr.3gv`g`3|a3d{" +
                "rgveva3~vg{|w3j|f3uvv.3d|f.w3`{|d3g{v3qv`g3av`f.g=";
            CeasarDecrypt ceasarDecrypt = new CeasarDecrypt(task1);
            //ceasarDecrypt.Decrypt();
            #endregion

            #region Task2

            string task2 = "1c41023f564b2a130824570e6b47046b521f3f5208201318245e0e6b40022643072e13183e51183f" +
                "5a1f3e4702245d4b285a1b23561965133f2413192e571e28564b3f5b0e6b50042643072e4b023f4a4b24554b3f5" +
                "b0238130425564b3c564b3c5a0727131e38564b245d0732131e3b430e39500a38564b27561f3f5619381f4b385c" +
                "4b3f5b0e6b580e32401b2a500e6b5a186b5c05274a4b79054a6b67046b540e3f131f235a186b5c052e131922540" +
                "33f130a3e470426521f22500a275f126b4a043e131c225f076b431924510a295f126b5d0e2e574b3f5c4b3e400e" +
                "6b400426564b385c193f13042d130c2e5d0e3f5a086b52072c5c192247032613433c5b02285b4b3c5c1920560f6" +
                "b47032e13092e401f6b5f0a38474b32560a391a476b40022646072a470e2f130a255d0e2a5f0225544b24414b2c" +
                "410a2f5a0e25474b2f56182856053f1d4b185619225c1e385f1267131c395a1f2e13023f13192254033f1305244" +
                "4476b4a043e131c225f076b5d0e2e574b22474b3f5c4b2f56082243032e414b3f5b0e6b5d0e33474b245d0e6b52" +
                "186b440e275f456b710e2a414b225d4b265a052f1f4b3f5b0e395689cbaa186b5d046b401b2a500e381d61";
            string result1 = DecodeHexToUTF8(task2);
            VigenereDecrypt vigenereDecrypt = new VigenereDecrypt(result1, null);
            vigenereDecrypt.Decrypt();
            #endregion


            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
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
