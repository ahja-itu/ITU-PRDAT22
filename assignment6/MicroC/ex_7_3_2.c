void squares(int n, int arr[]) {
  int i;

  for (i = 0; i < n; i = i + 1) {
    arr[i] = i * i;
    print arr[i];
  }
}

void main(int n) {
  int arr[20];

  squares(n, arr);

  println;
}