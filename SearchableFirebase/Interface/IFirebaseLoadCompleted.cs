using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SearchableFirebase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchableFirebase.Interface
{
    public interface IFirebaseLoadCompleted
    {
        public void onFirebaseLoadSuccess(List<Movie> movies);
        public void onFirebaseLoadFailed(string message);
    }
}