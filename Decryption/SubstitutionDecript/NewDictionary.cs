using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Decryption.SubstitutionDecript
{
    public class NewDictionary
    {
        private NewDictionary()
        {
            LoadBiDict();
            LoadTriDict();
            LoadQuadDict();
        }
        private static readonly NewDictionary _newDictionary = new NewDictionary();

        public static NewDictionary GetNewDictionary()
        {
            return _newDictionary;
        }

        public Dictionary<string, Dictionary<int, double>> BiDict { get; set; }
        public Dictionary<string, Dictionary<int, double>> TriDict { get; set; }
        public Dictionary<string, Dictionary<int, double>> QuadDict { get; set; }

        private void LoadBiDict()
        {
            BiDict = new Dictionary<string, Dictionary<int, double>>();
            double sum = 0.0;
            
            string[] lines = File.ReadAllLines("../../../Decryption/SubstitutionDecript/ngrams/ngrams2.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(',');
                BiDict[line[0].ToLower()] = new Dictionary<int, double>();
                for (int j = 2; j < 10; j++)
                {
                    sum += Convert.ToDouble(line[j]);
                }
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(',');
                for (int j = 2; j < 10; j++)
                {
                    BiDict[line[0].ToLower()][j] = Convert.ToDouble(line[j]) / sum;
                }
            }
        }

        private void LoadTriDict()
        {
            TriDict = new Dictionary<string, Dictionary<int, double>>();
            double sum = 0.0;

            string[] lines = File.ReadAllLines("../../../Decryption/SubstitutionDecript/ngrams/ngrams3.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(',');
                TriDict[line[0].ToLower()] = new Dictionary<int, double>();
                for (int j = 2; j < 9; j++)
                {
                    sum += Convert.ToDouble(line[j]);
                }
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(',');
                for (int j = 2; j < 9; j++)
                {
                    TriDict[line[0].ToLower()][j + 1] = Convert.ToDouble(line[j]) / sum;
                }
            }
        }

        private void LoadQuadDict()
        {
            QuadDict = new Dictionary<string, Dictionary<int, double>>();
            double sum = 0.0;

            string[] lines = File.ReadAllLines("../../../Decryption/SubstitutionDecript/ngrams/ngrams4.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(',');
                QuadDict[line[0].ToLower()] = new Dictionary<int, double>();
                for (int j = 2; j < 8; j++)
                {
                    sum += Convert.ToDouble(line[j]);
                }
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(',');
                for (int j = 2; j < 8; j++)
                {
                    QuadDict[line[0].ToLower()][j + 2] = Convert.ToDouble(line[j]) / sum;
                }
            }
        }
    }
}
