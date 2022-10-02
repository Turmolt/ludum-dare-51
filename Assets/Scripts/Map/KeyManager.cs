using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [Serializable]
    public class KeyLockPair
    {
        public Vector3Int KeyPosition { get; }
        public Vector3Int LockPosition { get; }
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
}
