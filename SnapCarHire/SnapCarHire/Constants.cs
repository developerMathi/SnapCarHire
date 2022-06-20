using SnapCarHireModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnapCarHire
{
    public class Constants
    {
        //public static int ClientId = 971;
        //public static int ClientId = 369;
        //public static int ClientId = 565;
        //public static int ClientId = 1028;
        //public static int ClientId = 1308; //production testing client
        //public static int ClientId = 1469; //production Evolve
        public static int ClientId = 1308; //production ML
        //public static int ClientId = 975;
        //public static int ClientId = 262;

        public static Admin admin = null;
        public static CutomerAuthContext cutomerAuthContext = null;
        public static CustomerReview customerDetails = null;
        public static List<int> countriesHasState = new List<int>() { 144, 121, 33, 34, 103, 198, 202, 69, 212, 2 };


        // using for find home page or not to enable back key
        public static bool IsHome = false;
        public static bool IsRegisteredandNotLogin = false;
        internal static bool IsHomeDetail;

        public void setAdmin(Admin admi)
        {
            admin = admi;
        }
    }
}
