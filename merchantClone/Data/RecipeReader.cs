using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using merchantClone.Models;
using MonoGame.Extended.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace merchantClone.Data
{
    public class RecipeReader : JsonContentTypeReader<List<Recipe>>
    {
    }
    public class ItemReader : JsonContentTypeReader<List<Item>>
    {

    }
}