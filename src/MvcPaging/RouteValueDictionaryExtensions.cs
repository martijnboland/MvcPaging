using System.Web.Routing;

namespace MvcPaging
{
    public static class RouteValueDictionaryExtensions
    {
        /// <summary>
        /// Fix RouteValueDictionaries that contain arrays.
        /// Source: http://stackoverflow.com/a/5208050/691965
        /// </summary>
        /// <param name="routes"></param>
        /// <returns></returns>
        public static RouteValueDictionary FixListRouteDataValues(this RouteValueDictionary routes)
        {
            var newRv = new RouteValueDictionary();
            foreach (var key in routes.Keys)
            {
                var value = routes[key];
                if (value is System.Collections.IEnumerable && !(value is string))
                {
                    var index = 0;
                    foreach (var val in (System.Collections.IEnumerable)value)
                    {
                        newRv.Add(string.Format("{0}[{1}]", key, index), val);
                        index++;
                    }
                }
                else
                {
                    newRv.Add(key, value);
                }
            }
            return newRv;
        }
    }
}
