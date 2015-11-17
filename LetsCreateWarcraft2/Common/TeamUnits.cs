using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsCreateWarcraft2.Map;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LetsCreateWarcraft2.Common
{
    class TeamUnits
    {
        private List<MapObject> _units;
        public string TeamName { get; private set; }
        public List<string> Allies { get; private set; } 

        public int GetCount()
        {
            return _units.Count;
        }

        public TeamUnits(List<MapObject> units, string teamName, List<string> allies)
        {
            _units = units;
            TeamName = teamName;
            Allies = allies; 
        }

        public MapObject GetUnitAtSpace(int x, int y, int id)
        {
            return _units.FirstOrDefault(u => u.Id != id && !u._movement._transitionOn && u.AtSpace().X == x && u.AtSpace().Y == y); 
        }

        public MapObject GetReserveAtSpace(int x, int y, int id)
        {
            return _units.FirstOrDefault(u => u.Id != id && u._movement._transitionOn && u._movement._goalX == x && u._movement._goalY == y);
        }

        public void LoadContent(ContentManager content)
        {
            _units.ForEach(u => u.LoadContent(content));
        }

        public void Update()
        {
            _units.ForEach(u => u.Update());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _units.ForEach(u => u.Draw(spriteBatch));
        }


    }
}
