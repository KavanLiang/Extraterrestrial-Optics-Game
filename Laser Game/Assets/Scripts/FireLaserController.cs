﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireLaserController : MonoBehaviour
{
    public Laser[] toFire;
    public EnergyController ec;
    public Button bu;
    // Start is called before the first frame update

    public void fire() {
        ec.useEnergy();
        foreach(Laser laser in toFire) {
            laser.shoot();
        }
    }

    void Update() {
        bu.interactable = !Laser.isShooting() && ec.remainingEnergy() > 0;
    }
}