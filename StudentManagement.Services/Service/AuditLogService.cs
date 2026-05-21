using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data.Context;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Admin;
using StudentManagement.Services.Helpers;

namespace StudentManagement.Services.Implementations;

public class AuditLogService : IAuditLogService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AuditLogService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context; _mapper = mapper;
    }

    public async Task<DataTableResponse<AuditLogViewModel>> GetDataTableAsync(DataTableRequest request)
    {
        var query = _context.AuditLogs.OrderByDescending(a => a.Timestamp).AsQueryable();
        var total = await query.CountAsync();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(a =>
                (a.KullaniciAdi != null && a.KullaniciAdi.ToLower().Contains(s)) ||
                (a.EntityName   != null && a.EntityName.ToLower().Contains(s))   ||
                (a.Details      != null && a.Details.ToLower().Contains(s)));
        }

        var filteredTotal = await query.CountAsync();
        var data = await query.Skip(request.Start).Take(request.Length).ToListAsync();

        return new DataTableResponse<AuditLogViewModel>
        {
            Draw = request.Draw, RecordsTotal = total, RecordsFiltered = filteredTotal,
            Data = _mapper.Map<IEnumerable<AuditLogViewModel>>(data).ToList()
        };
    }

    public async Task<IEnumerable<AuditLogViewModel>> GetByKullaniciAsync(int kullaniciId, int count = 20)
    {
        var logs = await _context.AuditLogs
            .Where(a => a.KullaniciId == kullaniciId)
            .OrderByDescending(a => a.Timestamp)
            .Take(count)
            .ToListAsync();
        return _mapper.Map<IEnumerable<AuditLogViewModel>>(logs);
    }
}
