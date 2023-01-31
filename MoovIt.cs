using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MoovIt
{
    public class MoovIt : IMoovIt
    {

        private HashSet<Route> routes= new HashSet<Route>();
        private Dictionary<string, Route> routsById= new Dictionary<string, Route>();

        public MoovIt()
        {

        }


        public int Count => this.routes.Count;

        public void AddRoute(Route route)
        {
            if (this.Contains(route))
            {
                throw new ArgumentException();
            }

            this.routsById.Add(route.Id, route);
            this.routes.Add(route);
        }

        public void ChooseRoute(string routeId)
        {
            if (!this.routsById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            routsById[routeId].Popularity++;
        }

        public bool Contains(Route route)
        {

            if (this.routes.Contains(route))
            {
                return true;
            }

            return false;
           
        }

        public IEnumerable<Route> GetFavoriteRoutes(string destinationPoint)
        {                      

            return this.routes.Where(x => x.IsFavorite == true &&
            x.LocationPoints[0] != destinationPoint
            && x.LocationPoints.Any(x => x == destinationPoint))
                .OrderBy(x => x.Distance)
                .ThenByDescending(x => x.Popularity);

        }

        public Route GetRoute(string routeId)
        {
            if (!this.routsById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            return routsById[routeId];


        }

        public IEnumerable<Route> GetTop5RoutesByPopularityThenByDistanceThenByCountOfLocationPoints()
        {
            
            return this.routes
                .OrderByDescending(x => x.Popularity)
                .ThenBy(x => x.Distance)
                .ThenBy(x => x.LocationPoints.Count)
                .Take(5);              

        }

        public void RemoveRoute(string routeId)
        {
            var route = this.routes.FirstOrDefault(r => r.Id == routeId);

            if (route == null) throw new ArgumentException();

            this.routes.Remove(route);

        }

        public IEnumerable<Route> SearchRoutes(string startPoint, string endPoint)
        {

            return this.routes
                .Where(x => x.LocationPoints.Contains(startPoint)
                && x.LocationPoints.Contains(endPoint)
                && x.LocationPoints.IndexOf(startPoint) < x.LocationPoints.IndexOf(endPoint))
                .OrderBy(x => x.IsFavorite)
                .ThenBy(x => x.LocationPoints.IndexOf(endPoint) - x.LocationPoints.IndexOf(startPoint))
                .ThenByDescending(x => x.Popularity);

        }
    }
}
