using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.Grid;
using UnityEngine;

namespace _02_Scripts.Game.GameActors.Items.Consumables
{
    public interface IPosTargeted
    {
        bool Rooted { get; set; }
        void Activate(Unit selectedUnit, Tile[,] gridSystemTiles, int p2, int p3);
        List<IAttackableTarget> GetAllTargets(Unit selectedUnit, Tile[,] gridSystemTiles, int i, int i1, Vector2Int direction = default);
        int GetSize();
        SkillTargetArea TargetArea { get; set; }
        bool ConfirmPosition();
        List<Vector2Int> GetTargetPositions(int selectedSkillLevel, Vector2Int direction = default);
        void ShowPreview(Unit caster, int x, int y);
        void HidePreview(Unit caster);
        int GetHpCost();
        int GetHealingDone(Unit selectedUnit, Unit target);
        int GetDamageDone(Unit selectedUnit, Unit target);
        string GetName();
        EffectType GetEffectType();
    }
}