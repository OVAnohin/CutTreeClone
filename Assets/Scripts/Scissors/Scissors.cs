using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ScissorsMover))]
public class Scissors : MonoBehaviour
{
    public int Coin { get; private set; }
    public event UnityAction<int> CoinChanged;

    private Animator _animator;
    private bool _isAnimationPlaying = false;
    private ScissorsMover _scissorsMover;

    private void Start()
    {
        Coin = 0;
        _animator = GetComponent<Animator>();
        _scissorsMover = GetComponent<ScissorsMover>();
    }

    public void ResetScissors()
    {
        _animator.Play("Idle");
        _scissorsMover.ResetPosition();
    }

    public void TryPlayAnimation()
    {
        if (_isAnimationPlaying == false)
            StartCoroutine(ActivateCutPlayAnimation());
    }

    public void AddCoin() 
    {
        Coin++;
        CoinChanged?.Invoke(Coin);
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
