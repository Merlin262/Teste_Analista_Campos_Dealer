using System;
using System.Collections.Generic;

public class MyBehavior {
    public MyBehavior(IEnumerable<string> items) {
        Console.WriteLine(""Constructor called!"");
    }
}

class Program {
    static void Main() {
        try {
            var arr = new string[0];
            Activator.CreateInstance(typeof(MyBehavior), arr);
        } catch(Exception e) {
            Console.WriteLine(""Exception: "" + e.Message);
        }
    }
}
