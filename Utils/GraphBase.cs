using System.Collections.Generic;

namespace AdventOfCode.Utils
{
    public class GraphBase<T>
    {
        private bool _isDirected = false;
        protected IDictionary<T, List<Edge>> _adj = new Dictionary<T, List<Edge>>();

        public GraphBase(bool isDirected = false) => _isDirected = isDirected;

        public void AddEdge(T u, T v, int w = 1)
        {
            if (!_adj.ContainsKey(u))
            {
                _adj[u] = new List<Edge>();
            }
            _adj[u].Add(new Edge(v, w));

            if (!_isDirected)
            {
                if (!_adj.ContainsKey(v))
                {
                    _adj[v] = new List<Edge>();
                }
                _adj[v].Add(new Edge(u, w));
            }
        }

        protected class Edge
        {
            public T Vertex { get; private set; }
            public int Weight { get; private set; }

            public Edge(T v, int w) => (Vertex, Weight) = (v, w);
        }
    }
}
