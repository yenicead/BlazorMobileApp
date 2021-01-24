using System;
using System.Collections.Generic;

namespace HaversineDistanceCalculator
{
    public class DistanceCalculator : IDistanceCalculator
    {
        // Necessary information about Haversine formula: https://en.wikipedia.org/wiki/Haversine_formula
        private const double Radius = 6371; // Earth radius = 6371 km.

        public double Calculate(double firstLatitude, double firstLongitude, double secondLatitude, double secondLongitude)
        {
            double latitude = (secondLatitude - firstLatitude) * (Math.PI / 180);
            double longitude = (secondLongitude - firstLongitude) * (Math.PI / 180);

            firstLatitude *= (Math.PI / 180);
            secondLatitude *= (Math.PI / 180);

            return Radius * 2 * 
                   Math.Asin(
                       Math.Sqrt(
                            Math.Pow(Math.Sin(latitude / 2), 2) + 
                            Math.Pow(Math.Sin(longitude / 2), 2) * 
                            Math.Cos(firstLatitude) * 
                            Math.Cos(secondLatitude)
                       )
                   );
        }

        public Dictionary<string, double> GetClosestUsers(UserGpsInformation userGpsInformation, IEnumerable<UserGpsInformation> userGpsInformations)
        {
            Dictionary<string, double> keyValuePairs = new Dictionary<string, double>();
            foreach (UserGpsInformation otherUserCoordinates in userGpsInformations)
            {
                double distance = Calculate(userGpsInformation.Latitude, userGpsInformation.Longitude, otherUserCoordinates.Latitude, otherUserCoordinates.Longitude);
                if (keyValuePairs.ContainsKey(otherUserCoordinates.UserNickname))
                {
                    keyValuePairs[otherUserCoordinates.UserNickname] = distance;
                }
                else
                {
                    keyValuePairs.Add(otherUserCoordinates.UserNickname, distance);
                }
            }

            return keyValuePairs;
        }
    }
}
