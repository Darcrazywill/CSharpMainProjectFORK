using log4net.Util;
using Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitBrains.Player;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;
using Model.Runtime;


public class ThirdUnitBrain : DefaultPlayerUnitBrain
{
    public override string TargetUnitName => "Ironclad Behemoth";

    private enum State { Moving, Attacking, Transition }
    private State currentState = State.Moving;
    private float transitionTime;
    private const float TransitionDuration = 1f;
    private const int MaxOfTargets = 3;

    private List<Vector2Int> _unreachableTargets = new List<Vector2Int>();
    private int _unitNumber;

    private void Update()
    {
        if (currentState == State.Transition)
        {
            transitionTime += Time.deltaTime;


            if (transitionTime >= TransitionDuration)// Завершаем переход на основе наличия целей
            {
                var targets = SelectTargets();
                currentState = targets.Count > 0 ? State.Attacking : State.Moving;
            }
            return;
        }


        var currentTargets = SelectTargets();

        if (currentState == State.Moving && currentTargets.Count > 0)
        {
            StartTransition();
        }
        else if (currentState == State.Attacking && currentTargets.Count == 0)
        {
            StartTransition();
        }

        if (currentState == State.Moving)
        {
            Move();
        }
        else if (currentState == State.Attacking)
        {
            Attack();
        }
    }

    private void StartTransition()
    {
        currentState = State.Transition;
        transitionTime = 0f;
    }

    private void Move()
    {
        Vector2Int nextStep = GetNextStep();
        Debug.Log($"Движение к: {nextStep}");
    }

    private void Attack()
    {
        var targets = SelectTargets();
        if (targets.Count > 0)
        {
            // Атакуем первую цель из списка
            Vector2Int target = targets[0];

        }
    }
    
}
