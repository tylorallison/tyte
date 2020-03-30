using System;
using System.Collections.Generic;

namespace TyTe {

    [Serializable]
    public class Zone {
        public const int maxWidth = 128;
        public const int maxHeight = 128;
        public string name = "zone";
        public int width = 50;
        public int height = 50;
        public Layer[] layers = new Layer[0];

        public Zone(
            string name,
            int width,
            int height
        ) {
            this.name = name;
            this.width = width;
            this.height = height;
        }

        public bool IsValid() {
            if (name == "") return false;
            if (width < 0 || width > maxWidth) return false;
            if (height < 0 || height > maxHeight) return false;
            return true;
        }

        public void AddLayer(
            Layer layer
        ) {
            var idx = layers.Length;
            Array.Resize(ref layers, layers.Length+1);
            layers[idx] = layer;
        }

        public void Raise(
            Layer layer
        ) {
            var idx = Array.IndexOf(layers, layer);
            if (idx > 0) {
                var l = new List<Layer>(layers);
                l.Remove(layer);
                l.Insert(idx-1,layer);
                layers = l.ToArray();
            }
        }

        public void Lower(
            Layer layer
        ) {
            var idx = Array.IndexOf(layers, layer);
            if (idx < layers.Length-1) {
                var l = new List<Layer>(layers);
                l.Remove(layer);
                l.Insert(idx+1,layer);
                layers = l.ToArray();
            }
        }

        public void RemoveLayer(
            Layer layer
        ) {
            var l = new List<Layer>(layers);
            l.Remove(layer);
            layers = l.ToArray();
        }

    }
}