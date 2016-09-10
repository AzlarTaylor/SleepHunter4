﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SleepHunter.Converters
{
  public class VisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var boolean = (bool)value;
      var parameterString = parameter as string;

      if (string.Equals("Reverse", parameterString, StringComparison.OrdinalIgnoreCase))
        boolean = !boolean;

      if (boolean)
        return Visibility.Visible;
      else
      {
        if (string.Equals("Collapse", parameterString, StringComparison.OrdinalIgnoreCase))
          return Visibility.Collapsed;
        else
          return Visibility.Hidden;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var visibility = (Visibility)value;

      if (visibility == Visibility.Visible)
        return true;
      else
        return false;
    }
  }
}
