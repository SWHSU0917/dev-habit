using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHabit.Api.Database.Configurations;

// 定義 HabitConfiguration 類別，實作 IEntityTypeConfiguration<Habit>，
// 用於設定 Habit 類別對應的資料庫欄位與結構。
public sealed class HabitConfiguration : IEntityTypeConfiguration<Habit>
{

    // 定義 HabitConfiguration 類別，實作 IEntityTypeConfiguration<Habit>，
    // 用於設定 Habit 類別對應的資料庫欄位與結構。
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        // 設定 Id 為主鍵
        builder.HasKey(h => h.Id);

        // 設定 Id 欄位的最大長度為 500（通常 Id 是字串）
        builder.Property(h => h.Id).HasMaxLength(500);

        // 設定 Name 欄位最大長度為 100
        builder.Property(h => h.Name).HasMaxLength(100);

        // 設定 Description 欄位最大長度為 500
        builder.Property(h => h.Description).HasMaxLength(500);

        // 設定 Frequency 為擁有型實體（值物件），
        // 它的屬性會當作 Habit 表中的欄位。
        builder.OwnsOne(h => h.Frequency);

        // 設定 Target 為擁有型實體，並進一步設定其內部屬性的欄位規則
        builder.OwnsOne(h => h.Target, targetBuilder =>
        {
            // 設定 Target 內部的 Unit 欄位最大長度為 100
            targetBuilder.Property(t => t.Unit).HasMaxLength(100);
        });

        // 設定 Milestone 為擁有型實體，其屬性會直接嵌入 Habit 資料表中
        builder.OwnsOne(h => h.Milestone);
    }
}
