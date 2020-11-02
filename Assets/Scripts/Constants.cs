
using UnityEngine;

public class Constants
{
    #region Strings
    public const string k_SunName = "Sun";
    #endregion


    #region Floats
    public const float k_SunSize = 7f;
    public const float k_SunLightRange = 140f;
    public const float k_SunMass = 40000f;
    public const float k_GraviConst = .02f;
    public const float k_SunLightIntencity = 2f;
    public const float k_MinPlanetSize = 1f;
    public const float k_MaxPlanetSize = 5f;
    public const float k_MinPlanetOrbitSpeed = 1f;
    public const float k_MaxPlanetOrbitSpeed = 10f;
    public const float k_MinPlanetDensity = 100f;
    public const float k_MaxPlanetDensity = 1500f;
    public const float k_InterPlanetDistance = 14f;
    public const float k_ArtileryMoveSpeed = 60f;
    public const float k_ShieldMoveSpeed = 100f;
    public const float k_ShieldHealth = 200000f;

    #endregion


    #region Colors
    public static Color k_WhiteColor = Utilities.ColorFromHex("#ffffff");
    public static Color k_SunColor = Utilities.ColorFromHex("#f5a511");
    #endregion

}