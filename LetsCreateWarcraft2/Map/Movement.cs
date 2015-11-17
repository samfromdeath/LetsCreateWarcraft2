using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using LetsCreateWarcraft2.Common;
using LetsCreateWarcraft2.Manager;
using Microsoft.Xna.Framework;

namespace LetsCreateWarcraft2.Map
{
    class Movement
    {
        private Pathfinding _pathfinding;
        private Sprite _sprite;
        public bool _transitionOn;
        public List<Vector2> _path = new List<Vector2>();
        private int _speed;

        public int _goalX;
        public int _goalY;

        public Movement(int id, Sprite sprite, ManagerTiles managerTiles, ManagerUnits managerUnit)
        {
            _sprite = sprite;
            _speed = 4;
            _pathfinding = new Pathfinding(id, sprite, managerTiles, managerUnit);
        }

        public void Move(int xTile, int yTile)
        {
            if (_sprite.XTilePos == xTile && _sprite.YTilePos == yTile)
                return;
            _path = _pathfinding.FindPath(ref xTile, ref yTile);
            if(_path.Count > 0)
            {
                _goalX = (int)_path.Last().X;
                _goalY = (int)_path.Last().Y;
                _transitionOn = true;
            }
            else
            {
                _transitionOn = false;
            }
            
        }

        public void Update()
        {
            if (!_transitionOn)
                return;

            UpdateTransition();
        }

        public void NextStep()
        {
            if (_path.Count > 0)
                _path = _pathfinding.FindPath(ref _goalX, ref _goalY);
        }

        private void UpdateTransition()
        {
            if (_sprite.TransitionOn)
                return;

            if (_path.Count > 0)
            {
                var wantedXTile = (int)_path[0].X;
                var wantedYTile = (int)_path[0].Y;


                if (_sprite.XTilePos < wantedXTile)
                {
                    _sprite.Move(1, 0, _speed);
                }
                else if (_sprite.XTilePos > wantedXTile)
                {
                    _sprite.Move(-1, 0, _speed);
                }
                else if (_sprite.YTilePos < wantedYTile)
                {
                    _sprite.Move(0, 1, _speed);
                }
                else if (_sprite.YTilePos > wantedYTile)
                {
                    _sprite.Move(0, -1, _speed);
                }

                _path.RemoveAt(0);
            }
            else
            {
                _transitionOn = false;
            }
        }


    }
}
