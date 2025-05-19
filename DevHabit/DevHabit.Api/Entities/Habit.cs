namespace DevHabit.Api.Entities;

/// <summary>
/// 習慣
/// </summary>
public sealed class Habit
{
    /// <summary>
    /// 編號
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// ID
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 習慣類型: Binary, Measurable 
    /// </summary>
    public HabitType Type { get; set; }

    /// <summary>
    /// 執行頻率
    /// </summary>
    public Frequency Frequency { get; set; }

    /// <summary>
    /// 數值目標
    /// </summary>
    public Target Target { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public HabitStatus Status { get; set; }

    public bool IsArchived { get; set; }

    public DateOnly? EndDate { get; set; }

    public Milestone? Milestone { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? LastCompletedAtUtc { get; set; }

    public List<HabitTag> HabitTags { get; set; }

    public List<Tag> Tags { get; set; }
}

/// <summary>
/// 習慣種類: Binary, Measurable
/// </summary>
public enum HabitType
{
    None = 0,
    Binary = 1,
    Measurable  =2,
}

/// <summary>
/// 執行狀態: 未設定, 執行中, 已完成
/// </summary>
public enum HabitStatus
{
    None = 0,
    Ongoing = 1,
    Completed = 2,
}

/// <summary>
/// 執行頻率資訊
/// </summary>
public sealed class Frequency
{
    public FrequencyType Type { get; set; }

    public int TimesPerPeriod { get; set; }
}

/// <summary>
/// 執行頻率種類: 每天, 每周, 每月
/// </summary>
public enum FrequencyType
{
    None = 0,
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
}

/// <summary>
/// 執行目標資訊
/// </summary>
public sealed class Target
{
    public int Value { get; set; }

    public string Unit { get; set; }    
}

/// <summary>
/// 執行次數: 目標數量, 已完成數量
/// </summary>
public sealed class Milestone
{
    public int Target { get; set; }

    public int Current { get; set; }
}
