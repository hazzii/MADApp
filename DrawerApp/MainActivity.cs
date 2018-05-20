using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;

using DrawerApp.Fragments;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Petrolhead;

using Android.Widget;
using System;
using System.IO;
using SQLite;

namespace DrawerApp
{
    [Activity(Label = "@string/app_name", LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon")]
    public class MainActivity : AppCompatActivity
    {
        private EditText txtUsername;
        private EditText txtPassword;
        private Button buttonRegister;
        private Button buttonLogin;

        //test comment
        DrawerLayout drawerLayout;
        NavigationView navigationView;

        IMenuItem previousItem;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);

            buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);
            buttonLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            txtUsername = FindViewById<EditText>(Resource.Id.txtUsername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);

            buttonLogin.Click += ButtonLogin_Click;
            buttonRegister.Click += ButtonRegister_Click;

            CreateDB();

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            //setup navigation view
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            //handle navigation
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                if (previousItem != null)
                    previousItem.SetChecked(false);

                navigationView.SetCheckedItem(e.MenuItem.ItemId);

                previousItem = e.MenuItem;

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_home_1:
                        ListItemClicked(0);
                        break;
                    case Resource.Id.nav_home_2:
                        ListItemClicked(1);
                        break;
                    case Resource.Id.nav_home_3:
                        ListItemClicked(2);
                        break;
                }


                drawerLayout.CloseDrawers();
            };


            //if first time you will want to go ahead and click the first item.
            if (savedInstanceState == null)
            {
                navigationView.SetCheckedItem(Resource.Id.nav_home_1);
                ListItemClicked(0);
            }
        }

        private void ButtonRegister_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal
                ), "user.db3");
                var conn = new SQLiteConnection(dbPath);
                var data = conn.Table<LoginTable>();

                var loginRecord = data.Where(x => x.username == txtUsername.Text && x.password == txtPassword.Text)
                    .FirstOrDefault();
                if (loginRecord != null)
                {
                    Toast.MakeText(this, "Login Success!", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Username or Password incorrect", ToastLength.Short).Show();
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(this, exception.ToString(), ToastLength.Short).Show();
            }
        }

        public string CreateDB()
        {
            var output = "";
            output += "Creating Database if is does not exist";
            string dbPath = Path.Combine(System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal
            ), "user.db3");
            var conn = new SQLiteConnection(dbPath);
            output += "\n Database Created...";
            return output;
        }


        int oldPosition = -1;
        private void ListItemClicked(int position)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            if (position == oldPosition)
                return;

            oldPosition = position;

            Android.Support.V4.App.Fragment fragment = null;
            switch (position)
            {
                case 0:
                    fragment = Fragment1.NewInstance();
                    break;
                case 1:
                    fragment = Fragment2.NewInstance();
                    break;
                case 2:
                    fragment = Fragment3.NewInstance();
                    break;
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

