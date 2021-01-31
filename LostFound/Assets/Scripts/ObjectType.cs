using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectColor
{
    White,
    Yellow,
    Blue,
    Red,
    Green,
    Black,
    Brown,
    Azure,
    Ivory,
    Teal,
    Purple,
    NavyBlue,
    PeaGreen,
    Gray,
    Orange,
    Maroon,
    Charcoal,
    Aquamarine,
    Coral,
    Fuchsia,
    Wheat,
    Lime,
    Crimson,
    Khaki,
    Pink,
    HotPink,
    Magenta,
    Plum,
    Olive,
    Cyan
}

public enum ObjectShape
{
    Boxey,
    Bitey,
    Cilindrical,
    Circular,
    Fluffy,
    Grabbable,
    Grabby,
    Hard,
    Jingling,
    Listening,
    Rectangular,

    Spikey,
    Triangular,
    Tasting,
    TiedUp,
    Wet,
    Watching,
    Closed,
}

[System.Serializable]
public static class ObjectType
{
    public static string ShapeNameFromType(ObjectShape shape)
    {
        switch(shape)
        {
            case ObjectShape.Bitey:
                return "Bitey";

            case ObjectShape.Boxey:
                return "Boxey";

            case ObjectShape.Cilindrical:
                return "Cilindrical";

            case ObjectShape.Circular:
                return "Circular";

            case ObjectShape.Fluffy:
                return "Fluffy";

            case ObjectShape.Grabbable:
                return "Grabbable";
                
            case ObjectShape.Grabby:
                return "Grabby";

            case ObjectShape.Hard:
                return "Hard";

            case ObjectShape.Jingling:
                return "Jingling";

            case ObjectShape.Listening:
                return "Listening";

            case ObjectShape.Rectangular:
                return "Rectangular";

            case ObjectShape.Spikey:
                return "Spikey";

            case ObjectShape.Tasting:
                return "Tasting";
        
            case ObjectShape.Triangular:
                return "Triangular";

            case ObjectShape.TiedUp:
                return "TiedUp";

            case ObjectShape.Wet:
                return "Wet";

            case ObjectShape.Watching:
                return "Watching";

            case ObjectShape.Closed:
                return "Closed";

            default:
                Debug.LogError("Not Supposed to fall here " + shape);
                return "CENSORED";
        }
    }

    public static Color ColorFromType(ObjectColor color)
    {
        switch (color)
        {
                case ObjectColor.White:
                    return Color.white;
                case ObjectColor.Yellow:
                    return Color.yellow;
                case ObjectColor.Blue:
                    return Color.blue;
                case ObjectColor.Red:
                    return Color.red;
                case ObjectColor.Green:
                    return Color.green;
                case ObjectColor.Black:
                    return Color.black;
                case ObjectColor.Brown:
                    return new Color(165/255.0f,42/255.0f,42/255.0f);
                case ObjectColor.Azure:
                    return new Color(0/255.0f, 127/255.0f, 255/255.0f);
                case ObjectColor.Ivory:
                    return new Color(255/255.0f,255/255.0f,240/255.0f);
                case ObjectColor.Teal:
                    return new Color(0/255.0f,128/255.0f,128/255.0f);
                case ObjectColor.Purple:
                    return new Color(128/255.0f,0/255.0f,128/255.0f);
                case ObjectColor.NavyBlue:
                    return new Color(0/255.0f,0/255.0f,128/255.0f);
                case ObjectColor.PeaGreen:
                    return new Color(115/255.0f,145/255.0f,34/255.0f);
                case ObjectColor.Gray:
                    return Color.gray;
                case ObjectColor.Orange:
                    return new Color(255/255.0f,165/255.0f,0/255.0f);
                case ObjectColor.Maroon:
                    return new Color(128/255.0f,0/255.0f,0/255.0f);
                case ObjectColor.Charcoal:
                    return new Color(54/255.0f, 69/255.0f, 79/255.0f);
                case ObjectColor.Aquamarine:
                    return new Color(127/255.0f, 255/255.0f, 21/255.0f);
                case ObjectColor.Coral:
                    return new Color(255/255.0f,127/255.0f,80/255.0f);
                case ObjectColor.Fuchsia:
                    return new Color(255/255.0f,0/255.0f,255/255.0f);
                case ObjectColor.Wheat:
                    return new Color(245/255.0f, 222/255.0f, 179/255.0f);
                case ObjectColor.Lime:
                    return new Color(191/255.0f, 255/255.0f, 0/255.0f);
                case ObjectColor.Crimson:
                    return new Color(220/255.0f, 20/255.0f, 60/255.0f);
                case ObjectColor.Khaki:
                    return new Color(76/255.0f, 69/255.0f, 56/255.0f);
                case ObjectColor.Pink:
                    return new Color(219/255.0f,112/255.0f,147/255.0f);
                case ObjectColor.HotPink:
                    return new Color(255/255.0f,105/255.0f,180/255.0f);
                case ObjectColor.Magenta:
                    return new Color(255/255.0f,0/255.0f,255/255.0f);
                case ObjectColor.Plum:
                    return new Color(221/255.0f,160/255.0f,221/255.0f);
                case ObjectColor.Olive:
                    return new Color(128/255.0f,128/255.0f,0/255.0f);
                case ObjectColor.Cyan:
                    return new Color(0/255.0f,255/255.0f,255/255.0f);
            default:
                Debug.LogError("Not Supposed to fall here " + color);
                return Color.white;
        }
    }

    public static string ColorNameFromType(ObjectColor color)
    {
        switch (color)
        {
                case ObjectColor.White:
                    return "White";
                case ObjectColor.Yellow:
                    return "Yellow";
                case ObjectColor.Blue:
                    return "Blue";
                case ObjectColor.Red:
                    return "Red";
                case ObjectColor.Green:
                    return "Green";
                case ObjectColor.Black:
                    return "Black";
                case ObjectColor.Brown:
                    return "Brown";
                case ObjectColor.Azure:
                    return "Azure";
                case ObjectColor.Ivory:
                    return "Ivory";
                case ObjectColor.Teal:
                    return "Teal";
                case ObjectColor.Purple:
                    return "Purple";
                case ObjectColor.NavyBlue:
                    return "Navy Blue";
                case ObjectColor.PeaGreen:
                    return "Pea Green";
                case ObjectColor.Gray:
                    return "Gray";
                case ObjectColor.Orange:
                    return "Orange";
                case ObjectColor.Maroon:
                    return "Maroon";
                case ObjectColor.Charcoal:
                    return "Charcoal";
                case ObjectColor.Aquamarine:
                    return "Aquamarine";
                case ObjectColor.Coral:
                    return "Coral";
                case ObjectColor.Fuchsia:
                    return "Fuchsia";
                case ObjectColor.Wheat:
                    return "Wheat";
                case ObjectColor.Lime:
                    return "Lime";
                case ObjectColor.Crimson:
                    return "Crimson";
                case ObjectColor.Khaki:
                    return "Khaki";
                case ObjectColor.Pink:
                    return "Pink";
                case ObjectColor.HotPink:
                    return "HotPink";
                case ObjectColor.Magenta:
                    return "Magenta";
                case ObjectColor.Plum:
                    return "Plum";
                case ObjectColor.Olive:
                    return "Olive";
                case ObjectColor.Cyan:
                    return "Cyan";
            default:
                Debug.LogError("Not Supposed to fall here " + color);
                return "White";
        }
    }
}

