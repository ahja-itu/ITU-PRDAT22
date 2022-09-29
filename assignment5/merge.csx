static int[] merge(int[] xs, int[] ys) {
    int[] zs = new int[xs.Length + ys.Length];

    int i = 0;
    int j = 0;

    while (i < xs.Length || j < ys.Length) {
        if (i < xs.Length && j < ys.Length) {
            if (xs[i] <= ys[j]) {
                zs[i + j] = xs[i];
                i++;
            } else {
                zs[i + j] = ys[j];
                j++;
            }
        } else if (i < xs.Length) {
            zs[i + j] = xs[i];
            i++;
        } else {
            zs[i + j] = ys[j];
            j++;
        }
    }

    return zs;
}

var arr = merge(new int[] {3, 5, 12}, new int[] {2, 3, 4, 7});
Console.WriteLine($"[{string.Join(", ", arr)}]");

