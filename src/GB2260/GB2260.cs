using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace GB2260
{
    public class GB2260
    {
        private readonly Revision revision;
        private Dictionary<String, String> data;
        private List<Division> provinces;

        public GB2260() : this(Revision.V2014)
        {

        }

        public GB2260(Revision revision)
        {
            this.revision = revision;
            data = new Dictionary<String, String>();
            provinces = new List<Division>();
            try
            {
                var path = "/data/" + (Int32)revision + ".txt";
                FileStream fileStream = new FileStream(path, FileMode.Open);
                using (StreamReader sr = new StreamReader(fileStream))
                {

                    while (sr.Peek() >= 0)
                    {
                        var line = sr.ReadLine();
                        var split = line.Split('\t');
                        var code = split[0];
                        var name = split[1];
                        data.Add(code, name);
                        if (Regex.IsMatch(code, "^\\d{2}0{4}$"))
                        {
                            Division division = new Division
                            {
                                Code = code,
                                Name = name
                            };
                            provinces.Add(division);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in loading GB2260 data!");
                throw ex;
            }


        }

        public Division getDivision(String code)
        {
            if (code.Length != 6)
            {
                throw new InvalidDataException("Invalid code");
            }

            if (!data.ContainsKey(code))
            {
                return null;
            }

            Division division = new Division
            {
                Name = data[code],
                Revision = revision,
                Code = code
            };

            if (Regex.IsMatch(code, "^\\d{2}0{4}$"))
            {
                return division;
            }

            String provinceCode = code.Substring(0, 2) + "0000";
            division.Province = data[provinceCode];

            if (Regex.IsMatch(code, "^\\d{4}0{2}$"))
            {
                return division;
            }

            String prefectureCode = code.Substring(0, 4) + "00";
            division.Prefecture = data[prefectureCode];
            return division;
        }

        public Revision getRevision()
        {
            return revision;
        }

        public List<Division> getProvinces()
        {
            return provinces;
        }

        public List<Division> getPrefectures(String code)
        {
            List<Division> rv = new List<Division>();

            if (!Regex.IsMatch(code, "^\\d{2}0{4}$"))
            {
                throw new InvalidDataException("Invalid province code");
            }

            if (!data.ContainsKey(code))
            {
                throw new InvalidDataException("Province code not found");
            }

            Division province = getDivision(code);

            string pattern = "^" + code.Substring(0, 2) + "\\d{2}00$";
            foreach (var key in data.Keys)
            {
                if (Regex.IsMatch(key, pattern))
                {
                    Division division = getDivision(key);
                    division.Province = province.Name;
                    rv.Add(division);
                }

            }

            return rv;
        }

        public List<Division> getCounties(String code)
        {
            List<Division> rv = new List<Division>();

            if (!Regex.IsMatch(code, "^\\d+[1-9]0{2,3}$"))
            {
                throw new InvalidDataException("Invalid prefecture code");
            }

            if (!data.ContainsKey(code))
            {
                throw new InvalidDataException("Prefecture code not found");
            }

            Division prefecture = getDivision(code);
            Division province = getDivision(code.Substring(0, 2) + "0000");

            string pattern = "^" + code.Substring(0, 4) + "\\d+$";
            foreach (var key in data.Keys)
            {
                if (Regex.IsMatch(key, pattern))
                {
                    Division division = getDivision(key);
                    division.Province = province.Name;
                    division.Prefecture = prefecture.Name;
                    rv.Add(division);
                }
            }

            return rv;
        }
    }
}
