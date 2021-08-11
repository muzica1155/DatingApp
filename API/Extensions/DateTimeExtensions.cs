using System;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;// not true bcoz depends on what day today is wheher or not this user has already had their birthday today
            if (dob.Date > today.AddYears(-age)) age--;// is that is the case then we r going to say age-- bcoz they haven't had their birthday yet this year 
             return age;// this will give some1 age 

            
        }
        
    }
}