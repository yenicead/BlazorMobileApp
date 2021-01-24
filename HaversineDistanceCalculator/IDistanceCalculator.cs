using System;
using System.Collections.Generic;
using System.Text;

namespace HaversineDistanceCalculator
{
    /// <summary>
    /// This interface simply calculates distances of two points with using Haversine formula.
    /// </summary>
    public interface IDistanceCalculator
    {
        /// <summary>
        /// Gets two points coordinates and returns distances as meter between these two points.
        /// </summary>
        /// <param name="firstLatitude">Latitude information of the first point.</param>
        /// <param name="firstLongitude">Longitude information of the first point</param>
        /// <param name="secondLatitude">Latitude information of the second point.</param>
        /// <param name="secondLongitude">Longitude information of the second point.</param>
        /// <returns>Distance as meter.</returns>
        double Calculate(double firstLatitude, double firstLongitude, double secondLatitude, double secondLongitude);

        /// <summary>
        /// Gets current user information and list of other user locations then returns the closest users to current user.
        /// </summary>
        /// <param name="userGpsInformation">Current user information.</param>
        /// <param name="userGpsInformations">List of other users' informations.</param>
        /// <returns>List of other users with distance information.</returns>
        Dictionary<string, double> GetClosestUsers(UserGpsInformation userGpsInformation, IEnumerable<UserGpsInformation> userGpsInformations);
    }
}
