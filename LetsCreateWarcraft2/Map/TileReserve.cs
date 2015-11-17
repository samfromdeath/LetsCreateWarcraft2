using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetsCreateWarcraft2.Map
{
    class TileReserve : Tile
    {
        public bool Collision { get; private set; }
        public int Id { get; private set; }
        public TileReserve(int xTilePos, int yTilePos, bool collision, int _id)
        {
            XTilePos = xTilePos;
            YTilePos = yTilePos;
            _width = 32;
            _height = 32;
            Collision = true;
            Id = _id;
        }
    }
}
