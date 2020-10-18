using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Scissors : MonoBehaviour
{
    private Animator _animator;
    private bool _isAnimationPlaying = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void TryCutAnimationPlay()
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
