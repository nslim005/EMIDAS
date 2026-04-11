using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class AppVersion
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Version { get; set; }

        public int Status { get; set; }

		public AppVersion()
		{

		}
	}
}

