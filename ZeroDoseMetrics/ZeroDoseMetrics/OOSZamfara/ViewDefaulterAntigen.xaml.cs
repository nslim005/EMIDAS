using System;
using System.Collections.Generic;
using SQLite;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class ViewDefaulterAntigen : ContentPage
	{

		
		public ViewDefaulterAntigen (DefaulterList list)
		{
			InitializeComponent ();


			using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
			{
                int id = list.Id;

                conn.CreateTable<DefaulterList>();
                var record = conn.Table<DefaulterList>().Where(x => x.Id == id).FirstOrDefault();
				
				ChildNameEntry.Text = record.ChildName;
				ChildIDEntry.Text = record.ChildID;

                if (string.IsNullOrEmpty(record.AntigensReceived))
				{
                    AntigenEditor.Text = "NONE";
                }
				else
				{
                    AntigenEditor.Text = record.AntigensReceived;
                }
				

            }

        }
	}
}

