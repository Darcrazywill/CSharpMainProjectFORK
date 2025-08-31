using Model;
using Model.Runtime.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;
using Model.Runtime;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 4f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;
        private int _unitNumber;
        private const int MaxOfTargets = 3;
        private static int unitCounter = 0;

        private List<Vector2Int> _unreachableTargets = new List<Vector2Int>();
        public SecondUnitBrain()
        {
            _unitNumber = unitCounter++;
           
        }

        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            ///////////////////////////////////////
            // Homework 1.3 (1st block, 3rd module)
            ///////////////////////////////////////  
            ///

            int currentTempreture = (int)GetTemperature();
            if (currentTempreture>=overheatTemperature)
            {
                return;
            }

            int projectileINT = currentTempreture;

            for (int i = 0; i < projectileINT; i++)
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);

            }
            IncreaseTemperature();


            ///////////////////////////////////////
        }
        public override Vector2Int GetNextStep()
        {
            if (_unreachableTargets.Count>0)
            {
                Vector2Int target = _unreachableTargets[0];
                return unit.Pos.CalcNextStepTowards(target);
            }
            return unit.Pos;
        }
        protected override List<Vector2Int> SelectTargets()
        {
            //////////////////////////////////////////////
            /////////////////////////////////////////////
            _unreachableTargets.Clear();
            List<Vector2Int> result = new List<Vector2Int>();
            List<Vector2Int> allTargets = GetAllTargets().ToList();
            
            if (!allTargets.Any()) 
            {
                int enemyPlayerId = IsPlayerUnitBrain ? RuntimeModel.BotPlayerId : RuntimeModel.PlayerId;
                Vector2Int enemyBase = runtimeModel.RoMap.Bases[enemyPlayerId];
                result.Add(enemyBase);
                return result;
            }
            SortByDistanceToOwnBase(allTargets);
            List<Vector2Int> limitedTargets = allTargets.Take(MaxOfTargets).ToList();
            int targetIndex = _unitNumber % limitedTargets.Count;

            // Получаем выбранную цель
            Vector2Int selectedTarget = allTargets[targetIndex];

            // Проверяем, находится ли цель в радиусе атаки
            if (IsTargetInRange(selectedTarget))
            {
                result.Add(selectedTarget);
            }
            else
            {
                _unreachableTargets.Add(selectedTarget);
            }
            return result;
        }
            ///////////////////////////////////////
            //////////////////////////////////////
        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if (_overheated) return (int)OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}