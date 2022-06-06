using System.Collections;
using System.Collections.Generic;


public static class Utility
{
    // Shuffling for obsticle placment using the Fisher-Yates Shuffle
    // in Fisher-Yates the last element swap is just the last item getting swapped
    // with itself so thats why im skipping that one with array.length - 1
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random pseudorandomgenerator = new System.Random(seed);
        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = pseudorandomgenerator.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }
}
