﻿using DevHabit.Api.DTOs.Habits;

namespace DevHabit.Api.DTOs.Tags;

public record TagDto
{
    public required string Id { get; init; }

    public required string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public required DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }
}
