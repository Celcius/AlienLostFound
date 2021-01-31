using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

public enum ObjectColor
{
    Apple,
    Raspberry,
    Bubblegum,
    Lavender,
    Plum,
    Saphire,
    Azure,
    Sky,
    Aquamarine,
    Emerald,
    Forest,
    Lime,
    Lemon,
    Mustard,
    Apricot,
    Bronze,
    White,
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
                return "Tied-Up";

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
case ObjectColor.Apple:
                    return new Color(0.9f,0.23f,0.25f);
case ObjectColor.Raspberry:
                    return new Color(0.98f,0.38f,0.38f);
case ObjectColor.Bubblegum:
                    return new Color(0.96f,0.64f,0.9f);
case ObjectColor.Lavender:
                    return new Color(0.8f,0.59f,0.98f);
case ObjectColor.Plum:
                    return new Color(0.61f,0.41f,0.92f);
case ObjectColor.Saphire:
                    return new Color(0.32f,0.5f,0.93f);
case ObjectColor.Azure:
                    return new Color(0.22f,0.68f,0.96f);
case ObjectColor.Sky:
                    return new Color(0.4f,0.89f,0.99f);
case ObjectColor.Aquamarine:
                    return new Color(0.49f,1f,0.88f);
case ObjectColor.Emerald:
                    return new Color(0.15f,0.96f,0.53f);
case ObjectColor.Forest:
                    return new Color(0.53f,0.9f,0.3f);
case ObjectColor.Lime:
                    return new Color(0.77f,1f,0.56f);
case ObjectColor.Lemon :
                    return new Color(1f,1f,0.56f);
case ObjectColor.Mustard:
                    return new Color(0.99f,0.79f,0.33f);
case ObjectColor.Apricot:
                    return new Color(1f,0.62f,0.14f);
case ObjectColor.Bronze:
                    return new Color(0.81f,0.41f,0.32f);
case ObjectColor.White:
                    return new Color(1f,1f,1f);
            default:
                Debug.LogError("Not Supposed to fall here " + color);
                return Color.white;
        }
    }

    public static string ColorNameFromType(ObjectColor color)
    {
        switch (color)
        {
            case ObjectColor.Apple:
                            return "Apple";
            case ObjectColor.Raspberry:
                            return "Raspberry";
            case ObjectColor.Bubblegum:
                            return "Bubblegum";
            case ObjectColor.Lavender:
                            return "Lavender";
            case ObjectColor.Plum:
                            return "Plum";
            case ObjectColor.Saphire:
                            return "Saphire";
            case ObjectColor.Azure:
                            return "Azure";
            case ObjectColor.Sky:
                            return "Sky";
            case ObjectColor.Aquamarine:
                            return "Aquamarine";
            case ObjectColor.Emerald:
                            return "Emerald";
            case ObjectColor.Forest:
                            return "Forest";
            case ObjectColor.Lime:
                            return "Lime";
            case ObjectColor.Lemon :
                            return "Lemon";
            case ObjectColor.Mustard:
                            return "Mustard";
            case ObjectColor.Apricot:
                            return "Apricot";
            case ObjectColor.Bronze:
                            return "Bronze";
            case ObjectColor.White:
                            return "White";
            default:
                Debug.LogError("Not Supposed to fall here " + color);
                return "White";
        }
    }

    public static HashSet<ObjectColor> GetAllColors()
    {
        return UnityEngineUtils.GetEnumHash<ObjectColor>();
    }

    public static HashSet<ObjectShape> GetAllShapes()
    {
        return UnityEngineUtils.GetEnumHash<ObjectShape>();
    }
}

