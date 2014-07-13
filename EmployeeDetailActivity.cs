
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ListViewSample
{
	[Activity (Label = "EmployeeDetailActivity")]			
	public class EmployeeDetailActivity : Activity
	{
		private EmployeeDatabase sqldb;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.employee_details);
			string text = Intent.GetStringExtra ("emp_id") ?? "0";
			sqldb = new EmployeeDatabase ("employee_db");

			Android.Database.ICursor result =  sqldb.GetRecordCursor (text);
			if (result.Count > 0) {
				result.MoveToFirst ();
				FindViewById<TextView>(Resource.Id.emp_name).Text = result.GetString(0) + " " + result.GetString(1) ;
				FindViewById<TextView>(Resource.Id.department).Text = result.GetString(2) ;
				FindViewById<TextView>(Resource.Id.officePhone).Text = result.GetString(3) ;
				FindViewById<TextView>(Resource.Id.cellphone).Text = result.GetString(4);
				FindViewById<TextView>(Resource.Id.title).Text = result.GetString(5) ;
				FindViewById<TextView>(Resource.Id.email).Text = result.GetString(6) ;
			}
		}
	}
}

