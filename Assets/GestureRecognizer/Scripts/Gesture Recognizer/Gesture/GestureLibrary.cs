using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TabTale;

namespace GestureRecognizer
{
	[Serializable, CreateAssetMenu(order = 900)]
	public class GestureLibrary : ScriptableObject
	{
		[SerializeField]
		public List<Gesture> Gestures = new List<Gesture>();

		public float gestureThreshold = 4f;

		public Result Recognize(Gesture gesture) {


			Result result = new Result();

			float distance = float.MaxValue;

			// Compare gesture against all others
			for (int i = 0; i < Gestures.Count; i++)
			{
				iterations = 0;
				distance = GreedyCloudMatch(gesture.NormalizedPoints, Gestures[i].NormalizedPoints);
				if (distance < result.Score) 
				{
					result.Set(Gestures[i].Name, distance);
				}
			}

			// Normalize score
			result.Score = Mathf.Max((2 - result.Score) / 2, 0f);



			return result;
		}

		public List<Result> RecognizeAll(Gesture gesture, List<string> shapesToFind, StringBuilder sb) {

			float distance = float.MaxValue;

			List<Result> sortedResults = new List<Result>();

			var gesturesToTest = Gestures.Where(g => shapesToFind.Contains(g.Name)).ToList();

			sb.AppendLine("testing: " + shapesToFind.ToCommaSeparatedString());



			// Compare gesture against all others
			for (int i = 0; i < gesturesToTest.Count; i++)
			{
				iterations = 0;
				sb.AppendLine("test shape " + gesturesToTest[i].Name + " points to compare: " + gesture.NormalizedPoints.Count());
				distance = GreedyCloudMatch(gesture.NormalizedPoints, gesturesToTest[i].NormalizedPoints);
				Result result = new Result();

				result.Set(gesturesToTest[i].Name, distance);
				result.OriginalScore = distance;

				#if UNITY_EDITOR
				float minScore = 4.5f;
				#else
				float minScore = 3f;
				#endif

				result.Score = Mathf.Max((minScore - result.Score) / minScore, 0f);

				if (result.Score > 0){

					if (shapesToFind != null && shapesToFind.Count > 0){
						if (shapesToFind.Contains(result.Name)){
							sb.AppendLine("found shape " + result.Name + " with score: " + result.OriginalScore);
							sortedResults.Add(result);	
						}
					}

					sortedResults.Add(result);	
				}else{
					sb.AppendLine("score for shape " + result.Name + " not close enough: " + result.OriginalScore);
				}			
			}

			sortedResults.Sort(CompareScores);

			sb.AppendLine("total iterations: " + iterations);

			return sortedResults;
		}

		int CompareScores(Result a, Result b){
			return a.OriginalScore.CompareTo(b.OriginalScore);
		}


		public void ResampleGestures()
		{
			List<Gesture> newGestures = new List<Gesture>();

			for (int i = 0; i < Gestures.Count; i++)
			{
				Gesture newGesture = new Gesture(Gestures[i].OriginalPoints, Gestures[i].Name);
				newGestures.Add(newGesture);
			}

			Gestures = newGestures;
			Debug.Log("Gestures in this library have been resampled");
		}


		private float GreedyCloudMatch(Point[] points1, Point[] points2) {

			float e = 0.5f;
			int step = Mathf.FloorToInt(Mathf.Pow(points1.Length, 1.0f - e));
			float minDistance = float.MaxValue;

			for (int i = 0; i < points1.Length; i += step) {
				
				float distance1 = CloudDistance(points1, points2, i);
				float distance2 = CloudDistance(points2, points1, i);
				minDistance = Mathf.Min(minDistance, distance1, distance2);
			}
			return minDistance;
		}

		public static int iterations = 0;

		private float CloudDistance(Point[] points1, Point[] points2, int startIndex) {
			bool[] matched = new bool[points1.Length];
			Array.Clear(matched, 0, points1.Length);

			float sum = 0;
			int i = startIndex;

			do {
				

				int index = -1;
				float minDistance = float.MaxValue;

				for (int j = 0; j < points1.Length; j++) {
					iterations++;
					if (!matched[j]) {
						float distance = Vector2.Distance(points1[i].Position, points2[j].Position);
						if (distance < minDistance) {
							minDistance = distance;
							index = j;
						}
					}
				}

				matched[index] = true;
				float weight = 1.0f - ((i - startIndex + points1.Length) % points1.Length) / (1.0f * points1.Length);
				sum += weight * minDistance;
				i = (i + 1) % points1.Length;

			} while (i != startIndex);

			return sum;
		}
	}
}