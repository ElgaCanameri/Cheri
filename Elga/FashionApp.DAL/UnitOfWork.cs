using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionApp.DAL.Repositories;

namespace FashionApp.DAL
{
	public interface IUnitOfWork
	{
		IProductsRepository ProductsRepository { get; }
		IFavouritesRepository FavouritesRepository { get; }
		ICartRepository CartRepository { get; }
		ICartProductRepository CartProductRepository { get; }
		IOrdersRepository OrdersRepository { get; }
		ICategoryRepository CategoryRepository { get; }
		ILogsRepository LogsRepository { get; }
		T ExecuteTransaction<T>(Func<T> action);

		void Commit();
	}
	internal class UnitOfWork : IUnitOfWork//encapsulation
	{
		private readonly AppDbContext _appDbContext;

		public UnitOfWork(AppDbContext dbContext)
		{
			_appDbContext = dbContext;
		}
		private IProductsRepository _productsRepository;
		public IProductsRepository ProductsRepository
		{
			get
			{
				_productsRepository ??= new ProductsRepository(_appDbContext);
				return _productsRepository;
			}
		}

		private IFavouritesRepository _favouritesRepository;
		public IFavouritesRepository FavouritesRepository
		{
			get
			{
				_favouritesRepository ??= new FavouritesRepository(_appDbContext);
				return _favouritesRepository;
			}
		}
		private ICartRepository _cartRepository;
		public ICartRepository CartRepository
		{
			get
			{
				_cartRepository ??= new CartRepository(_appDbContext);
				return _cartRepository;
			}
		}

		private ICartProductRepository _cartProductRepository;
		public ICartProductRepository CartProductRepository
		{
			get
			{
				_cartProductRepository ??= new CartProductRepository(_appDbContext);
				return _cartProductRepository;
			}
		}
		public IOrdersRepository _ordersRepository;
		public IOrdersRepository OrdersRepository
		{
			get
			{
				_ordersRepository ??= new OrdersRepository(_appDbContext);
				return _ordersRepository;
			}
		}

		private ICategoryRepository _categoryRepository;
		public ICategoryRepository CategoryRepository
		{
			get
			{
				_categoryRepository ??= new CategoryRepository(_appDbContext);
				return _categoryRepository;
			}
		}

		private ILogsRepository _logsRepository;
		public ILogsRepository LogsRepository
		{
			get
			{
				_logsRepository ??= new LogsRepository(_appDbContext);
				return _logsRepository;
			}
		}
		public void Commit()
		{
			_appDbContext.SaveChanges();
		}

		public T ExecuteTransaction<T>(Func<T> action)
		{
			var transaction = _appDbContext.Database.BeginTransaction();
			try
			{
				var result = action();
				transaction.Commit();
				return result;
			}
			catch
			{
				transaction.Rollback();
				throw;
			}
		}
	}
}
