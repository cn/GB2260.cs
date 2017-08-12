using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GB2260
{
    public class Gb2260
    {
        public Revision Revision {get;set;}
        private readonly Dictionary<string, string> _data;
        public List<Division> Provinces {get;set;}
        
        public Gb2260(Revision revision = Revision.V201607)
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
                            var division = new Division
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

            var division = new Division
            {
                Name = _data[code],
                Revision = Revision,
                Code = code
            };

            if (Regex.IsMatch(code, "^\\d{2}0{4}$"))
            {
                return division;
            }

            var provinceCode = $"{code.Substring(0, 2)}0000";
            division.Province = _data[provinceCode];

            if (Regex.IsMatch(code, "^\\d{4}0{2}$"))
            {
                return division;
            }

            var prefectureCode = $"{code.Substring(0, 4)}00";
            division.Prefecture = _data[prefectureCode];
            return division;
        }
        public List<Division> GetPrefectures(string code)
        {
            var lstDiv = new List<Division>();

            if (!Regex.IsMatch(code, "^\\d{2}0{4}$"))
            {
                throw new InvalidDataException("Invalid province code");
            }

            if (!_data.ContainsKey(code))
            {
                throw new InvalidDataException("Province code not found");
            }

            var province = GetDivision(code);

            var pattern = "^" + code.Substring(0, 2) + "\\d{2}00$";
            foreach (var key in _data.Keys)
            {
                if (Regex.IsMatch(key, pattern))
                {
                    var division = GetDivision(key);
                    division.Province = province.Name;
                    lstDiv.Add(division);
                }

            }

            return lstDiv;
        }

        public List<Division> GetCounties(string code)
        {
            var lstDiv = new List<Division>();

            if (!Regex.IsMatch(code, "^\\d+[1-9]0{2,3}$"))
            {
                throw new InvalidDataException("Invalid prefecture code");
            }

            if (!_data.ContainsKey(code))
            {
                throw new InvalidDataException("Prefecture code not found");
            }

            var prefecture = GetDivision(code);
            var province = GetDivision($"{code.Substring(0, 2)}0000");

            var pattern = "^" + code.Substring(0, 4) + "\\d+$";
            foreach (var key in _data.Keys)
            {
                if (Regex.IsMatch(key, pattern))
                {
                    var division = GetDivision(key);
                    division.Province = province.Name;
                    division.Prefecture = prefecture.Name;
                    lstDiv.Add(division);
                }
            }

            return lstDiv;
        }
    }
}
