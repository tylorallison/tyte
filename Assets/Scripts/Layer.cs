using System;
using UnityEngine;

namespace TyTe {

    [Serializable]
    public class Layer {
        public string name;
        public int width;
        public int height;
        public int[] grid;

        public Layer(
            string name,
            int width,
            int height
        ) {
            this.name = name;
            this.width = width;
            this.height = height;
            this.grid = new int[width*height];

        }

        int Idx(
            int x,
            int y 
        ) {
            x = Mathf.Clamp(x, 0, width-1);
            y = Mathf.Clamp(y, 0, height-1);
            return x % width + width * y;
        }

        public int Get(
            int x,
            int y
        ) {
            return grid[Idx(x,y)];
        }

        public void Set(
            int x,
            int y,
            int id
        ) {
            grid[Idx(x,y)] = id;
        }

    }

}