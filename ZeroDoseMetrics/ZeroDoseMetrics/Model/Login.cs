using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class Login
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string InterviewerName { get; set; }

        public string PhoneNo { get; set; }

        public string TeamCode { get; set; }

        public string HealthFacility { get; set; }

        public string Settlement { get; set; }

        public string UserId { get; set; }

        public string LGA { get; set; }

        public string Ward { get; set; }

        public Login()
		{
           
        }
	}
}

