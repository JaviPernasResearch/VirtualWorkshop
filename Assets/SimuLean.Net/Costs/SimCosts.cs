using System;
using System.Collections.Generic;
using System.Collections;

namespace simProcess
{
    public class SimCosts
    {

        public const double inventoryUnitCost = 0.10;
   
        public const double pendingOrderUnitCost = 0.40;

        public const double purchaseCost = 10.0;

        public const double processingCost = 9.0;

        public const double salePrice = 100.0;

        public const double shipmentCost = 40.0;

        public const double orderCost = 40.0;

        public static double totalCost = 0.0;
        public static double totalRevenue = 0.0;

        public static double costPerInventiryCapacity = 2.0;
        public static double costPerAssemblerCapacity = 250.0;
        public static double costPerWorkstationCapacity = 75.0;


        public static void addCost(double value)
        {
            totalCost += value;
        }

        public static void addRevenue(double value)
        {
            totalRevenue += value;
        }

        public static double getEarnings()
        {
            return totalRevenue - totalCost;
        }

        public static void restartEarnings()
        {
            totalCost = 0;
            totalRevenue = 0;
        }

    }
}