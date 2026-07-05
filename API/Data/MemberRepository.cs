using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MemberRepository(AppDbContext Context) : IMemberRepository
{
    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await Context.Members.FindAsync(id);
    }

    public async Task<IReadOnlyList<Member>> GetMembersAsync()
    {
        return await Context.Members.ToListAsync();
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosByMemberIdAsync(string memberId)
    {
        return await Context.Members.Where(x => x.Id == memberId).SelectMany(x => x.Photos).ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await Context.SaveChangesAsync() > 0;
    }

    public void Update(Member member)
    {
        Context.Entry(member).State = EntityState.Modified;
    }
}