using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace N24Calendar.Converters;

/// <summary>
/// Converts boolean value of activity to background color of a cell.
/// </summary>
public class TimeChartConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var wake = value is bool b ? b : false;
		if(wake)
		{
			Application.Current!.TryGetResource("SystemControlHighlightAccentBrush", Application.Current.ActualThemeVariant, out var resource);
			return resource ?? Brushes.Green;
		}
		else
		{
			Application.Current!.TryGetResource("SystemControlForegroundBaseLowBrush", Application.Current.ActualThemeVariant, out var resource);
			return resource ?? Brushes.DarkGray;
		}
	}
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}