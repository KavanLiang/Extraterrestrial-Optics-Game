using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireController : MonoBehaviour
{
    private float hp;
    // Start is called before the first frame update
    void Start()
    {
      hp = 100;
    }

    void decrementHp(float dmg) {
      hp -= dmg;
    }

    // Update is called once per frame
    void Update()
    {
      if(hp <= 0) {
        Die();
      }
    }

    void Die() {
      //NOTIMPLEMENTEDYET
    }
}
