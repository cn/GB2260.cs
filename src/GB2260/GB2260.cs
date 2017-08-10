using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GB2260
{
    public class Gb2260
    {
        private static Gb2260 _instance;
        private static readonly object Locker = new object();
        public Revision Revision {get;set;}
        private readonly Dictionary<string, string> _data;
        public List<Division> Provinces {get;set;}

        private Gb2260(Revision revision)
        {
            Revision = revision;
            _data = new Dictionary<string, string>();
            Provinces = new List<Division>();
            try
            {
                var fileName = $"GB2260.{(int)revision}.tsv";
                var assembly = typeof(Gb2260).GetTypeInfo().Assembly;
                using(var stream = assembly.GetManifestResourceStream(fileName))
                using (var textReader = new StreamReader(stream))
                {
                    string line;
                    while( (line = textReader.ReadLine()) != null )
                    {
                        var split = line.Split('\t');
                        var code = split[2];
                        var name = split[3];
                        _data.Add(code, name);
                        if (Regex.IsMatch(code, "^\\d{2}0{4}$"))
                        {
                            var source = split[0];
                            var intRevision = int.Parse(split[1]);
                            Division division = new Division
                            {
                                Source = source,
                                Revision = (Revision)intRevision,
                                Code = code,
                                Name = name
                            };
                            Provinces.Add(division);

                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error in loading GB2260 data!");
                throw;
            }


        }
        public static Gb2260 GetInstance(Revision revision = Revision.V201607)
        {
            
            if (_instance == null)
            {
                lock (Locker)
                {
                    
                    if (_instance == null)
                    {
                        _instance = new Gb2260(revision);
                    }
                }
            }
            return _instance;
        }   

        public Division GetDivision(string code)
        {
            if (code.Length != 6)
            {
                throw new InvalidDataException("Invalid code");
            }

            if (!_data.ContainsKey(code))
            {
                return null;
            }

            Division division = new Division
            {
                Name = _data[code],
                Revision = Revision,
                Code = code
            };

            if (Regex.IsMatch(code, "^\\d{2}0{4}$"))
            {
                return division;
            }

            string provinceCode = code.Substring(0, 2) + "0000";
            division.Province = _data[provinceCode];

            if (Regex.IsMatch(code, "^\\d{4}0{2}$"))
            {
                return division;
            }

            string prefectureCode = code.Substring(0, 4) + "00";
            division.Prefecture = _data[prefectureCode];
            return division;
        }
        public List<Division> GetPrefectures(string code)
        {
            List<Division> rv = new List<Division>();

            if (!Regex.IsMatch(code, "^\\d{2}0{4}$"))
            {
                throw new InvalidDataException("Invalid province code");
            }

            if (!_data.ContainsKey(code))
            {
                throw new InvalidDataException("Province code not found");
            }

            Division province = GetDivision(code);

            string pattern = "^" + code.Substring(0, 2) + "\\d{2}00$";
            foreach (var key in _data.Keys)
            {
                if (Regex.IsMatch(key, pattern))
                {
                    Division division = GetDivision(key);
                    division.Province = province.Name;
                    rv.Add(division);
                }

            }

            return rv;
        }

        public List<Division> GetCounties(String code)
        {
            List<Division> rv = new List<Division>();

            if (!Regex.IsMatch(code, "^\\d+[1-9]0{2,3}$"))
            {
                throw new InvalidDataException("Invalid prefecture code");
            }

            if (!_data.ContainsKey(code))
            {
                throw new InvalidDataException("Prefecture code not found");
            }

            Division prefecture = GetDivision(code);
            Division province = GetDivision(code.Substring(0, 2) + "0000");

            string pattern = "^" + code.Substring(0, 4) + "\\d+$";
            foreach (var key in _data.Keys)
            {
                if (Regex.IsMatch(key, pattern))
                {
                    Division division = GetDivision(key);
                    division.Province = province.Name;
                    division.Prefecture = prefecture.Name;
                    rv.Add(division);
                }
            }

            return rv;
        }
    }
}
