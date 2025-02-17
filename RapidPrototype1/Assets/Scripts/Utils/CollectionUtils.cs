using System.Collections.Generic;
using UnityEngine;

public static class CollectionUtils
{
    public static T[] ReOrderArray<T>(T[] array, int target)
    {
        T value = array[target];

        for (int i = target; i >= 0; i--)
        {
            if (i == 0)
            {
                array[i] = value;
                continue;
            }
            array[i] = array[i - 1];
        }

        return array;
    }

    public static List<T> ReOrderList<T>(List<T> list, int target)
    {
        T value = list[target];

        for (int i = target; i >= 0; i--)
        {
            if (i == 0)
            {
                list[i] = value;
                continue;
            }
            list[i] = list[i - 1];
        }

        return list;
    }
}
