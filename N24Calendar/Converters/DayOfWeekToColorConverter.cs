using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace N24Calendar.Converters;

public class DayOfWeekToColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if(value is DayOfWeek.Sunday)
		{
			Application.Current!.TryGetResource("SystemControlHighlightAccentBrush", Application.Current.ActualThemeVariant, out var resource);
			return resource ?? Brushes.Red;
		}
		else
		{
			Application.Current!.TryGetResource("SystemControlForegroundBaseHighBrush", Application.Current.ActualThemeVariant, out var resource);
			return resource ?? Brushes.DarkGray;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotSupportedException();
}