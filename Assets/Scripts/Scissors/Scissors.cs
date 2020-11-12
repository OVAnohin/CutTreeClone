using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ScissorsMover))]
public class Scissors : MonoBehaviour
{
    private Animator _animator;
    private bool _isAnimationPlaying = false;
    private ScissorsMover _scissorsMover;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _scissorsMover = GetComponent<ScissorsMover>();
    }

    public void ResetScissors()
    {
        _scissorsMover.MoveToStartPosition();
    }

    public void TryPlay()
    {
        if (_isAnimationPlaying == false)
            StartCoroutine(ActivateCutPlayAnimation());
    }

    private IEnumerator ActivateCutPlayAnimation()
    {
        _animator.Play("Cut");
        _isAnimationPlaying = true;

        yield return new WaitForSeconds(1f);

        _isAnimationPlaying = false;
        _animator.Play("Idle");
    }
}
