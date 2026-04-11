using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class Passcode
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string PassCode { get; set; }

        public Passcode()
		{

		}
	}
}

