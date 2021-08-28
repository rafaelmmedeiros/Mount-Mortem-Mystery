using RPG.Stats.Enums;
using System.Collections.Generic;

namespace RPG.Stats.Interfaces
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}