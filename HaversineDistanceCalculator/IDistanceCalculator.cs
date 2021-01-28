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
    }
}
