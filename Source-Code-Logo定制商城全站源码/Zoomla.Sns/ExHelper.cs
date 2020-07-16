using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoomLa.Sns
{
    public class ExHelper
    {
        public static string ShowCountry(M_Order_Contact model)
        {
            string text = model.Country.ToLower();
            if (text.Contains("canada"))
            {
                return "CANADA";
            }
            return "USA";
        }

        public static string ShowCityAndState(M_Order_Contact model)
        {
            string stateShort = ExHelper.GetStateShort(model.Country, model.State);
            return string.Format("{0}, {1} {2}", model.City, stateShort, model.Zip.ToUpper());
        }

        public static string GetStateShort(string country, string state)
        {
            state = state.Trim();
            string[] array = "Alberta:AB|British Columbia:BC|Manitoba:MB|Newfoundland and Labrador:NL|New Brunswick:NB|Northwest Territories:NT|Nova Scotia:NS|Nunavut:NU|Ontario:ON|Prince Edward Island:PE|Quebec:QC|Saskatchewan:SK|Yukon:YT".Split('|');
            string[] array2 = "Alabama:AL|Alaska:AK|Arizona:AZ|Arkansas:AR|California:CA|Colorado:CO|Connecticut:CT|Delaware:DE|Florida:FL|Georgia:GA|Hawaii:HI|Idaho:ID|Illinois:IL|Indiana:IN|Iowa:IA|Kansas:KS|Kentucky:KY|Louisiana:LA|Maine:ME|Maryland:MD|Massachusett:MA|Michigan:MI|Minnesota:MN|Mississippi:MS|Missouri:MO|Montana:MT|Nebraska:NE|Nevada:NV|New hamp:NHshire|New jersey:NJ|New mexico:NM|New York:NY|North Carolina:NC|North Dakota:ND|Ohio:OH|Oklahoma:OK|Oregon:OR|Pennsylvania:PA|Rhode island:RI|South carolina:SC|South dakota:SD|Tennessee:TN|Texas:TX|Utah:UT|Vermont:VT|Virginia:VA|Washington:WA|West Virginia:WV|Wisconsin:WI|Wyoming:WY".Split('|');
            if (country.ToLower().Contains("canada"))
            {
                string[] array3 = array;
                foreach (string text in array3)
                {
                    string text2 = text.Split(':')[0];
                    string result = text.Split(':')[1];
                    if (text2.Equals(state))
                    {
                        return result;
                    }
                }
            }
            else
            {
                string[] array4 = array2;
                foreach (string text3 in array4)
                {
                    string text4 = text3.Split(':')[0];
                    string result2 = text3.Split(':')[1];
                    if (text4.Equals(state))
                    {
                        return result2;
                    }
                }
            }
            return state;
        }
    }
}
