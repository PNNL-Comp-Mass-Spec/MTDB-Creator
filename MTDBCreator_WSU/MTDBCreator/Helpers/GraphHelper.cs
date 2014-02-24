#region Namespaces

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Data;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Wpf;
using LineAnnotation = OxyPlot.Wpf.LineAnnotation;

#endregion

namespace MTDBCreator.Helpers
{
    public static class GraphHelper
    {
        public static Color PickColor()
        {
            const int DELTA_PERCENT = 15;
            List<Color> alreadyChoosenColors = new List<Color>();
            Color tmpColor = Colors.Black;

            bool chooseAnotherColor = true;
            while (chooseAnotherColor)
            {
                // create a random color by generating three random channels
                int redColor = random.Next(0, 255);
                int greenColor = random.Next(0, 255);
                int blueColor = random.Next(0, 255);
                tmpColor = Color.FromArgb(255, Convert.ToByte(redColor), Convert.ToByte(greenColor), Convert.ToByte(blueColor));

                // check if a similar color has already been created
                chooseAnotherColor = false;

                foreach (Color c in alreadyChoosenColors)
                {
                    int delta = c.R * DELTA_PERCENT / 100;
                    if (c.R - delta <= tmpColor.R && tmpColor.R <= c.R + delta)
                    {
                        chooseAnotherColor = true;
                        break;
                    }

                    delta = c.G * DELTA_PERCENT / 100;
                    if (c.G - delta <= tmpColor.G && tmpColor.G <= c.G + delta)
                    {
                        chooseAnotherColor = true;
                        break;
                    }

                    delta = c.B * DELTA_PERCENT / 100;
                    if (c.B - delta <= tmpColor.B && tmpColor.B <= c.B + delta)
                    {
                        chooseAnotherColor = true;
                        break;
                    }
                }
            }
            alreadyChoosenColors.Add(tmpColor);
            // you can safely use the tmpColor here
            return tmpColor;
        }

        private static readonly Random random = new Random();
    }
}
