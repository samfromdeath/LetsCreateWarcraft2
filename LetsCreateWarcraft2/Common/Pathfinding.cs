using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LetsCreateWarcraft2.Manager;
using LetsCreateWarcraft2.Map;
using Microsoft.Xna.Framework;

namespace LetsCreateWarcraft2.Common
{
    class Pathfinding
    {
        private List<Vector2> _path;
        private List<PathNode> _openList;
        private List<PathNode> _closedList;
        private ManagerTiles _managerTiles;
        private ManagerUnits _managerUnits;
        private int _xTilePos;
        private int _yTilePos;
        private int _goalX;
        private int _goalY;
        private int _deep;
        private Sprite _sprite;
        private bool _checkForUnit;
        private int _id;

        public int MainGoalX;
        public int MainGoalY;

        public Pathfinding(int id, Sprite sprite, ManagerTiles managerTiles, ManagerUnits managerUnits)
        {
            _id = id;
            _sprite = sprite;
            _managerTiles = managerTiles;
            _path = new List<Vector2>();
            _managerUnits = managerUnits;
            _checkForUnit = true;
        }

        public List<Vector2> FindPath(ref int xTilePos, ref int yTilePos) //, int _mainGoalX, int _mainGoalY)
        {
   //         this.MainGoalX = _mainGoalX;
//            this.MainGoalY = _mainGoalY;

            _path = new List<Vector2>();

            _xTilePos = _sprite.XTilePos;
            _yTilePos = _sprite.YTilePos;

            this._goalX = xTilePos;
            this._goalY = yTilePos;

            if (this._goalX == _xTilePos && this._goalY == _yTilePos && !_managerUnits.CheckCollision(_goalX, _goalY, _id, true))
            {                
                return new List<Vector2>();
            }

            _openList = new List<PathNode>();
            _closedList = new List<PathNode>();
            
            if (!CheckCollision(_goalX, _goalY, false) && _managerUnits.CheckCollision(_goalX, _goalY, _id, true))
            {
                // lets set new goal. around;
                int OutSide = 1;
                bool done = false;         
                while (!done)
                {
                    for(int i = -OutSide; i <= OutSide; i++)
                    {
                        if (!CheckCollision(_goalX + i, _goalY - OutSide, true))
                        {
                            this._goalX = _goalX + i;
                            this._goalY = _goalY - OutSide;
                            done = true;
                            break;
                        }
                        if (!CheckCollision(_goalX + i, _goalY + OutSide, true))
                        {
                            this._goalX = _goalX + i;
                            this._goalY = _goalY + OutSide;
                            done = true;
                            break;
                        }
                        if (!CheckCollision(_goalX + OutSide, _goalY + i, true))
                        {
                            this._goalX = _goalX + OutSide;
                            this._goalY = _goalY + i;
                            done = true;
                            break;
                        }
                        if (!CheckCollision(_goalX - OutSide, _goalY + i, true))
                        {
                            this._goalX = _goalX - OutSide;
                            this._goalY = _goalY + i;
                            done = true;
                            break;
                        }
                    }

                    //if (!CheckCollision(_goalX - OutSide, _goalY, true))
                    //{
                    //    this._goalX = _goalX - OutSide;
                    //    break;
                    //}

                    //if (!CheckCollision(_goalX, _goalY - OutSide, true))
                    //{
                    //    this._goalY = _goalY - OutSide;
                    //    break;
                    //}
                    //if (!CheckCollision(_goalX + OutSide, _goalY, true))
                    //{
                    //    this._goalX = _goalX + OutSide;
                    //    break;
                    //}

                    //if (!CheckCollision(_goalX, _goalY + OutSide, true))
                    //{
                    //    this._goalY = _goalY + OutSide;
                    //    break;
                    //}

                    //if (!CheckCollision(_goalX + OutSide, _goalY + OutSide, true))
                    //{
                    //    this._goalX = _goalX + OutSide;
                    //    this._goalY = _goalY + OutSide;
                    //    break;
                    //}

                    //if (!CheckCollision(_goalX - OutSide, _goalY + OutSide, true))
                    //{
                    //    this._goalX = _goalX - OutSide;
                    //    this._goalY = _goalY + OutSide;
                    //    break;
                    //}

                    //if (!CheckCollision(_goalX - OutSide, _goalY - OutSide, true))
                    //{
                    //    this._goalX = _goalX - OutSide;
                    //    this._goalY = _goalY - OutSide;
                    //    break;
                    //}

                    //if (!CheckCollision(_goalX + OutSide, _goalY - OutSide, true))
                    //{
                    //    this._goalX = _goalX + OutSide;
                    //    this._goalY = _goalY - OutSide;
                    //    break;
                    //}

                    OutSide++;
                }

                xTilePos = this._goalX;
                yTilePos = this._goalY;

                _openList.Clear();
                _openList.Clear();
            }

            _openList.Add(new PathNode(_xTilePos, _yTilePos, null, this._goalX, this._goalY));
            _deep = 0;
            CheckNodesClose(_openList[0], this._goalX, this._goalY);

            var node = _closedList[_closedList.Count - 1];
            while (node.Parent != null)
            {
                _path.Add(new Vector2(node.XTilePos, node.YTilePos));
                node = node.Parent;
            }

            _path.Reverse();

            return _path;
        }


        private void CheckNodesClose(PathNode node, int goalX, int goalY)
        {
            _deep++;

            if (_deep > 2500)
                return;

            _openList.Remove(node);
            _closedList.Add(node);

            if (node.XTilePos == goalX && node.YTilePos == goalY)
                return;

            CheckNode(node.XTilePos, node.YTilePos - 1, node, goalX, goalY);
            CheckNode(node.XTilePos, node.YTilePos + 1, node, goalX, goalY);
            CheckNode(node.XTilePos + 1, node.YTilePos, node, goalX, goalY);
            CheckNode(node.XTilePos - 1, node.YTilePos, node, goalX, goalY);
            
            //_openList = _openList.OrderBy(n => n.F).ToList(); 

            _openList.Sort(new PathNodeComparer());

            if (_openList.Count > 0)
                CheckNodesClose(_openList[0], goalX, goalY);
        }

        private void CheckNode(int xTilePos, int yTilePos, PathNode node, int goalX, int goalY)
        {
            if (!CheckCollision(xTilePos, yTilePos, false) &&
              !ClosedNode(xTilePos, yTilePos)) // Math.Abs(Math.Pow(goalX - xTilePos, 2) + Math.Pow(goalY - yTilePos, 2)) <= 1
            {
                var oldNode = _openList.FirstOrDefault(n => n.XTilePos == xTilePos && n.YTilePos == yTilePos);
                if (oldNode == null)
                {
                    _openList.Add(new PathNode(xTilePos, yTilePos, node, goalX, goalY));
                }
                else
                {
                    if (node.H + 10 <= oldNode.H)
                    {
                        oldNode.Parent = node;
                        oldNode.H = node.H + 10;
                    }
                }
            }            
        }

        private bool ClosedNode(int xTilePos, int yTilePos)
        {
            return _closedList.Any(p => p.XTilePos == xTilePos && p.YTilePos == yTilePos);
        }

        public bool CheckCollision(int x, int y, bool CheckUnits = false)
        {            
            return x < 0 || y < 0 || x >= 800 / 32 || y >= 480 / 32 || (CheckUnits && _managerUnits.CheckCollision(x, y, _id, CheckUnits)) || _managerTiles.CheckCollision(x, y);
        }

        private PathNode PickNextBest()
        {            
            var node = new PathNode(_goalX, _goalY, null, _xTilePos, _yTilePos);
            CheckNodesClose(node, _xTilePos, _yTilePos);
            //_openList.Reverse();            
            if (_openList.Count == 0)
                return null;
            return _openList[0];
        }


    }
}
