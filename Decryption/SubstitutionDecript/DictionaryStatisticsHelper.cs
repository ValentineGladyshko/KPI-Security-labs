using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Decryption.SubstitutionDecript
{
    public class DictionaryStatisticsHelper
    {
        //singleton
        private DictionaryStatisticsHelper()
        {
            Dictionary = LoadDictionary();
        }
        private static readonly DictionaryStatisticsHelper _dictionaryStatisticsHelper = new DictionaryStatisticsHelper();

        public static DictionaryStatisticsHelper GetDictionaryStatistics()
        {
            return _dictionaryStatisticsHelper;
        }

        public HashSet<string> Dictionary { get; set; }

        private HashSet<string> LoadDictionary()
        {
            string[] lines = File.ReadAllLines("../../../Decryption/SubstitutionDecript/dictionary/words_alpha.txt");
            return new HashSet<string>(lines);
        }
    }
}
