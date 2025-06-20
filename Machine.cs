using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Could be anything (enemy AI, interactable, etc.) that has a set of behaviors that must be executed based on state or events.
/// </summary>
public class Machine : MonoBehaviour
{
    [Header("Behaviors")]
    [SerializeField] protected BehaviorSet pressedBehaviorSet = new();
    [SerializeField] protected BehaviorSet breakBehaviorSet = new();
    protected List<BehaviorSet> behaviorSets = new();

    [Serializable]
    public class BehaviorSet
    {
        [SerializeField] private List<BehaviorHolder> BehaviorHolders;

        [Serializable]
        public class BehaviorHolder // this design has been adopted as SubclassSelector isn't compatible with lists
        {
            [SerializeReference, SubclassSelector] public Behavior Behavior;
        }

        public List<Behavior> Behaviors { get; private set; } = new();

        public void Init(Machine owner)
        {
            foreach (var holder in BehaviorHolders)
            {
                if (holder.Behavior != null)
                {
                    Behaviors.Add(holder.Behavior);
                }
            }

            foreach (var behavior in Behaviors)
            {
                behavior.Init(owner);
                behavior.SetUp();
            }
        }
    }

    // ================================================================================================================ INITIALIZATION

    private void Awake()
    {
        // Initialize behaviors
        ListUpBehaviorSets();

        foreach (var set in behaviorSets)
        {
            // Call Init() and SetUp() for each behavior on each set 
            set.Init(this);
        }

        Init();
    }

    protected virtual void ListUpBehaviorSets()
    {
        behaviorSets.Add(breakBehaviorSet);
    }

    protected virtual void Init() { }

    // ================================================================================================================ BEHAVIORS

    public void Pressed()
    {
        foreach (var behavior in pressedBehaviorSet.Behaviors)
        {
            behavior.Execute();
        }
    }

    public void Break()
    {
        foreach (var behavior in breakBehaviorSet.Behaviors)
        {
            behavior.Execute();
        }
    }
}

