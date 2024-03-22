using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Entities;

namespace FashionApp.DAL.Repositories
{
	public interface ILogsRepository : IBaseRepository<AuditLog, int>
	{
	}
	internal class LogsRepository : BaseRepository<AuditLog, int>, ILogsRepository
	{
		public LogsRepository(AppDbContext dbContext) : base(dbContext)
		{
		}
	}
}
