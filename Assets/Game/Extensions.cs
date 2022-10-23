using BrewedInk.CardCheat.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BrewedInk.CardCheat.Game
{
    public static class Extensions
    {

        public static Vector3 CellToLocal(this Grid grid, Vector2Int position)
        {
            return grid.CellToLocal(new Vector3Int(position.x, position.y));
        }
        public static Vector3 CellToWorld(this Grid grid, Vector2Int position)
        {
            return grid.CellToWorld(new Vector3Int(position.x, position.y));
        }
        
        
        public static Vector2 SizeToParent(this RawImage image, float padding = 0) {
            float w = 0, h = 0;
            var parent = image.GetComponentInParent<RectTransform>();
            var imageTransform = image.GetComponent<RectTransform>();
 
            // check if there is something to do
            if (image.texture != null) {
                if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;
                padding = 1 - padding;
                float ratio = image.texture.width / (float)image.texture.height;
                var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
                if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90) {
                    //Invert the bounds if the image is rotated
                    bounds.size = new Vector2(bounds.height, bounds.width);
                }
                //Size by height first
                h = bounds.height * padding;
                w = h * ratio;
                if (w > bounds.width * padding) { //If it doesn't fit, fallback to width;
                    w = bounds.width * padding;
                    h = w / ratio;
                }
            }
            imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            return imageTransform.sizeDelta;
        }
    }
}