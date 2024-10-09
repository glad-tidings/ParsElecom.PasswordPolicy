using System;
using System.Text.RegularExpressions;

namespace ParsElecom.PasswordPolicy
{
    public class PasswordPolicy
    {
        int _PasswordLength = -1;
        int _MinNoOfLowerCaseChars = -1;
        int _MinNoOfUpperCaseChars = -1;
        int _MinNoOfUniCaseChars = -1;
        int _MinNoOfNumbers = -1;
        int _MinNoOfSymbols = -1;
        int _MaxNoOfAllowedRepetitions = -1;
        string _DisallowedChars = string.Empty;
        string _UnicodeCharSetRanges = string.Empty;

        public string UnicodeCharSetRanges { get { return _UnicodeCharSetRanges; } set { _UnicodeCharSetRanges = value; } }
        public int MinPasswordLength { get { return _PasswordLength; } set { _PasswordLength = value; } }
        public int MinNoOfUpperCaseChars { get { return _MinNoOfUpperCaseChars; } set { _MinNoOfUpperCaseChars = value; } }
        public int MinNoOfLowerCaseChars { get { return _MinNoOfLowerCaseChars; } set { _MinNoOfLowerCaseChars = value; } }
        public int MinNoOfUniCaseChars { get { return _MinNoOfUniCaseChars; } set { _MinNoOfUniCaseChars = value; } }
        public int MinNoOfNumbers { get { return _MinNoOfNumbers; } set { _MinNoOfNumbers = value; } }
        public int MinNoOfSymbols { get { return _MinNoOfSymbols; } set { _MinNoOfSymbols = value; } }
        public int MaxNoOfAllowedRepetitions { get { return _MaxNoOfAllowedRepetitions; } set { _MaxNoOfAllowedRepetitions = value; } }
        public string DisallowedChars { get { return _DisallowedChars; } set { _DisallowedChars = value; } }

        public string GetExpression()
        {
            if (DisallowedChars.Length > 0)
            {
                DisallowedChars = DisallowedChars.Replace(@"\", @"\\");
            }

            string Unicase = String.IsNullOrEmpty(UnicodeCharSetRanges) ? "A-Z" : UnicodeCharSetRanges.Split(',')[0].Trim();
            string Lowercase = String.IsNullOrEmpty(UnicodeCharSetRanges) ?  "a-z" : (UnicodeCharSetRanges.Split(',').Length >= 2 ? UnicodeCharSetRanges.Split(',')[1] : "a-z");
 
            return @"^" 
                + (MaxNoOfAllowedRepetitions > -1 ? @"(?=^((.)(?!(.*?\2){" + (MaxNoOfAllowedRepetitions + 1).ToString() + @",}))+$)" : "")
                + (MinPasswordLength > -1 ? "(?=.{" + MinPasswordLength.ToString() + @",})" : "") 
                + (MinNoOfNumbers > -1 ? @"(?=([^0-9]*?\d){" + MinNoOfNumbers.ToString() + ",})" : "")
                + (MinNoOfUniCaseChars > -1 ? "(?=([^" + Unicase + @"]*?[" + Unicase + @"]){" + MinNoOfUniCaseChars.ToString() + @",})" : "")
                + (MinNoOfLowerCaseChars > -1 ? "(?=([^" + Lowercase + @"]*?[" + Lowercase + @"]){" + MinNoOfLowerCaseChars.ToString() + ",})" : "")
                + (MinNoOfUpperCaseChars > -1 ? "(?=([^" + Unicase + @"]*?[" + Unicase + @"]){" + MinNoOfUpperCaseChars.ToString() + @",})" : "")                 
                + (MinNoOfSymbols > -1 ? "(?=([" + Unicase + Lowercase + @"0-9]*?[^" + Unicase + Lowercase + @"]){" + MinNoOfSymbols.ToString() + ",})" : "")
                + (DisallowedChars.Length > 0 ? @"(?=[^" + DisallowedChars + @"]+$)" : "")
                + @".*$";
        }

        public bool MatchPassword(string Password)
        {
            return Regex.Match(Password, GetExpression()).Success;
        }
    }
}
