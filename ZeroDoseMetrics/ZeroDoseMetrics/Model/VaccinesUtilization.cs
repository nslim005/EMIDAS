using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class VaccinesUtilization
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string VaccinatorName { get; set; }

        public string VaccinatorNumber { get; set; }

        public string TeamCode { get; set; }

        public string LGA { get; set; }

        public string Ward { get; set; }

        public string CAHF { get; set; }

        public string Settlement { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string BCGDosesRecv { get; set; }

        public string BCGDosesOpnd { get; set; }

        public string HepBDosesRecv { get; set; }

        public string HepBDosesOpnd { get; set; }

        public string OPVRecv { get; set; }
        
        public string OPVOpend { get; set; }

        public string PentaRecv { get; set; }

        public string PentaOpend { get; set; }

        public string PCVRecv { get; set; }

        public string PCVOpend { get; set; }

        public string RotaRecv { get; set; }
        
        public string RotaOpend { get; set; }

        public string IPVRecv { get; set; }

        public string IPVOpend { get; set; }

        public string YFRecv { get; set; }

        public string YFOpend { get; set; }

        public string MenARecv { get; set; }
        
        public string MenAOpend { get; set; }

        public string MeaslesRecv { get; set; }

        public string MeaslesOpnd { get; set; }

        public string HPVRecv { get; set; }

        public string HPVOpend { get; set; }

        public string TDRecv { get; set; }
        
        public string TDOpend { get; set; }

        public string AFPCasesRecorded { get; set; }

        public string MeaslesCaseRecorded { get; set; }

        public string TFCasesRecorded { get; set; }

        public string choleraCasesRecorded { get; set; }

        public string diptheriaCasesRecorded { get; set; }

        public string meningitisCasesRecorded { get; set; }

        public string neoTetanisCasesRecorded { get; set; }

        public string others { get; set; }

        public string pcmReceived { get; set; }

        public string pcmOpened { get; set; }

        public string soapReceived { get; set; }

        public string soapOpened { get; set; }

        public VaccinesUtilization()
		{

		}
	}
}

