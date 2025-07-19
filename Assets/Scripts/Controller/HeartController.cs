using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private IHeartModel _heartModel;
    private IHeartView _heartView;
    private Coroutine spikeRoutine;
    private ResetPosition resetPosition;

    private void OnEnable()
    {
        // Listen to spike and heart collision events
        SC_Death.OnSpikeCollision += StartSpikeDamage;
        SC_Heart.OnHeartCollision += OnHeartCollision;
    }

    private void OnDisable()
    {
        SC_Death.OnSpikeCollision -= StartSpikeDamage;
        SC_Heart.OnHeartCollision -= OnHeartCollision;
    }

    private void Awake()
    {
        _heartView = FindObjectOfType<HeartView>();
        resetPosition = GetComponent<ResetPosition>();
        _heartModel = new HeartModel();
        UpdateView();
    }

    private void OnHeartCollision()
    {
        _heartModel.AddHeart();
        UpdateView();
    }

    private void UpdateView()
    {
        if(_heartView != null && _heartModel != null)
            _heartView.UpdateHeartAmount(_heartModel.GetHeartAmount());
    }

    private void StartSpikeDamage()
    {
        // Prevent multiple routines running
        if (spikeRoutine == null)
            spikeRoutine = StartCoroutine(SpikeDamageRoutine());
    }

    private IEnumerator BlinkRed()
    {
        // Briefly change color to red
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    private IEnumerator SpikeDamageRoutine()
    {
        while (!_heartModel.IsDead())
        {
            _heartModel.RemoveHeart();
            UpdateView();

            if (spriteRenderer != null)
                yield return BlinkRed();

            if (_heartModel.IsDead())
                break;

            yield return new WaitForSeconds(1f);
        }

        if (resetPosition != null)
        {
            yield return BlinkRed();
            resetPosition.ResetPlayerPosition();
            yield return null;

            RespawnAll respawner = FindObjectOfType<RespawnAll>();
            if (respawner != null)
            {
                Debug.Log("called respawn!");
                respawner.Respawn();
            }
        }

        _heartModel.AddHeart();
        UpdateView();
        spikeRoutine = null;
    }
}
