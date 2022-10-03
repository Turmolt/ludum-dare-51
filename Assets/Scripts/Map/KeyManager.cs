using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeyManager : MonoBehaviour
{
    [Serializable]
    public class KeyLockPair
    {
        public Vector3Int KeyPosition;


        public Vector3Int LockPosition;
    }

    [SerializeField]private KeyLockPair[] keyLocks;

    private Dictionary<Vector3Int, Vector3Int> _keyLockDictionary = new();

    void Start()
    {
        ParseKeys();
    }

    private void ParseKeys()
    {
        foreach (var pair in keyLocks)
        {
            _keyLockDictionary.Add(pair.KeyPosition, pair.LockPosition);
        }
    }

    public Vector3Int? GetLockPosition(Vector3Int keyPosition) => _keyLockDictionary.TryGetValue(keyPosition, out var value) ? value : null;

    private void OnDrawGizmosSelected()
    {
        foreach (var pair in keyLocks)
        {
            
            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            Gizmos.DrawSphere(pair.KeyPosition + Vector3.one * 0.5f, 0.25f);
            
            
            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.DrawSphere(pair.LockPosition+ Vector3.one * 0.5f, 0.25f);
        }
    }
}
