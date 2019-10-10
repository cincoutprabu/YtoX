//Repository.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

using YtoX.Entities;
using YtoX.Core.Storage;
using YtoX.Core.Weather;
using YtoX.Core.Location;

namespace YtoX.Core
{
    public class Repository
    {
        public static Repository Instance;

        #region Properties

        public List<RecipeGroup> Groups { get; set; }

        #endregion

        #region Constructors

        private Repository()
        {
            Groups = new List<RecipeGroup>();
        }

        #endregion

        #region Methods

        public RecipeGroup GetGroup(string name)
        {
            var matches = this.Groups.Where(group => group.Name.Equals(name));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public Recipe GetRecipe(string name)
        {
            var matches = this.Groups.SelectMany(group => group.Recipes).Where(item => item.Name.Equals(name));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public RecipeGroup GetGroupOfRecipe(Recipe recipe)
        {
            var matches = this.Groups.Where(group => group.Recipes.FirstOrDefault(r => r.Name == recipe.Name) != null);
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public void Save()
        {
            try
            {
                foreach (var group in this.Groups)
                {
                    foreach (var recipe in group.Recipes)
                    {
                        RecipeDAC.SetEnabled(recipe);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion

        #region Helper-Methods

        public static void Read()
        {
            try
            {
                var allGroups = RecipeGroupDAC.FetchAll();
                Log.Write("DatabasePath: " + DAC.DatabasePath);

                Repository.Instance = new Repository();
                Repository.Instance.Groups.AddRange(allGroups);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void GetWeatherRecipe()
        {
            Func<int, int, bool> f = new Func<int, int, bool>((a, b) => a > b);

            Func<int, int, int> sum = new Func<int, int, int>((a, b) => a + b);
            Func<int, int> square = new Func<int, int>(a => a ^ 2);
            Func<int, double> sqrt = new Func<int, double>(a => Math.Sqrt(a));

            Func<int, double> sqrt1 = a => Math.Sqrt(a);
            var ans = sqrt1(4);

            Expression<Func<int, double>> sqrtExpr = a => Math.Sqrt(a);
            var sqrtFn = sqrtExpr.Compile();
            var ans1 = sqrtFn(64);

            ParameterExpression paramA = Expression.Parameter(typeof(int), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(int), "b");
            BinaryExpression sumExpr = Expression.Add(paramA, paramB);
            //var sumLambda = Expression.Lambda(sumExpr, new ParameterExpression[] { paramA, paramB });
            var sumLambda1 = Expression.Lambda<Func<int, int, int>>(sumExpr, new ParameterExpression[] { paramA, paramB });
            var sumFn = sumLambda1.Compile();
            var sumAns = sumFn(3, 4);

            //WeatherHelper.GetWeather(lat,long) <= 30
            double latitude = 12.984722, longitude = 80.251944; //Chennai Perungudi

            //ParameterExpression latitudeParam = Expression.Parameter(typeof(double), "latitude");
            //ParameterExpression longitudeParam = Expression.Parameter(typeof(double), "longitude");
            //Expression left = Expression.Call(typeof(Wunderground).GetMethod("GetWeather"), new Expression[] { latitudeParam, longitudeParam });
            Expression left = Expression.Call(typeof(WeatherHelper).GetMethod("GetWeather"), new Expression[] { });
            Expression right = Expression.Constant(30.0, typeof(double));
            Expression condition = Expression.LessThanOrEqual(left, right);

            //Func<double, double, bool> fn = (lat, lon) => Wunderground.GetWeather(latitude, longitude) <= 30;
            Func<double, double, bool> fn = (lat, lon) => WeatherHelper.CurrentWeather.Temperature <= 30;
            //var lambda = Expression.Lambda(condition, new ParameterExpression[] { latitudeParam, longitudeParam });
            //var lambda = Expression.Lambda(condition, new ParameterExpression[] { });
            //var lambda = Expression.Lambda<Func<double, double, bool>>(condition, new ParameterExpression[] { latitudeParam, longitudeParam });
            //var fn = lambda.Compile();
            //var flag = fn(latitude, longitude);
            var flag = fn.DynamicInvoke(new object[] { latitude, longitude });

            //Expression<Func<float,bool>> weatherLambda=e
        }

        #endregion
    }
}
