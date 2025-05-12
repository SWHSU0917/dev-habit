using DevHabit.Api.Database;
using DevHabit.Api.DTOs.HabitTags;
using DevHabit.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("habits/{habitId}/tags")]
public class HabitTagsController(ApplicationDbContext dbContext) : ControllerBase
{
    // tags
    // PUT: habits/:id/tags/:tagId
    // 將 Habit 關聯的 Tag 更新為最後提供的 UpsertHabitTagsDto 內容為主
    [HttpPut]
    public async Task<ActionResult> AddTagToHabit(string habitId, UpsertHabitTagsDto upsertHabitTagsDto)
    {
        // 取得 Habit 資料及相關的 HabitTags
        Habit? habit = await dbContext.Habits
            .Include(h => h.HabitTags) // 確保載入 HabitTags
            .FirstOrDefaultAsync(h => h.Id == habitId);

        // 檢查 Habit 是否存在
        if (habit is null)
        {
            return NotFound();
        }

        // 取得目前 Habit 已經關聯的 TagId 集合
        // habit.HabitTags 是 Habit 和 Tag 之間的關聯資料，這裡提取出 TagId
        var currentTagIds = habit.HabitTags.Select(ht => ht.TagId).ToHashSet();

        // 比對當前 HabitTags 和使用者傳入的 TagIds 是否相同
        if (currentTagIds.SetEquals(upsertHabitTagsDto.TagIds))
        {
            return NoContent();
        }

        // 查詢資料庫中，哪些 TagIds 是有效的（即在 Tags 資料表中存在的 Tag）
        // 查詢所有使用者提供的 TagId 是否存在於 Tags 資料表中
        List<string> existingTagIds = await dbContext
            .Tags
            .Where(t => upsertHabitTagsDto.TagIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        // 如果存在的有效 TagIds 數量不等於使用者提供的 TagIds 數量，代表有無效的 TagId
        if (existingTagIds.Count != upsertHabitTagsDto.TagIds.Count)
        {
            return BadRequest("One or more tag IDs is invalid");
        }

        // 移除不再需要的 HabitTag 關聯，即那些不在使用者提供的 TagId 清單中的 HabitTags
        habit.HabitTags.RemoveAll(ht => !upsertHabitTagsDto.TagIds.Contains(ht.TagId));

        // 找出需要新增的 TagIds，這些是使用者提供但現在 Habit 沒有關聯的 TagIds
        string[] tagIdsAdd = upsertHabitTagsDto.TagIds.Except(currentTagIds).ToArray();

        // 新增這些需要加入的 HabitTag 關聯
        habit.HabitTags.AddRange(tagIdsAdd.Select(tagId => new HabitTag
        {
            HabitId = habitId,
            TagId = tagId,
            CreatedAtUtc = DateTime.UtcNow,
        }));

        await dbContext.SaveChangesAsync();

        return Ok();
    }


    // tags
    // DELETE: habits/:id/tags/:tagId
    [HttpDelete("{tagId}")]
    public async Task<ActionResult> DeleteHabitTag(string habitId, string tagId)
    {
        HabitTag? habitTag = await dbContext.HabitTags.SingleOrDefaultAsync(ht => ht.HabitId == habitId && ht.TagId == tagId);
        if (habitTag is null)
        {
            return NotFound();
        }

        dbContext.HabitTags.Remove(habitTag);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
