using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FashionApp.BLL.Services
{
	internal class LogsService : IHostedService
	{
		private readonly IWriteLog _logMethod;
		private readonly IServiceProvider _serviceProvider;
		private string _dir;
		public string Dir
		{
			get { return _dir; }
			set
			{
				try
				{
					if (!Directory.Exists(value))
					{
						Directory.CreateDirectory(value);
						if (!Directory.Exists(value))
							throw new Exception("Direktoria nuk ekziston");
					}
					_dir = value;
				}
				catch { }
			}
		}
		public LogsService(IConfiguration configuration, IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			var logMethod = configuration["AppSettings:LogsConfig"];
			Dir = configuration["AppSettings:LogsDirectory"];

			switch (logMethod)
			{
				case "Database":
					_logMethod = WriteLogToDatabase;
					break;
				case "File":
					_logMethod = WriteLogToFile;
					break;
				default:
					throw new ArgumentOutOfRangeException("Nuk njihet lloji i loggerit");
			}
		}
		public void WriteLogToFile(DAL.Entities.AuditLog auditLog)
		{
			try
			{
				var fileName = $"{Dir}/{DateTime.UtcNow:dd-MM-yyyy}.txt";
				StringBuilder sb = new StringBuilder();
				sb.AppendLine($"========{DateTime.UtcNow:dd-MM-yyyy HH:mm:ss}=======");
				sb.AppendLine(auditLog.Type + " :" + " ID: " + auditLog.ObjectId + "; " + auditLog.Description);
				using (StreamWriter sw = File.AppendText(fileName))
				{
					sw.WriteLine(sb.ToString());
					sw.Close();
				}
			}
			catch { }
		}
		public void WriteLogToDatabase(DAL.Entities.AuditLog auditLog)
		{
			try
			{
				using (var scope = _serviceProvider.CreateScope())
				{
					var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
					unitOfWork.LogsRepository.Add(auditLog);
					unitOfWork.Commit();
				}
			}
			catch
			{

			}
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			ProductService.OnLogOccured += _logMethod;
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			ProductService.OnLogOccured += _logMethod;
			return Task.CompletedTask;
		}
	}
}
