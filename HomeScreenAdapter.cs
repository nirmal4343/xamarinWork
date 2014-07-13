using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.Threading;
using Android.OS;
using Android.Graphics;
using Android.Util;

namespace ListViewSample
{
	public class HomeScreenAdapter : BaseAdapter<TableItem> {
		List<TableItem> items;
		Activity context;
		private readonly BitmapCache cache;
		public HomeScreenAdapter(Activity context, List<TableItem> items)
			: base()
		{
			this.context = context;
			this.items = items;
			cache = new BitmapCache(50);
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override TableItem this[int position]
		{
			get { return items[position]; }
		}
		public override int Count
		{
			get { return items.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
				view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
			view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Heading;
			view.FindViewById<TextView>(Resource.Id.Text2).Text = item.SubHeading;
			//view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(item.ImageResourceId);
			Console.WriteLine ("Preparing Row..");

			// Creating Thread to start downloading Tumb Nail Image for listview and attached in a list item
			Thread thread = new Thread(() =>
			{
					downloadImage (view.FindViewById<ImageView> (Resource.Id.Image),item.DownloadUrl, item.ImageName);
			});
			thread.Start();

			//downloadImage (view.FindViewById<ImageView> (Resource.Id.Image),item.DownloadUrl);
			return view;
		}


		public void downloadImage(ImageView tumb_nail, string image_url, string imageName)
		{

			if (cache.ContainsKey (imageName)) {
				Bitmap imageBitmap = (Bitmap)cache.Get(imageName);
				setImageToList (tumb_nail, imageBitmap);
			} else {
				var imageBitmap = GetImageBitmapFromUrl (image_url,imageName);
				setImageToList (tumb_nail, imageBitmap);
			}

		}

		private void setImageToList(ImageView tumb_nail,Bitmap imageObj){

			this.context.RunOnUiThread (() => {
				tumb_nail.SetImageBitmap (imageObj);
			});

		}


		private Bitmap GetImageBitmapFromUrl(string url, string imageName)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}
			AddBitmapToCache (imageName, imageBitmap);
			return imageBitmap;
		}


		public sealed class BitmapCache : LruCache
		{
			public BitmapCache(int maxSize)
				: base(maxSize)
			{
			}

			public bool ContainsKey(string url)
			{
				return Get(url) != null;
			}

		}

		private void AddBitmapToCache(string url, Bitmap bmp)
		{
			cache.Put(url, bmp);
		}

	}
}