using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchableFirebase.Model
{
    public class Movie
    {
        [JsonProperty("title")]
        public string Title;

        [JsonProperty("image")]
        public string Image;

        [JsonProperty("rating")]
        public double Rating;

        [JsonProperty("releaseYear")]
        public int ReleaseYear;

        [JsonProperty("genre")]
        public IDictionary<string,string> Genre;
    }
}