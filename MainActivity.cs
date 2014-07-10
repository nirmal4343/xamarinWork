using System;
using System.Linq;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Threading.Tasks;
using System.Json;
using System.IO;

namespace ListViewSample
{
	[Activity (Label = "ListViewSample", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : ListActivity
	{
		List<TableItem> items = new List<TableItem> ();
		ProgressDialog progress;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);
			progress = new ProgressDialog(this);
			progress.Indeterminate = true;
			progress.SetProgressStyle(ProgressDialogStyle.Spinner);
			progress.SetMessage("Contacting server. Please wait...");
			progress.SetCancelable(false);
			progress.Show();
			createListView ();
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			var t = items[position];
			Android.Widget.Toast.MakeText(this, t.Heading, Android.Widget.ToastLength.Short).Show();
		}

		/*  // Sample data to test list view
		private List<TableItem> createSampleData (int range){

			var itemsList = new List<TableItem>();

			for (int r = 0; r < range; r++) {

				var items = new TableItem {

					Heading = string.Format("Item 1 {0}", r) , 
					SubHeading = string.Format("Item Description {0}", r)
				};
				itemsList.Add (items);
			}
			return itemsList;

		}
		*/

		public  void createListView()
		{
			var url = "http://learnresfull-restcall.rhcloud.com/restaurent/";
			var itemsList = new List<TableItem>();
			var httpReq = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			httpReq.BeginGetResponse ((ar) => {
				var request = (HttpWebRequest)ar.AsyncState;

				using (var response = (HttpWebResponse)request.EndGetResponse (ar)) {

					var s = response.GetResponseStream ();

					var j = (JsonArray)JsonArray.Load (s);

					for (int r = 0; r < j.Count; r++) {
						var obj = j[r];
						Console.WriteLine ("## Val ## {0}", obj["firstName"]);
						var itm = new TableItem {
							Heading = obj["firstName"] , 
							SubHeading = obj["title"],
							DownloadUrl = obj["imageDownloadPath"]
						};
						items.Add (itm);
					}

					RunOnUiThread (() => {
						progress.Dismiss();
						ListAdapter = new HomeScreenAdapter(this, items);
						ListView.FastScrollEnabled = true;

					});
	

				}            

			}, httpReq);
		}

	}
}


