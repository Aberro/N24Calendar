using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace N24Calendar.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	public static readonly Dictionary<DayOfWeek, string> DayOfWeekShortNames = new()
	{
		{ DayOfWeek.Monday, "Mo" },
		{ DayOfWeek.Tuesday, "Tu" },
		{ DayOfWeek.Wednesday, "We" },
		{ DayOfWeek.Thursday, "Th" },
		{ DayOfWeek.Friday, "Fr" },
		{ DayOfWeek.Saturday, "Sa" },
		{ DayOfWeek.Sunday, "Su" }
	};
	public interface IChartCell
	{
	}
	public record ChartCell : IChartCell
	{
		public bool IsActive { get; init; }
		public DateTime Time { get; init; }
		public ChartCell(bool isActive, DateTime time)
		{
			IsActive = isActive;
			Time = time;
		}
	}
	public record ChartCornerCell : IChartCell
	{
	}
	public record ChartColumnHeaderCell(DateTime Date) : IChartCell
	{
		public string DayOfWeek => DayOfWeekShortNames[Date.DayOfWeek];
	}
	public record ChartRowHeaderCell(int Hour) : IChartCell;

	private int _circadianCycleHours = 24;
	private int _circadianCycleMinutes = 0;
	private int _circadianCycleSeconds = 0;
	private DateTime _wakeUpTime = DateTime.Now;
	private DateTime _activityChartStartingDate = DateTime.Now.Date;
	private TimeSpan _sleepSpan = TimeSpan.FromHours(8);
	public int ActivityChartDays => 60;

	public DateTimeOffset ActivityChartStartingDate
	{
		get => _activityChartStartingDate;
		set
		{
			if(_activityChartStartingDate.Date != value.Date)
			{
				OnPropertyChanging(nameof(ActivityChartStartingDate));
				_activityChartStartingDate = new DateTime(value.Year, value.Month, value.Day);
				OnPropertyChanged(nameof(ActivityChartStartingDate));
				RaiseChartUpdate();
			}
		}
	}
	public IEnumerable<IChartCell> ActivityChartData
	{
		get
		{
			yield return new ChartCornerCell();
			for(int d = 0; d < 60; d++)
				yield return new ChartColumnHeaderCell(_activityChartStartingDate.Date.AddDays(d));
			for(int h = 0; h < 24; h++)
			{
				yield return new ChartRowHeaderCell(h);
				for(int d = 0; d < 60; d++)
				{
					var time = _activityChartStartingDate.Date.AddDays(d).AddHours(h);
					yield return new ChartCell(GetActivity(time), time);
				}
			}
		}
	}
	public int CircadianCycleHours
	{
		get => _circadianCycleHours;
		set
		{
			SetProperty(ref _circadianCycleHours, value);
			RaiseChartUpdate();
		}
	}
	public int CircadianCycleMinutes
	{
		get => _circadianCycleMinutes;
		set
		{
			SetProperty(ref _circadianCycleMinutes, value);
			RaiseChartUpdate();
		}
	}
	public int CircadianCycleSeconds
	{
		get => _circadianCycleSeconds;
		set
		{
			SetProperty(ref _circadianCycleSeconds, value);
			RaiseChartUpdate();
		}
	}
	public DateTime WakeUpDate
	{
		get => _wakeUpTime.Date;
		set
		{
			if(_wakeUpTime.Date != value.Date)
			{
				OnPropertyChanging(nameof(WakeUpDate));
				_wakeUpTime = new DateTime(value.Year, value.Month, value.Day, _wakeUpTime.Hour, _wakeUpTime.Minute, _wakeUpTime.Second);
				OnPropertyChanged(nameof(WakeUpDate));
				RaiseChartUpdate();
			}
		}
	}
	public TimeSpan WakeUpTime
	{
		get => _wakeUpTime.TimeOfDay;
		set
		{
			
			if(_wakeUpTime.TimeOfDay != value)
			{
				OnPropertyChanging(nameof(WakeUpTime));
				_wakeUpTime = new DateTime(_wakeUpTime.Year, _wakeUpTime.Month, _wakeUpTime.Day, value.Hours, value.Minutes, value.Seconds);
				OnPropertyChanged(nameof(WakeUpTime));
				RaiseChartUpdate();
			}
		}
	}

	public bool GetActivity(DateTime time)
	{
		var cycle = TimeSpan.FromHours(_circadianCycleHours) +
				TimeSpan.FromMinutes(_circadianCycleMinutes) +
				TimeSpan.FromSeconds(_circadianCycleSeconds);

		var delta = time - _wakeUpTime;
		var offset = ((delta.Ticks % cycle.Ticks) + cycle.Ticks) % cycle.Ticks; // ensure non-negative
		var wakeSpan = cycle - _sleepSpan;
		return offset < wakeSpan.Ticks;
	}

	public void RaiseChartUpdate()
	{
		OnPropertyChanged(nameof(ActivityChartData));
	}
}
