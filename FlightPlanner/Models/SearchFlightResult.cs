using System.Linq;
using Microsoft.AspNetCore.Http.Features;

namespace FlightPlanner.Models
{
    public class SearchFlightResults
    {
        private readonly Flight[] _items;
        private readonly int _page;
        private readonly int _totalItems;

        public SearchFlightResults(Flight[] items)
        {
            _items = items;
            _page = _items.Length > 0 ? 1 : 0;
            _totalItems = _items.Length;
        }

        public Flight[] items => _items.ToArray();
        public int page => _page;
        public int totalItems => _totalItems;
    }
}