using System;

namespace HaversineDistanceCalculator
{
    public class DistanceCalculator : IDistanceCalculator
    {
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
    }
}
