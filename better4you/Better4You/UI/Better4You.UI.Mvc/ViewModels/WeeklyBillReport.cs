namespace Better4You.UI.Mvc.ViewModels
{
    public class WeeklyBillReport
    {
        public long SchoolId { get; set; }
        public string SchoolName { get; set; }

        public int TotalStandardBreakfastServed { get; set; }
        public double TotalStandardBreakfastBill { get; set; }

        public int TotalVeganBreakfastServed { get; set; }
        public double TotalVeganBreakfastBill { get; set; }

        public int TotalSpecialBreakfastServed { get; set; }
        public double TotalSpecialBreakfastBill { get; set; }

        public int TotalStandardLunchServed { get; set; }
        public double TotalStandardLunchBill { get; set; }

        public int TotalVeganLunchServed { get; set; }
        public double TotalVeganLunchBill { get; set; }

        public int TotalSpecialLunchServed { get; set; }
        public double TotalSpecialLunchBill { get; set; }

        public int TotalSackLunchServed { get; set; }
        public double TotalSackLunchBill { get; set; }

        public int TotalSnackServed { get; set; }
        public double TotalSnackBill { get; set; }

        public int TotalSupperServed { get; set; }
        public double TotalSupperBill { get; set; }
       
        public double TotalBill
        {
            get
            {
                return (TotalStandardBreakfastBill + TotalVeganBreakfastBill + TotalSpecialBreakfastBill
                    + TotalStandardLunchBill + TotalVeganLunchBill + TotalSpecialLunchBill + TotalSackLunchBill+
                    TotalSnackBill + TotalSupperBill);
            }
        }

        public int TotalBreakfastServed
        {
            get
            {
                return (TotalStandardBreakfastServed + TotalVeganBreakfastServed + TotalSpecialBreakfastServed);
            }
        }
        public int TotalLunchServed
        {
            get
            {
                return (TotalStandardLunchServed + TotalVeganLunchServed + TotalSpecialLunchServed);
            }
        }

        public double TotalBreakfastBill
        {
           get
           {
               return (TotalStandardBreakfastBill + TotalVeganBreakfastBill + TotalSpecialBreakfastBill);
           }
        }
        public double TotalLunchBill
        {
            get
            {
                return (TotalStandardLunchBill+TotalVeganLunchBill+TotalSpecialLunchBill);
            }
        }

        public double breakfastRate { get; set; }
        public double lunchRate { get; set; }
        public double snackRate { get; set; }
        public double sackRate { get; set; }
        public double supperRate { get; set; }
       
    }
}
