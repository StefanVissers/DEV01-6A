using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryPoint
{
#if WINDOWS || LINUX
  public static class Program
  {
        [STAThread]
    static void Main()
    {

        var fullscreen = false;
        read_input:
        switch (Microsoft.VisualBasic.Interaction.InputBox("Which assignment shall run next? (1, 2, 3, 4, or q for quit)", "Choose assignment", VirtualCity.GetInitialValue()))
        {
            case "1":
                using (var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen))
                    game.Run();
                break;
            case "2":
                using (var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen))
                    game.Run();
                break;
            case "3":
                using (var game = VirtualCity.RunAssignment3(FindRoute, fullscreen))
                    game.Run();
                break;
            case "4":
                using (var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen))
                    game.Run();
                break;
            case "q":
                return;
        }
        goto read_input;
    }

        static public double Distance(Vector2 v, Vector2 house)
        {
            return Math.Sqrt(Math.Pow((house.X - v.X), 2) + Math.Pow((house.Y - v.Y), 2));
        }


        static public void DoMerge(Vector2[] vectors, int left, int mid, int right, Vector2 house)
        {
            Vector2[] temp = new Vector2[vectors.Count()];
            int i, left_end, elements, tmp_pos;

            left_end = (mid - 1);
            tmp_pos = left;
            elements = (right - left + 1);

            while ((left <= left_end) && (mid <= right))
            {
                if (Distance(vectors[left], house) <= Distance(vectors[mid], house))
                    temp[tmp_pos++] = vectors[left++];
                else
                    temp[tmp_pos++] = vectors[mid++];
            }

            while (left <= left_end)
                temp[tmp_pos++] = vectors[left++];

            while (mid <= right)
                temp[tmp_pos++] = vectors[mid++];

            for (i = 0; i < elements; i++)
            {
                vectors[right] = temp[right];
                right--;
            }
        }

        static public void MergeSort_Recursive(Vector2[] vectors, int left, int right, Vector2 house)
        {
            int mid;

            if (right > left)
            {
                mid = (right + left) / 2;
                MergeSort_Recursive(vectors, left, mid, house);
                MergeSort_Recursive(vectors, (mid + 1), right, house);

                DoMerge(vectors, left, (mid + 1), right, house);
            }
        }

        private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
    {

            Vector2[] array = specialBuildings.ToArray();

            MergeSort_Recursive(array, 0, array.Count() - 1, house);

            return array;
    }

    private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(
      IEnumerable<Vector2> specialBuildings, 
      IEnumerable<Tuple<Vector2, float>> housesAndDistances)
    {
            Tree tree = new Tree(specialBuildings.ToArray());
            
            return tree.Traverse(tree, housesAndDistances).ToArray();

      
      /* return
          from h in housesAndDistances
          select
            from s in specialBuildings
            where Vector2.Distance(h.Item1, s) <= h.Item2
            select s; */
    }

    private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, 
      Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
      List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
      var prevRoad = startingRoad;
      for (int i = 0; i < 30; i++)
      {
        prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, destinationBuilding)).First());
        fakeBestPath.Add(prevRoad);
      }
      return fakeBestPath;
    }

    private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding, 
      IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
    {
      List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
      foreach (var d in destinationBuildings)
      {
        var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
        List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
        var prevRoad = startingRoad;
        for (int i = 0; i < 30; i++)
        {
          prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
          fakeBestPath.Add(prevRoad);
        }
        result.Add(fakeBestPath);
      }
      return result;
    }
  }
#endif
}
