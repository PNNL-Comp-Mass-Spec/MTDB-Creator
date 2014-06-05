#region Namespaces

using System;
using System.Collections.Generic;
using System.Windows.Media;

#endregion

namespace MTDBCreator.Helpers
{
    public static class GraphHelper
    {
        public static Color PickColor()
        {
            const int deltaPercent = 15;
            var alreadyChoosenColors = new List<Color>();
            var tmpColor = Colors.Black;

            var chooseAnotherColor = true;
            while (chooseAnotherColor)
            {
                // create a random color by generating three random channels
                var redColor = Random.Next(0, 255);
                var greenColor = Random.Next(0, 255);
                var blueColor = Random.Next(0, 255);
                tmpColor = Color.FromArgb(255, Convert.ToByte(redColor), Convert.ToByte(greenColor), Convert.ToByte(blueColor));

                // check if a similar color has already been created
                chooseAnotherColor = false;

                foreach (var c in alreadyChoosenColors)
                {
                    var delta = c.R * deltaPercent / 100;
                    if (c.R - delta <= tmpColor.R && tmpColor.R <= c.R + delta)
                    {
                        chooseAnotherColor = true;
                        break;
                    }

                    delta = c.G * deltaPercent / 100;
                    if (c.G - delta <= tmpColor.G && tmpColor.G <= c.G + delta)
                    {
                        chooseAnotherColor = true;
                        break;
                    }

                    delta = c.B * deltaPercent / 100;
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

        private static readonly Random Random = new Random();
    }
}
