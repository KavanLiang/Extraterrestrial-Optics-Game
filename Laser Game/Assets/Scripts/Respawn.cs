using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject enemy;
    public LevelManager levelManager;
    public static Respawn Instance {
        get;
        private set;
    }

    void Awake() {
        if(Instance == null) {
            Instance = this;
            Instance.enemy.GetComponent<AlienController>().lm = levelManager;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }
    public static void RespawnEnemy(GameObject obj) {
        Vector3 pos = obj.transform.position;
        Quaternion rot = obj.transform.rotation;
        Instance.StartCoroutine(Instance.REHelper(obj));
    }

    IEnumerator REHelper(GameObject obj) {
        Vector3 pos = obj.transform.position;
        Quaternion rot = obj.transform.rotation;
        while(Laser.isShooting()) {
            yield return new WaitForSeconds(1.5f);
        }
        Instantiate(Instance.enemy, pos, rot);
    }
}
