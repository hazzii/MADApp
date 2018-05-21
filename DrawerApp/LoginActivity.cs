using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using System.IO;
using DrawerApp;
using SQLite;
using Android;
using Android.Content;

namespace DrawerApp
{
    [Activity(Label = "Petrolhead")]
    public class LoginActivity : Activity
    {
        private EditText txtUsername;
        private EditText txtPassword;
        private Button buttonRegister;
        private Button buttonLogin;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.login);

            buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);
            buttonLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            txtUsername = FindViewById<EditText>(Resource.Id.txtUsername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);

            buttonLogin.Click += ButtonLogin_Click;
            buttonRegister.Click += ButonRegister_Click;

            CreateDB();
        }

        private void ButonRegister_Click(object sender, EventArgs e)
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
                    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
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
    }
}

