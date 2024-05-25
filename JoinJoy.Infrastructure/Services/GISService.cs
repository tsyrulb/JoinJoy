﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Services
{
    public class GISService : IGISService
    {
        public double CalculateGeographicalDistance(Location loc1, Location loc2)
        {
            // Simplified distance calculation
            var d1 = loc1.Latitude * (System.Math.PI / 180.0);
            var num1 = loc1.Longitude * (System.Math.PI / 180.0);
            var d2 = loc2.Latitude * (System.Math.PI / 180.0);
            var num2 = loc2.Longitude * (System.Math.PI / 180.0) - num1;
            var d3 = System.Math.Pow(System.Math.Sin((d2 - d1) / 2.0), 2.0) +
                     System.Math.Cos(d1) * System.Math.Cos(d2) * System.Math.Pow(System.Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * System.Math.Atan2(System.Math.Sqrt(d3), System.Math.Sqrt(1.0 - d3))) / 1000; // km
        }
    }
}
