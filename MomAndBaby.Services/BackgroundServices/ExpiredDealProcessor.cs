using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MomAndBaby.Core.Base;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Interface;

namespace MomAndBaby.Services.BackgroundServices
{
    public class ExpiredDealProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ExpiredDealProcessor> _logger;

        public ExpiredDealProcessor(IServiceScopeFactory serviceScopeFactory, ILogger<ExpiredDealProcessor> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var today = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7)).Date;

                    // Lấy tất cả deal đã hết hạn và chưa bị xóa
                    await unitOfWork.BeginTransactionAsync();
                    try
                    {
                        var deals = await unitOfWork.GenericRepository<Deal>()
                                                  .GetAllAsync(d => d.EndDate.Date < today
                                                                    && d.Status == BaseEnum.Active.ToString(), null);

                        if (deals.Any())
                        {
                            foreach (var deal in deals)
                            {
                                deal.Status = BaseEnum.Deleted.ToString();
                                unitOfWork.GenericRepository<Deal>().Update(deal);
                            }

                            await unitOfWork.SaveChangeAsync();
                            await unitOfWork.CommitTransactionAsync();

                            _logger.LogInformation("{Count} expired deal(s) processed and marked as Deleted.", deals.Count);
                        }
                        else
                        {
                            await unitOfWork.RollbackTransactionAsync();
                            _logger.LogInformation("No expired deals found at {Time}.", DateTimeOffset.Now);
                        }
                    }
                    catch (Exception ex)
                    {
                        await unitOfWork.RollbackTransactionAsync();
                        _logger.LogError("Error occurred while processing expired deals: {Message}", ex.Message);
                    }
                }

                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }

}
