using System;

public class MergeArrays
{
    public static void Main(String[] args)
    {
        Console.WriteLine("\nPress return to begin.");
        Console.In.Read();

        Console.WriteLine("Merging 100000 arrays\n");
        mergeNArrays(100000);
    }

    private static int[] mergeNArrays(int n)
    {
        Random rnd = new Random();
        int[] result = new int[1];

        for (int i = 0; i < n; i++)
        {
            result = mergeArrays(result, new int[i]);
        }

        return result;
    }

    private static int[] mergeArrays(int[] first, int[] second)
    {
        int[] res = new int[first.Length + second.Length];

        for (int i = 0; i < first.Length; i++)
        {
            res[i] = first[i];
        }

        for (int i = 0; i < second.Length; i++)
        {
            res[i + first.Length] = second[i];
        }

        return res;
    }
}
