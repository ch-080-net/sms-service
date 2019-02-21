using WebCustomerApp.Models;

namespace Model.Interfaces
{
	public interface ITariffRepository : IBaseRepository<Tariff>
	{
		void ChangeTariffLimit(Tariff currentTariff, int newLimit);
	}
}
