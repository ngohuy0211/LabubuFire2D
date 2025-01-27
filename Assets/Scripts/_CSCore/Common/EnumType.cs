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

public enum AvatarType : int
{
    HERO = 0,
    SPECIAL = 1,
    FRAME = 2
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

public enum Faction : int
{
    UNKNOWN = 0,
}

public enum ItemType : int
{
    CONSUMABLE = 1,
    EQUIPMENT = 2,
    SHARD = 3
}

public enum GraphicSettingLevel : int
{
    QUALITY_VERY_LOW = 0,
    QUALITY_LOW = 1,
    QUALITY_MEDIUM = 2,
    QUALITY_HIGH = 3,
    QUALITY_VERY_HIGH = 4,
    QUALITY_ULTRA = 5,
}

public enum FormatNumberType : int
{
    NORMAL = 1,
    THREE_DOT = 2,
    K = 3,
    M = 4
}

public enum BonusItemKey : int
{
    ENERGY = 11,
    GOLD = 12,
    GEM = 13,
    RUBY = 14,
    USER_EXP = 17,
    EXP_VIP = 18,
}