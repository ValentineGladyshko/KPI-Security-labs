using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Decryption.Proto4Task
{
    public class WordDictionary
    {
        private WordDictionary()
        {
            LoadDictionary();
        }
        private static readonly WordDictionary _wordDictionary = new WordDictionary();

        public static WordDictionary GetWordDictionary()
        {
            return _wordDictionary;
        }

        public HashSet<string> Dictionary { get; set; }

        private void LoadDictionary()
        {
            string[] lines = File.ReadAllLines("../../../Decryption/SubstitutionDecript/dictionary/Words.txt");
            Dictionary = new HashSet<string>(lines);
        }
    }
}
