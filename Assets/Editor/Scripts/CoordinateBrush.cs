using UnityEngine;

namespace UnityEditor.Tilemaps
{
    [CustomGridBrush(true, true, true, "Coordinate Brush")]
    public class CoordinateBrush : GridBrush
    {
    }

    [CustomEditor(typeof(CoordinateBrush))]
    public class CoordinateBrushEditor : GridBrushEditor
    {
        private int storeX;
        private int storeY;

        public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
        {
            base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);


            if (position.size.x == 1 && position.size.y == 1)
            {
                storeX = position.x;
                storeY = position.y;
            }


            var labelText = "Pos: " + position.position;
            if (position.size.x > 1 || position.size.y > 1)
            {
                AdjustPosInGridFilledBoxForRightOrUpDirection(ref labelText, in position);

                labelText += " size: " + position.size;
            }


            Handles.Label(grid.CellToWorld(position.position), labelText);
        }



        private void AdjustPosInGridFilledBoxForRightOrUpDirection(ref string label, in BoundsInt position)
        {
            if (storeX == position.x)
                label = "Pos: (" + (position.max.x - 1) + ", ";

            else
                label = "Pos: (" + position.x + ", ";


            if (storeY == position.y)
                label += (position.max.y - 1) + ", " + position.z + ")";

            else
                label += (position.y) + ", " + position.z + ")";
        }
    }



}