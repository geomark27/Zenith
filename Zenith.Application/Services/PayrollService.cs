using Microsoft.EntityFrameworkCore;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Payroll;
using Zenith.Core.Entities;
using Zenith.Infrastructure.Data;

namespace Zenith.Application.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly ZenithDbContext _context;

        public PayrollService(ZenithDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PayrollResponseDto>> GetAllAsync(int tenantId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Payrolls
                .Where(p => p.TenantId == tenantId);

            if (startDate.HasValue)
                query = query.Where(p => p.PayPeriodStart >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.PayPeriodEnd <= endDate.Value);

            return await query
                .Include(p => p.Employee)
                .Include(p => p.StatusCatalog)
                .Include(p => p.PaymentMethodCatalog)
                .Select(p => new PayrollResponseDto
                {
                    Id = p.Id,
                    EmployeeId = p.EmployeeId,
                    EmployeeName = p.Employee.FirstName + " " + p.Employee.LastName,
                    PayPeriodStart = p.PayPeriodStart,
                    PayPeriodEnd = p.PayPeriodEnd,
                    PaymentDate = p.PaymentDate,
                    BaseSalary = p.BaseSalary,
                    Bonuses = p.Bonuses,
                    OvertimePay = p.OvertimePay,
                    Deductions = p.Deductions,
                    NetPay = p.NetPay,
                    StatusCatalogId = p.StatusCatalogId,
                    StatusName = p.StatusCatalog.Value,
                    PaymentMethodCatalogId = p.PaymentMethodCatalogId,
                    PaymentMethodName = p.PaymentMethodCatalog.Value,
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(p => p.PayPeriodStart)
                .ToListAsync();
        }

        public async Task<IEnumerable<PayrollResponseDto>> GetByEmployeeIdAsync(int employeeId, int tenantId)
        {
            return await _context.Payrolls
                .Where(p => p.EmployeeId == employeeId && p.TenantId == tenantId)
                .Include(p => p.Employee)
                .Include(p => p.StatusCatalog)
                .Include(p => p.PaymentMethodCatalog)
                .Select(p => new PayrollResponseDto
                {
                    Id = p.Id,
                    EmployeeId = p.EmployeeId,
                    EmployeeName = p.Employee.FirstName + " " + p.Employee.LastName,
                    PayPeriodStart = p.PayPeriodStart,
                    PayPeriodEnd = p.PayPeriodEnd,
                    PaymentDate = p.PaymentDate,
                    BaseSalary = p.BaseSalary,
                    Bonuses = p.Bonuses,
                    OvertimePay = p.OvertimePay,
                    Deductions = p.Deductions,
                    NetPay = p.NetPay,
                    StatusCatalogId = p.StatusCatalogId,
                    StatusName = p.StatusCatalog.Value,
                    PaymentMethodCatalogId = p.PaymentMethodCatalogId,
                    PaymentMethodName = p.PaymentMethodCatalog.Value,
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(p => p.PayPeriodStart)
                .ToListAsync();
        }

        public async Task<PayrollResponseDto?> GetByIdAsync(int id, int tenantId)
        {
            return await _context.Payrolls
                .Where(p => p.Id == id && p.TenantId == tenantId)
                .Include(p => p.Employee)
                .Include(p => p.StatusCatalog)
                .Include(p => p.PaymentMethodCatalog)
                .Select(p => new PayrollResponseDto
                {
                    Id = p.Id,
                    EmployeeId = p.EmployeeId,
                    EmployeeName = p.Employee.FirstName + " " + p.Employee.LastName,
                    PayPeriodStart = p.PayPeriodStart,
                    PayPeriodEnd = p.PayPeriodEnd,
                    PaymentDate = p.PaymentDate,
                    BaseSalary = p.BaseSalary,
                    Bonuses = p.Bonuses,
                    OvertimePay = p.OvertimePay,
                    Deductions = p.Deductions,
                    NetPay = p.NetPay,
                    StatusCatalogId = p.StatusCatalogId,
                    StatusName = p.StatusCatalog.Value,
                    PaymentMethodCatalogId = p.PaymentMethodCatalogId,
                    PaymentMethodName = p.PaymentMethodCatalog.Value,
                    CreatedAt = p.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PayrollResponseDto> CreateAsync(CreatePayrollDto dto, int userId)
        {
            var payroll = new Payroll
            {
                EmployeeId = dto.EmployeeId,
                PayPeriodStart = dto.PayPeriodStart,
                PayPeriodEnd = dto.PayPeriodEnd,
                PaymentDate = dto.PaymentDate,
                BaseSalary = dto.BaseSalary,
                Bonuses = dto.Bonuses,
                OvertimePay = dto.OvertimePay,
                Deductions = dto.Deductions,
                NetPay = dto.NetPay,
                StatusCatalogId = dto.StatusCatalogId,
                PaymentMethodCatalogId = dto.PaymentMethodCatalogId,
                TenantId = dto.TenantId,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            _context.Payrolls.Add(payroll);
            await _context.SaveChangesAsync();

            return (await GetByIdAsync(payroll.Id, dto.TenantId))!;
        }

        public async Task<PayrollResponseDto?> UpdateAsync(int id, UpdatePayrollDto dto, int tenantId, int userId)
        {
            var payroll = await _context.Payrolls
                .FirstOrDefaultAsync(p => p.Id == id && p.TenantId == tenantId);

            if (payroll == null)
                return null;

            if (dto.PaymentDate.HasValue) payroll.PaymentDate = dto.PaymentDate.Value;
            if (dto.Bonuses.HasValue) payroll.Bonuses = dto.Bonuses;
            if (dto.OvertimePay.HasValue) payroll.OvertimePay = dto.OvertimePay;
            if (dto.Deductions.HasValue) payroll.Deductions = dto.Deductions;
            if (dto.NetPay.HasValue) payroll.NetPay = dto.NetPay.Value;
            if (dto.StatusCatalogId.HasValue) payroll.StatusCatalogId = dto.StatusCatalogId.Value;
            if (dto.PaymentMethodCatalogId.HasValue) payroll.PaymentMethodCatalogId = dto.PaymentMethodCatalogId.Value;
            
            payroll.UpdatedAt = DateTime.UtcNow;
            payroll.UpdatedById = userId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id, tenantId);
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var payroll = await _context.Payrolls
                .FirstOrDefaultAsync(p => p.Id == id && p.TenantId == tenantId);

            if (payroll == null)
                return false;

            _context.Payrolls.Remove(payroll);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}