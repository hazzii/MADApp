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
using System.IO;
using Petrolhead;
using SQLite;
using Environment = System.Environment;

namespace DrawerApp
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        private EditText txtRegUsername;
        private EditText txtRegPassword;
        private Button buttonCreate;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.new_user);

            buttonCreate = FindViewById<Button>(Resource.Id.buttonCreate);
            txtRegUsername = FindViewById<EditText>(Resource.Id.txtRegUsername);
            txtRegPassword = FindViewById<EditText>(Resource.Id.txtRegPassword);

            buttonCreate.Click += ButtonCreate_Click;
        }

        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal
                ), "user.db3");

                var conn = new SQLiteConnection(dbPath);
                conn.CreateTable<LoginTable>();
                LoginTable loginTable = new LoginTable();
                loginTable.username = txtRegUsername.Text;
                loginTable.password = txtRegPassword.Text;
                conn.Insert(loginTable);
                Toast.MakeText(this, "Record Added Successfully...", ToastLength.Short).Show();

            }
            catch (Exception exception)
            {
                Toast.MakeText(this, exception.ToString(), ToastLength.Short).Show();

            }
        }
    }
}