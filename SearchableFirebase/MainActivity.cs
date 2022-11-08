using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using BumpTech.GlideLib;
using Com.Toptoche.Searchablespinnerlibrary;
using Firebase.Database;
using Google.Android.Material.BottomSheet;
using SearchableFirebase.Interface;
using SearchableFirebase.Model;
using Square.PicassoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using static Android.Widget.AdapterView;
using Movie = SearchableFirebase.Model.Movie;

namespace SearchableFirebase
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity,IFirebaseLoadCompleted,IValueEventListener,IOnItemSelectedListener
    {
        SearchableSpinner _searcheableSpinner;
        private DatabaseReference _movieReferences;
        IFirebaseLoadCompleted _loadCompleted;
        List<Movie> _movieList = new List<Movie>();
        BottomSheetDialog _bottomSheet;
        ImageView _image;
        TextView _movieTitle;
        TextView _resaleseYear;
        TextView _rating;
        private bool isFirstTime = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            UIReferences();

        }

        private void UIReferences()
        {
            _searcheableSpinner = FindViewById<SearchableSpinner>(Resource.Id.searchableSpinner);
            _searcheableSpinner.OnItemSelectedListener = this;
            _movieReferences = FirebaseDatabase.Instance.GetReference("Movies");
            _movieReferences.AddListenerForSingleValueEvent(this);
            _loadCompleted = this;

            _bottomSheet = new BottomSheetDialog(this);
            View bottomSheetView = LayoutInflater.Inflate(Resource.Layout.movieLayout, null);
            _image = bottomSheetView.FindViewById<ImageView>(Resource.Id.imageViewMovie);
            _movieTitle = bottomSheetView.FindViewById<TextView>(Resource.Id.textViewMovieTitle);
            _resaleseYear = bottomSheetView.FindViewById<TextView>(Resource.Id.textViewRealeseYear);
            _rating = bottomSheetView.FindViewById<TextView>(Resource.Id.textViewRating);
            _bottomSheet.SetContentView(bottomSheetView);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void onFirebaseLoadSuccess(List<Movie> movies)
        {
            _movieList = movies;
            List<string> name = new List<string>();
            foreach (var movie in _movieList)
                name.Add(movie.Title);
            ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(this,Resource.Layout.support_simple_spinner_dropdown_item,name);
            _searcheableSpinner.Adapter = arrayAdapter;
        }

        public void onFirebaseLoadFailed(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        public void OnCancelled(DatabaseError error)
        {
            _loadCompleted.onFirebaseLoadFailed(error.Message);
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            List<Movie> local = new List<Movie>();
            int i = 0;
            foreach (DataSnapshot snapshotMovie in snapshot.Children.ToEnumerable())
            {
                Movie movie = new Movie();
                movie.Image = snapshotMovie.Child("image").GetValue(true).ToString();
                movie.Title = snapshotMovie.Child("title").GetValue(true).ToString();
                movie.Rating = double.Parse(snapshotMovie.Child("rating").GetValue(true).ToString());
                movie.ReleaseYear = int.Parse(snapshotMovie.Child("releaseYear").GetValue(true).ToString());
              

                local.Add(movie);
            }
            _loadCompleted.onFirebaseLoadSuccess(local);
        }

        public void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            string gerne = null;

          if (isFirstTime)
            {
                Movie movie = _movieList[position];
                _movieTitle.Text = movie.Title;
                _rating.Text = movie.Rating.ToString();
                _resaleseYear.Text = movie.ReleaseYear.ToString();

                //  Glide.With(this).Load(movie.Image).Into(_image);

                _image.SetImageBitmap(BitmapFactory.DecodeFile(movie.Image));
                _bottomSheet.Show();
            }
          else
                isFirstTime = false;
        }

        public void OnNothingSelected(AdapterView parent)
        {
           
        }
    }
}