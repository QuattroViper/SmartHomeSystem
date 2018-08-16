using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.classes
{
    public partial class GeoData
    {
        private string guid;
        private int weather;
        private int terrian;                // Get from the Enum
        private int howRemoteLocation;
        private decimal installAmountIncrease;
        private string description;

        public GeoData()
        {

        }

        public GeoData(string guid, int weather, int terrian, int howRemoteLocation, decimal installAmountIncrease, string description)
        {
            this.guid = guid;
            this.weather = weather;
            this.terrian = terrian;
            this.howRemoteLocation = howRemoteLocation;
            this.installAmountIncrease = installAmountIncrease;
            this.description = description;

        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        public decimal InstallAmountIncrease
        {
            get { return installAmountIncrease; }
            set { installAmountIncrease = value; }
        }

        public int HowRemoteLocation
        {
            get { return howRemoteLocation; }
            set { howRemoteLocation = value; }
        }


        public int Terrian
        {
            get { return terrian; }
            set { terrian = value; }
        }


        public int AverageWeather
        {
            get { return weather; }
            set { weather = value; }
        }

    }

    public partial class GeoData
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }

    public enum Terrian
    {
        HILL = 0, OASIS, OPEN, TUNDRA, DESERT, MOUNTAIN, FOREST, MARSH, SWAMP, RIVER, OCEAN
    }

    public enum Weather
    {
        Sunny = 0, Stormy, Rainy, Cloudy, Dry, Hurricanes, Foggy, Snow, drought, wildfire
    }

}
