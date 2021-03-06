using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Gms.Maps;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using System.Threading.Tasks;


namespace ActionBarMaps
{
	[Activity (Label = "ActionBarCompat", Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat.Light", MainLauncher = true)]
	//[MetaData ("android.support.PARENT_ACTIVITY", Value = "actionbarcompat.MainActivity")]
	public class MainActivity : ActionBarActivity
	{
		DrawerLayout drawerLayout;
		ActionBarDrawerToggle drawerToggle;
		ListView drawerList;

		static string[] sections = new[] { "SupportFragment", "Map in FrameLayout", "Map in Fragment", "Partial Screen Map"};

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			drawerLayout.SetDrawerShadow (Resource.Drawable.drawer_shadow, (int)GravityFlags.Start);
			drawerList = FindViewById<ListView> (Resource.Id.flyout);
			drawerList.Adapter = new ArrayAdapter<string> (this, Resource.Layout.drawer_list_item, sections);


			drawerToggle = new ActionBarDrawerToggle (this, drawerLayout, Resource.String.drawer_open, Resource.String.drawer_close);

			drawerLayout.SetDrawerListener (drawerToggle);

			drawerList.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => ListItemClicked (e.Position);
			ListItemClicked (0);

			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
		}

		void ListItemClicked (int position)
		{
			SupportFragmentManager.PopBackStack (null, (int)PopBackStackFlags.Inclusive);

			Android.Support.V4.App.Fragment fragment = null;

			switch (position) 
			{
			case 0:
				//load a map fragment directly from the API
				fragment = new SupportMapFragment ();

				break;
			case 1:
				//load a fragment with a frame layout, create a SupportMapFragment,
				//and insert it into the frame layout
				fragment = new MyMapFragment ();
				break;
			case 2:
				//load a fragment with a map fragment defined in the layout
				fragment = new MyMapFragment2 ();
				break;
			case 3:
				//example of map in a fragment sharing screen space with other controls
				fragment = new MyMapFragment3 ();
				break;
			default:

				break;
			}

			// Insert the fragment by replacing any existing fragment
			SupportFragmentManager.BeginTransaction ()
				.Replace (Resource.Id.content_frame, fragment)
				.Commit ();

			// Highlight the selected item, update the title, and close the drawer
			drawerList.SetItemChecked (position, true);
			SupportActionBar.Title = sections [position];
			drawerLayout.CloseDrawer (drawerList);
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			drawerToggle.SyncState ();
		}
		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			drawerToggle.OnConfigurationChanged (newConfig);
		}
		// Pass the event to ActionBarDrawerToggle, if it returns
		// true, then it has handled the app icon touch event
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (drawerToggle.OnOptionsItemSelected (item))
				return true;

			return base.OnOptionsItemSelected (item);
		}
		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			var drawerOpen = drawerLayout.IsDrawerOpen (drawerList);
			//when open don't show anything
			for (int i = 0; i < menu.Size (); i++)
				menu.GetItem (i).SetVisible (!drawerOpen);

			return base.OnPrepareOptionsMenu (menu);
		}
	}
}


