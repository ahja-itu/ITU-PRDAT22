import java.util.HashMap;
import java.util.Map;

public class Exercise_1_4 {
    public static void main(String[] args) {
        test_1_4_ii();
        test_1_4_iv();
    }

    public static void test_1_4_ii() {
        System.out.println("Exercise 1.4.ii - Printing expressions in abstract syntax:");

        Expr e1 = new Add(new CstI(17), new Var("z"));
        System.out.printf("e1:\n%s\n", e1);

        Expr e2 = new Mul(new CstI(2), new Sub(new Var("x"), new Add(new Var("w"), new Var("z"))));
        System.out.printf("e2:\n%s\n", e2);

        Expr e3 = new Add(new Var("x"), new Add(new Var("y"), new Add(new Var("z"), new Var("v"))));
        System.out.printf("e3:\n%s\n", e3);

        Expr e4 = new Mul(new Add(new Var("a"), new Var("b")), new Add(new Var("b"), new Var("a")));
        System.out.printf("e4:\n%s\n", e4);
    }

    public static void test_1_4_iv() {
        System.out.println("Exercise 1.4.iv - Simplifying expressions:");

        System.out.println("Addition:");
        printSimplification(new Add(new CstI(0), new CstI(5)));         // + 5
        printSimplification(new Add(new CstI(10), new CstI(0)));    // 10 + 0
        printSimplification(new Add(new CstI(3), new CstI(8)));     // 3 + 8
        printSimplification(new Add(new CstI(0), new Var("x")));    // 0 + x
        printSimplification(new Add(new Var("c"), new CstI(0)));    // c + 0
        printSimplification(new Add(new Var("x"), new Var("x")));   // x + x

        System.out.println("Multiplication:");
        printSimplification(new Mul(new CstI(0), new Var("y")));    // 0 * y
        printSimplification(new Mul(new CstI(1), new Var("y")));    // 1 * y
        printSimplification(new Mul(new CstI(2), new Var("y")));    // 2 * y
        printSimplification(new Mul(new Var("x"), new CstI(0)));    // x * 0
        printSimplification(new Mul(new Var("x"), new CstI(1)));    // x * 1
        printSimplification(new Mul(new Var("x"), new CstI(2)));    // x * 2
        printSimplification(new Mul(new Var("x"), new Var("y")));   // x * y
        
        System.out.println("Subtraction:");
        printSimplification(new Sub(new CstI(10), new CstI(0)));    // 10 - 0
        printSimplification(new Sub(new CstI(20), new CstI(20)));   // 20 - 20
        printSimplification(new Sub(new CstI(30), new CstI(20)));   // 30 - 20
        printSimplification(new Sub(new Var("x"), new CstI(0)));    // x - 0
        printSimplification(new Sub(new Var("x"), new Var("x")));   // x - x

        System.out.println("Mixed");
        printSimplification(new Add(new Sub(new CstI(1), new CstI(2)), new Mul(new CstI(3), new CstI(4))));  // (1 - 2) + (3 * 4)
        printSimplification(new Mul(new Var("x"), new Add(new CstI(2), new Mul(new CstI(1), new CstI(0))))); // x * (2 + (1 * 0))
    }
    
    private static void printSimplification(Expr e) {
        System.out.printf("%s -> %s\n", e, e.simplify());
    }
}

interface Expr {
    int eval(Map<String, Integer> env);

    default Expr simplify() {
        return this;
    }
}

class CstI implements Expr {
    private int value;

    public CstI(int value) {
        this.value = value;
    }
    
    public int eval(Map<String, Integer> env) {
        return this.value;
    }

    public String toString() {
        return Integer.toString(value);
    }
}

class Var implements Expr {
    private final String name;

    public Var(String name) {
        this.name = name;
    }

    @Override
    public int eval(Map<String, Integer> env) {
        var value = env.get(this.name);

        if (value == null) {
            throw new IllegalArgumentException("There were no bound variable with this name");
        }

        return value;
    }

    @Override
    public String toString() {
        return this.name;
    }
}

abstract class Binop implements Expr {
    protected final Expr a, b;
    protected final char operator;

    public Binop(Expr a, Expr b, char operator) {
        this.a = a;
        this.b = b;
        this.operator = operator;
    }

    abstract int binopFun(int x, int y);

    @Override
    public int eval(Map<String, Integer> env) {
        return binopFun(a.eval(env), b.eval(env));
    }

    @Override
    public String toString() {
        return String.format("(%s %c %s)", a, operator, b);
    }
}

class Add extends Binop {
    public Add(Expr a, Expr b) {
        super(a, b, '+');
    }

    @Override
    int binopFun(int x, int y) {
        return x + y;
    }

    @Override
    public Expr simplify() {
        Map<String, Integer> env = new HashMap<>();

        Expr _a = a.simplify();
        Expr _b = b.simplify();

        if (_a instanceof CstI && _b instanceof CstI) {
            return new CstI(_a.eval(env) + _b.eval(env));
        } else if (_a instanceof CstI) {
            if (_a.eval(env) == 0) {
                return _b;           // 0 + b = b
            }
        } else if (_b instanceof CstI) {
            if (_b.eval(env) == 0) {
                return _a;           // a + 0 = a
            }
            return _a;
        }

        return this;
    }
}

class Mul extends Binop {
    public Mul(Expr a, Expr b) {
        super(a, b, '*');
    }

    @Override
    int binopFun(int x, int y) {
        return x * y;
    }

    @Override
    public Expr simplify() {
        Map<String, Integer> env = new HashMap<>();
        
        Expr _a = a.simplify();
        Expr _b = b.simplify();
        
        if (_a instanceof CstI && _b instanceof CstI) {
            return new CstI(_a.eval(env) * _b.eval(env));
        } else if (_a instanceof CstI) {
            int x = _a.eval(env);
            
            if (x == 0) {
                return new CstI(0); // 0 * b = 0
            } else if (x == 1) {
                return _b;           // 1 * b = b
            }
        } else if (_b instanceof CstI) {
            int y = _b.eval(env);
            
            if (y == 0) {
                return new CstI(0); // a * 0 = 0
            } else if (y == 1) {
                return _a;           // a * 1 = a
            }
        }

        return this;
    }
}

class Sub extends Binop {
    public Sub(Expr a, Expr b) {
        super(a, b, '-');
    }

    @Override
    int binopFun(int x, int y) {
        return x - y;
    }

    @Override
    public Expr simplify() {
        var emptyEnv = new HashMap<String, Integer>();
        
        Expr _a = a.simplify();
        Expr _b = b.simplify();

        if (_b instanceof CstI) {

            // håndter case hvor b = 0
            int resB = _b.eval(emptyEnv);
            if (resB == 0) {
                return _a;
            }

            // håndter case hvor a = b
            if (_a instanceof CstI) {
                int resA = _a.eval(emptyEnv);
                int newVal = resA - resB;
                return new CstI(newVal);
            }
        } else if (_a instanceof Var && _b instanceof Var) {
            if (_a.toString().equals(_b.toString())) {
                return new CstI(0);
            }
        }
        return this;
    }
}
