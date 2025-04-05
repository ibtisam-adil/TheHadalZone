using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{

    private Transform WeaponHolder;
    public Vector3 aimDirection { get; private set; }
    private Camera mainCamera;

    private void Awake()
    {
        WeaponHolder = transform.Find("WeaponHolder");

        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleAiming();
    }

    private void HandleAiming()
    {
        // Get mouse position in world space
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure the Z value is set to zero to maintain 2D aiming

        // Calculate the direction from the aim position to the mouse position
        aimDirection = (mousePosition - WeaponHolder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        WeaponHolder.eulerAngles = new Vector3(0, 0, angle);

        // Flip the sprite based on the angle
        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.y = 1f;
        }
        WeaponHolder.localScale = aimLocalScale;
    }


}
