using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.models
{
    public static class ShiftManager
    {
        public static int? CurrentShiftID { get; set; }
        public static string CashierName { get; set; }
        public static DateTime? ShiftStartTime { get; set; }

        public static void StartShift(int shiftID, string cashierName, DateTime shiftStartTime)
        {
            CurrentShiftID = shiftID;
            CashierName = cashierName;
            ShiftStartTime = shiftStartTime;
        }

        public static void EndShift()
        {
            CurrentShiftID = null;
            CashierName = null;
            ShiftStartTime = null;
        }
    }
}
