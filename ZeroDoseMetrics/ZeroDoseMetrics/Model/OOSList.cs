using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class OOSList
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string VaccinatorName { get; set; }

        public string VaccinatorNumber { get; set; }

        public string VaccinatorSupName { get; set; } // new

        public string TeamCode { get; set; }

        public string Respondent { get; set; }

        public string HouseHoldHeadName { get; set; }

        public string HouseHoldPhone { get; set; }

        public string CaregiverName { get; set; }

        public string ChildID { get; set; }

        public string ChildName { get; set; }

        public string RICardAvailable { get; set; } //new

        public string Gender { get; set; }

        public string HasReceivedAntigen { get; set; }

        public string OldAntigensReceived { get; set; }

        public string AntigensReceived { get; set; }

        public string IsRecordTally { get; set; } 

        public string AntigensFromChildHealthCard { get; set; }

        public string AEFI { get; set; }

        public string AEFIType { get; set; }

        public string Age { get; set; }

        public string CurrentAge { get; set; }

        public string AgeCategory { get; set; }

        public string CaregiverNumber { get; set; }

        public string CatchmentAreaHF { get; set; }

        public string SettlementName { get; set; }

        public string InternationalBorderSettlementType { get; set; } //new

        public string NeighbouringCountryName { get; set; } //new

        public string SettlementHabitationStatus { get; set; } //new

        public string ReasonForDesertion { get; set; } //new

        public string AccessibilityStatus { get; set; } //new

        public string NomadicPopStayPeriod { get; set; } //new

        public string NomadicPopulationMove { get; set; } //new

        public string NomadicWhenMoving { get; set; } //new

        public string LGA { get; set; }

        public string Ward { get; set; }

        public string SettlementType { get; set; }

        public string StateFrom { get; set; } // new

        public string LGAFrom { get; set; } // new

        public string StateTo { get; set; } // new

        public string LGATo { get; set; } // new

        public string AFPCase { get; set; } //new

        public string AFPCaseCount { get; set; } //new

        public string AFPReportingDSNO { get; set; } //new

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Completed { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string VaccDate { get; set; }

        public string VaccTime { get; set; }

        public string Temp { get; set; }

        public string DueForNextAntigen { get; set; }

        public int isCheckedForSync { get; set; }

        public int uploaded { get; set; }

        public string VaccinationStatus { get; set; }

        public string TargetStatus { get; set; }

        
        public OOSList()
		{

		}
	}
}

