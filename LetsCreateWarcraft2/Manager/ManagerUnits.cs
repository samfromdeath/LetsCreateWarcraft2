using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsCreateWarcraft2.Common;
using LetsCreateWarcraft2.Map;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LetsCreateWarcraft2.Manager
{
    class ManagerUnits
    {
        private List<TeamUnits> _teams;

        public int GetCount()
        {
            int y = 0;
            _teams.ForEach(x => y += x.GetCount());
            return y;
        }

        public ManagerUnits(ManagerMouse managerMouse, ManagerTiles managerTiles)
        {
            _teams = new List<TeamUnits>();

            //For test
            var mapObjects = new List<MapObject>();
            for (int n = 0; n < 10; n++)
            {
                mapObjects.Add(new MapObject(managerMouse, new Sprite(2, 1 + n), managerTiles, this));
            }
            for (int n = 0; n < 10; n++)
            {
                mapObjects.Add(new MapObject(managerMouse, new Sprite(3, 1+ n), managerTiles, this));
            }
            for (int n = 0; n < 10; n++)
            {
                mapObjects.Add(new MapObject(managerMouse, new Sprite(4, 1 + n), managerTiles, this));
            }

            _teams.Add(new TeamUnits(mapObjects, "player", new List<string>()));
        }

        public void LoadContent(ContentManager content)
        {
            _teams.ForEach(t => t.LoadContent(content));
        }

        public void Update()
        {
            _teams.ForEach(t => t.Update());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _teams.ForEach(t => t.Draw(spriteBatch));
        }

        // is there a reserve!

        public bool CheckCollisionUnit(int x, int y, int id)
        {
            var team = _teams.FirstOrDefault(t => t.GetUnitAtSpace(x, y, id) != null);
            if (team == null)
                return false;
            var unit = team.GetUnitAtSpace(x, y, id);

            if (unit == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckCollisionReserves(int x, int y, int id)
        {
            var team = _teams.FirstOrDefault(t => t.GetReserveAtSpace(x, y, id) != null);
            if (team == null)
                return false;
            var unit = team.GetReserveAtSpace(x, y, id);

            if (unit == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckCollision(int x, int y, int id, bool CheckReserves = false)
        {
            if(!CheckCollisionUnit(x, y, id))
            {
                // is there reserves!!
                if(CheckReserves)
                {
                    return CheckCollisionReserves(x, y, id);
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
