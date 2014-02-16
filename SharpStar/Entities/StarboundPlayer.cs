﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpStar.DataTypes;

namespace SharpStar.Entities
{
    public class StarboundPlayer
    {

        public string Name { get; private set; }

        public string UUID { get; set; }

        public PlanetCoordinate Coordinates { get; set; }

        public bool OnShip { get; set; }

        public StarboundPlayer(string name, string uuid)
        {
            Name = name;
            UUID = uuid;
        }

    }
}
