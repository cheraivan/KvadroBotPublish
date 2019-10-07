using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace KopterBot.Commons
{
    class RegularExpression
    {
        public static bool IsTelephoneCorrect(string input)
        {
            const string myReg1 = @"((\+38|8|\+3|\+ )[ ]?)?([(]?\d{3}[)]?[\- ]?)?(\d[ -]?){6,14}";
            if (Regex.IsMatch(input, myReg1))
                return true;
            return false;
        }
    }
}
