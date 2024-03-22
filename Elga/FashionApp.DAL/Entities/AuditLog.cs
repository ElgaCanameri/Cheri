using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionApp.DAL.Entities
{
	public class AuditLog : BaseEntity<int>
	{
		public int ObjectId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
	}
}
