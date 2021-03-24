using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TennisScoreCalculationApi.Model;

namespace TennisScoreCalculationUI.UI.Converters
{
    public class IntAndAdvToScoreConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values != null && parameter != null)
                {
                    var playerScore = int.Parse(values[0].ToString());
                    var playerIndex = int.Parse(parameter.ToString());
                    ESidesInGame advToSide;

                    if (Enum.TryParse<ESidesInGame>(values[1].ToString(), out advToSide))
                    {
                        if (advToSide == ESidesInGame.None)
                        {
                            switch (playerScore)
                            {
                                case 0:
                                    return "0";
                                case 1:
                                    return "15";
                                case 2:
                                    return "30";
                                case 3:
                                    return "40";
                                case 4:
                                    return "60";
                                default:
                                    return "0";
                            }
                        }
                        else
                        {
                            return (int)advToSide == playerIndex ? "AD" : "-";
                        }
                    }
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
