
///////////////////////////////////////////////////////////////
/// Los GameActions son las etiquetas que vamos a usar para identificar el input del jugador como accion, esto es 
/// independiente del input por hardware, intenté ignorar los nombres de los botones o teclas para que sea génerico
///////////////////////////////////////////////////////////////
public enum GameActions
{
    Right_Hand,         // Sword, R_Tool
    Left_Hand,          // Shield, L_Tool
    Move_Modifier,      // Dash, Teleport, Roll, etc
    Interact,           // Interact, Escape, Enter, Talk
    LSkill_Primary,     // SwordSkill, OtherRHandSkill
    LSkill_Secondary,   // ChangeRSkill
    RSkill_Primary,     // ShieldSkill, OtherLHandSkill
    RSkill_Secondary,   // ChangeLSkill
    HorizontalMove,     // HorizontalMove, HorizontalMenuCursor
    VerticalMove,       // VerticalMove, VerticalMenuCursor
    HorizontalLook,     // Horizontal_point_view, Horizontal_Skill_Direccion
    VerticalLook,       // Vertical_point_view, Vertical_Skill_Direccion
    Game_Start,         // GameMenu
    Game_Back,          // Inventory, Map, Skills

    // hasta acá son las que tenemos, el resto por ahi se pueden usar para uso cosmético
    // o tal vez para seleccionar opciones rapidas o para algo puse esto simplemente xq me sobraban botones en el joystick
    // (Options => DPADS) (Extras => SticksCenterButtons)

    Option_1,
    Option_2,
    Option_3,
    Option_4,
    Extra_1,
    Extra_2
}
public enum InputEventAction { Up, Down, Stay, Axis }

public static class UnityJoystickInputNames
{
    public const string BUTTON_A = "xbox_button_A";
    public const string BUTTON_B = "xbox_button_B";
    public const string BUTTON_X = "xbox_button_X";
    public const string BUTTON_Y = "xbox_button_Y";
    public const string BUTTON_LB = "xbox_button_LB";
    public const string BUTTON_RB = "xbox_button_RB";
    public const string BUTTON_BACK = "xbox_button_BACK";
    public const string BUTTON_START = "xbox_button_START";
    public const string BUTTON_LEFT_STICKCENTER = "xbox_button_LEFT_STICKCENTER";
    public const string BUTTON_RIGHT_STICKCENTER = "xbox_button_RIGHT_STICKCENTER";
    public const string AXIS_LEFT_HORIZONTAL = "xbox_axis_LEFT_HORIZONTAL";
    public const string AXIS_LEFT_VERTICAL = "xbox_axis_LEFT_VERTICAL";
    public const string AXIS_RIGHT_HORIZONTAL = "xbox_axis_RIGHT_HORIZONTAL";
    public const string AXIS_RIGHT_VERTICAL = "xbox_axis_RIGHT_VERTICAL";
    public const string AXIS_TRIGGERS = "xbox_axis_TRIGGERS";
    public const string AXIS_DPAD_HORIZONTAL = "xbox_axis_DPAD_HORIZONTAL";
    public const string AXIS_DPAD_VERTICAL = "xbox_axis_DPAD_VERTICAL";
}