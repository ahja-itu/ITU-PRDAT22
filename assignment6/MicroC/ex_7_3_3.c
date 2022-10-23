void histogram(int n, int ns[], int max, int freq[]) {
  int i;

  for (i = 0; i < max; i = i + 1) {
    freq[i] = 0;
  }
  for (i = 0; i < n; i = i + 1) {
    freq[ns[i]] = freq[ns[i]] + 1;
  }
}

void main(int n) {
  int ns[7];
  ns[0] = 7;
  ns[1] = 13;
  ns[2] = 9;
  ns[3] = 8;
  ns[4] = 7;
  ns[5] = 9;
  ns[6] = 9;

  int freq[20];
  histogram(n, ns, 20, freq);

  int i;
  
  for (i = 0; i < 20; i = i + 1) {
    print freq[i];
  }

  println;
}