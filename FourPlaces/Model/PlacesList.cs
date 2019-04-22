using System;
using System.Collections.Generic;
using System.Text;

namespace FourPlaces.Model
{
    public class PlacesList
    {
        public List<PlaceItemSummary> Places { get; set; }

        public PlacesList()
        {
            this.Places = new List<PlaceItemSummary>()
            {
            };
        }

        public PlacesList(List<PlaceItemSummary> list)
        {
            this.Places = list;
        }

    }
}
