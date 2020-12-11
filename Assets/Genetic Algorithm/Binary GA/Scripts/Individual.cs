using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Individual
{
    static int RAND_MAX = 32767; // c++ random max

    int chromLength;
    int[] chromosome;
    double fitness;

    static int Flip(double prob)
    {
        return (Random.Range(0, RAND_MAX) < prob * RAND_MAX ? 1 : 0);
    }

    void Init(int chromLen)
    {
        chromosome = new int[chromLen];
        for (int i = 0; i < chromLength; i++)
        {
            chromosome[i] = Flip(0.5f);
        }
    }

    void Mutate(double pm)
    {
        for (int i = 0; i < chromLength; i++)
        {
            if (Flip(pm) > 0)
            {
                chromosome[i] = 1 - chromosome[i];
            }
        }
    }
}
