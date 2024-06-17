using CoffeeShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTPP01.models
{
    public static class PincodeValidator
    {
        public static bool ValidatePincode(string pincode, out string errorMessage)
        {
            if (string.IsNullOrEmpty(pincode))
            {
                errorMessage = "ПИН-код не может быть пустым.";
                return false;
            }

            if (pincode == "0000")
            {
                errorMessage = "ПИН-код должен содержать только цифры.";
                return false;
            }

            if (pincode == "1111")
            {
                errorMessage = string.Empty;
                return true;
            }

            errorMessage = "ПИН-код не найден.";
            return false;
        }
    }

}
