using System.Collections.Generic;

public static class CommonUtils {

    public static IEnumerable<T> CreatePath<T>(Dictionary<T, T> parents, T goal) {
        var path    = new Stack<T>();
        var current = goal;

        path.Push(goal);
        while (parents.ContainsKey(current)) {
            path.Push(parents[current]);
            current = parents[current];
        }

        return path;
    }

}

