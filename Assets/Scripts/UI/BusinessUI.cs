using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessUI : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private BusinessSO businessSo;
    
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer floor_1;
    [SerializeField] private SpriteRenderer floor_2;
    [SerializeField] private SpriteRenderer roof;


    private void Start()
    {
        LoadBusinessInformation();
    }

    private void LoadBusinessInformation()
    {
        floor_1.sprite = businessSo.floor_1;
        floor_2.sprite = businessSo.floor_2;
        roof.sprite = businessSo.roof;
    }
    
}
