using UnityEngine;

namespace LeanTween.Examples.Scripts
{
	public class PathSpline2d : MonoBehaviour {

		public Transform[] cubes;

		public GameObject dude1;
		public GameObject dude2;

		private Framework.LTSpline visualizePath;

		void Start () {
			Vector3[] path = new Vector3[] {
				cubes[0].position,
				cubes[1].position,
				cubes[2].position,
				cubes[3].position,
				cubes[4].position
			};

			visualizePath = new Framework.LTSpline( path );
			// move
			Framework.LeanTween.moveSpline(dude1, path, 10f).setOrientToPath2d(true).setSpeed(2f);

			// move Local
			Framework.LeanTween.moveSplineLocal(dude2, path, 10f).setOrientToPath2d(true).setSpeed(2f);
		}

		void OnDrawGizmos(){
			Gizmos.color = Color.red;
			if(visualizePath!=null)
				visualizePath.gizmoDraw();
		}
	}
}
