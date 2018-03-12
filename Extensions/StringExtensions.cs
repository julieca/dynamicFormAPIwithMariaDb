using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Medico.Core.Extensions
{
    public static class StringExtensions
    {
        public static string JoinClaim(this string _string, string Claim)
        {
            string[] _temp1 = _string.Split(",");
            string[] _temp2 = Claim.Split(",");

            int _max = (_temp1.Count() > _temp2.Count()) ? _temp1.Count() : _temp2.Count();
            int _min = (_temp1.Count() < _temp2.Count()) ? _temp1.Count() : _temp2.Count();

            string[] _temp3 = new string[_max];
            for (int i = 0; i <= _max - 1; i++)
            {
                if (i >= _min)
                {
                    if (_temp1.Count() > _temp2.Count())
                    {
                        _temp3[i] = _temp1[i];
                    }
                    else if (_temp1.Count() < _temp2.Count())
                    {
                        _temp3[i] = _temp2[i];
                    }
                    else
                    {
                        _temp3[i] = ((_temp1[i] == "1") || (_temp2[i] == "1")) ? "1" : "0";
                    }
                }
                else
                {
                    _temp3[i] = ((_temp1[i] == "1") || (_temp2[i] == "1")) ? "1" : "0";
                }
            }
            string value = string.Join(",", _temp3);
            return value;
        }

        public static string RandomPass(this string data)
        {
            var chars1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var chars2 = "abcdefghijklmnopqrstuvwxyz";
            var chars3 = "0123456789";
            var chars4 = "!@#$%^&*";
            var chars5 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            string password = Convert.ToString(chars1[random.Next(chars1.Length)]);
            password = password + Convert.ToString(chars2[random.Next(chars2.Length)]);
            password = password + Convert.ToString(chars3[random.Next(chars3.Length)]);
            password = password + Convert.ToString(chars4[random.Next(chars4.Length)]);
            for (int i = 0; i < 4; i++)
            {
                password = password + Convert.ToString(chars5[random.Next(chars5.Length)]);
            }
            return password;
        }

        public static List<string> GetErrorList(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                from error in state.Errors
                select error.ErrorMessage;
            var errorList = query.ToList();
            return errorList;
        }

        public static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    //Console.WriteLine(jex.Message);
                    //return false;
                }
                catch (Exception ex) 
                {
                    //some other exception
                    //Console.WriteLine(ex.ToString());
                    //return false;
                }
            }
            return false;
        }
    }
}
