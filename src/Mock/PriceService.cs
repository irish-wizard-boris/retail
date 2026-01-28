using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abysalto.Retail.Mock
{
    public interface IPriceService
    {
        public decimal GetPrice(Guid productId);
    }

    public class PriceService : IPriceService
    {
		public decimal GetPrice(Guid productId)
        {
			byte[] bytes = productId.ToByteArray();

			int hash = BitConverter.ToInt32(bytes, 0);

			hash = Math.Abs(hash);

			int scaledValue = hash % 1001; // 0-1000

			decimal result = 1.00m + (scaledValue / 1000.0m * 10.00m);

			return Math.Round(result, 2, MidpointRounding.AwayFromZero);
		}

	}
}
