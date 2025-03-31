using System;

public enum FactionType
{
    FIRE = 1,
    WATER = 2,
    WIND = 3,
    DARK = 4,
    LIGHT = 5,
}


public enum StatusType
{
    NONE = 999,
    LOCK = 0,
    INPROGRESS = 1,
    CAN_COLLECT = 2,
    DONE = 3,
}

public enum PoolTypeShow
{
    NONE = 0, // Hiện thẳng luôn
    STEP_BY_STEP = 1, // Hiện từng cái 1
    STEP_MIX_SCALE = 2, // Hiện từng cái và doscale
    STEP_MIX_FADE = 3, // Hiện từng cái và fade
    STEP_DIAGONAL = 4,
    //todo : sau này có thể bổ sung thêm theo dotween
}

public enum PopupShowOptions
{
    NULL = 0,
    TRANSITION = 1,
    SCALE = 2,
    FADE = 3,
    ROTATE = 4,
    COLOR = 5,
    SCALE_TRANSITION = 6,
    TRANS_SCALE_AND_FADE = 7,
    SCALE_AND_FADE = 8
}

public enum GuildRole : int
{
    LEADER = 3,
    CO_LEADER = 2,
    ELDER = 1,
    MEMBER = 0,
    NONE = -1
}

public enum ItemQuality : int
{
    SILVER = 1,
    GREEN = 2,
    BLUE = 3,
    PURPLE = 4,
    YELLOW = 5,
    ORANGE = 6,
    RED = 7,
    PINK = 8,
    SUPER = 9,
    NONE = 999,
}

public enum LoginPanelType : int
{
    LOGIN = 1,
    REGISTER = 2,
    FORGET = 3,
    INIT = 4,
}

public enum ItemDropType : int
{
    NONE = 0,
    PLUS = 1,
    MINUS = 2,
    BULLET = 3,
    SPEED_FIRE = 4,
    HEAL_HP = 5,
    ADD_COIN = 6
}