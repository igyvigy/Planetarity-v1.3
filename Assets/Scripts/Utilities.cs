using Planetarity;
using UnityEngine;

public class Utilities
{
    public static Color ColorFromHex(string hex)
    {
        Color newCol = new Color(0.5f, 0.5f, 0.5f, 1f);
        ColorUtility.TryParseHtmlString(hex, out newCol);
        return newCol;
    }

    public static string GetRandomRocketName()
    {
        var rocketTypes = GameManager.i.rocketTypes;
        var index = UnityEngine.Random.Range(0, rocketTypes.Length);
        return rocketTypes[index].name;
    }

    public static RocketSO GetRocketTypeByName(string name)
    {
        var rocketTypes = GameManager.i.rocketTypes;
        RocketSO foundType = rocketTypes[0];
        foreach (RocketSO type in rocketTypes)
            if (type.name == name) foundType = type;
        return foundType;
    }

    public static Rect GetScreenRect(GameObject gameObject)
    {
        float depth = gameObject.transform.lossyScale.z;
        float width = gameObject.transform.lossyScale.x;
        float height = gameObject.transform.lossyScale.y;

        Vector3 lowerLeftPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x - width / 2, gameObject.transform.position.y - height / 2, gameObject.transform.position.z - depth / 2));
        Vector3 upperRightPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x + width / 2, gameObject.transform.position.y + height / 2, gameObject.transform.position.z - depth / 2));
        Vector3 upperLeftPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x - width / 2, gameObject.transform.position.y + height / 2, gameObject.transform.position.z - depth / 2));
        Vector3 lowerRightPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x + width / 2, gameObject.transform.position.y - height / 2, gameObject.transform.position.z - depth / 2));

        float xPixelDistance = Mathf.Abs(lowerLeftPoint.x - upperRightPoint.x);
        float yPixelDistance = Mathf.Abs(lowerLeftPoint.y - upperRightPoint.y);

        return new Rect(lowerLeftPoint.x, lowerLeftPoint.y, xPixelDistance, yPixelDistance);
    }
}
