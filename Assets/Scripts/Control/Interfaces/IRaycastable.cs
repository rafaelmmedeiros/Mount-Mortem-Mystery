using RPG.Control.Enums;

namespace RPG.Control.Interfaces
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(PlayerController callingController);
    }
}
