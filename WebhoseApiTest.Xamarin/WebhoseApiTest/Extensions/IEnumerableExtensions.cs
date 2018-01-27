using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebhoseApiTest.Extensions
{
    public static class IEnumerableExtensions
    {
        static Random _myRandom = new Random();

        public static T Random<T>(this IEnumerable<T> ie)
        {
            return ie.ElementAt(_myRandom.Next(ie.Count()));
        }
    }
}
