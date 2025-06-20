using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviors that can be mix-and-matched by Interactables for different situations (e.g. break, pressed, etc.).
/// </summary>
public abstract class Behavior
{
    protected Machine owner;

    [Header("Settings")]
    [SerializeField] protected float preDelay;
    [SerializeField] protected float timeout;
    [SerializeField] protected bool activateTimeout;

    private Coroutine executeRoutine;
    private Coroutine timeoutRoutine;

    public virtual void Init(Machine owner)
    {
        this.owner = owner;
    }

    public void Execute()
    {
        EndBehavior(); // Clean up any previous calls
        executeRoutine = owner.StartCoroutine(ExecuteRoutine());
    }

    private IEnumerator ExecuteRoutine()
    {
        yield return new WaitForSeconds(preDelay);

        StartBehavior();

        if (activateTimeout)
        {
            timeoutRoutine = owner.StartCoroutine(Timeout());
        }
    }

    private IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeout);

        if (executeRoutine != null)
        {
            owner.StopCoroutine(executeRoutine);
            executeRoutine = null;
        }

        EndBehavior();
    }

    // For any initialization that is needed specific to the behaviour. Force-setting properties should also be done here.
    public virtual void SetUp() { }

    // This should define the main chunk of the behavior.
    protected abstract void StartBehavior();

    protected void EndBehavior()
    {
        if (executeRoutine != null)
        {
            owner.StopCoroutine(executeRoutine);
            executeRoutine = null;
        }

        if (timeoutRoutine != null)
        {
            owner.StopCoroutine(timeoutRoutine);
            timeoutRoutine = null;
        }
    }
}

// ================================================================================================================ BEHAVIORS

[Serializable]
public class InteractableFall : Behavior
{
    [Space(10), SerializeField] private float gravityScale;

    public override void SetUp()
    {
        // set gravity to 0
    }

    protected override void StartBehavior()
    {
        // set gravity to gravityScale
    }
}

[Serializable]
public class InteractableEnableHitboxes : Behavior
{
    [Space(10), SerializeField] private List<InteractableHitbox> hitBoxes;

    protected override void StartBehavior()
    {
        foreach (var hitbox in hitBoxes)
        {
            // enable hitbox
        }
    }
}