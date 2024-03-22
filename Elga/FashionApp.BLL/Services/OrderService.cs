using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.BLL.DTO;
using FashionApp.BLL.DTO.Responses;
using FashionApp.DAL.Entities;

namespace FashionApp.BLL.Services
{
	public interface IOrderService
	{
		public StandardViewResponse<bool> AddOrder(Order order);
		public List<Order> GetAllOrdersByUserId(int userId);
		public List<Order> GetAllOrders();
	}
	public class OrderService : BaseService, IOrderService
	{
		public OrderService(IServiceProvider unitOfWork) : base(unitOfWork)
		{
		}
		public StandardViewResponse<bool> AddOrder(Order order)
		{
			try
			{
				var res = _unitOfWork.ExecuteTransaction<StandardViewResponse<bool>>(() =>
				{
					_unitOfWork.OrdersRepository.Add(order);
					_unitOfWork.Commit();
					if (order.Id > 0)
					{
						return new StandardViewResponse<bool>(true);
					}
					return new StandardViewResponse<bool>(false, "The order could not be processed.");
				});
				return res;
			}
			catch (Exception ex) { }
			return new StandardViewResponse<bool>(false, "The order could not be processed.");
		}

        public List<Order> GetAllOrders()
        {
			try
			{
				var allOrders = _unitOfWork.OrdersRepository.GetAll().ToList();
				return allOrders;
			}
			catch (Exception)
			{

			}
			return new List<Order>();
        }

        public List<Order> GetAllOrdersByUserId(int userId)
		{
			var orders = new List<Order>();
			try
			{
				orders = _unitOfWork.OrdersRepository.GetOrdersByUserId(userId);
			}
			catch (Exception)
			{

			}
			return orders;
		}
	}
}
