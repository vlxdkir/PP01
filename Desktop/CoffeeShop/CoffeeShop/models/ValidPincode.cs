using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.models
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

            

            if (!pincode.All(char.IsDigit))
            {
                errorMessage = "ПИН-код должен содержать только цифры.";
                return false;
            }

            using (var context = new CoffeeShopEntities())
            {
                var user = context.Employees.FirstOrDefault(u => u.Pincode == pincode);
                if (user == null)
                {
                    errorMessage = "ПИН-код не найден.";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
