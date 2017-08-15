using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Items;


namespace AssemblyCSharp
{
	public class ItemDrop : MonoBehaviour
	{
		public Item item;
		public int x;
		public int z;
		public GameObject gameObject;
		public const float HOVER_TIME=1.0f;
		bool selected=false;

		public ItemDrop (Item i, int x, int z)
		{
			this.item = i;
			this.x = x;
			this.z = z;
			MainScript.fieldHoveredEvent += Hovered;
			InstantiateItemDrop();
		}
		private void InstantiateItemDrop(){
			int rngx = Random.Range (-5, 5);
			int rngz = Random.Range (-5, 5);
			GameObject parentObject = Instantiate (new GameObject (),new Vector3(x+0.5f+rngx/10.0f,GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.GetHeight(x,z)+0.1f,z+0.5f+rngz/10.0f), Quaternion.AngleAxis(90, new Vector3(1,0,0)))as GameObject;
			parentObject.transform.localScale = new Vector3 (19.5f, 19.5f, 19.5f);
			int rng = Random.Range (1, 360);

			parentObject.transform.Rotate(new Vector3(0,0,rng));
		
			gameObject = Instantiate(item.gameobject, new Vector3(0,0,0), Quaternion.identity) as GameObject;
			gameObject.transform.localScale = item.gameobject.transform.localScale;
			gameObject.transform.SetParent(parentObject.transform,false);
			gameObject.AddComponent<ItemDrop> ();
			MeshRenderer [] renderer=gameObject.GetComponentsInChildren<MeshRenderer> ();
			foreach (MeshRenderer mr in renderer) {
				Texture tmp = mr.material.mainTexture;
				mr.material = GameObject.Find("RessourceScript").GetComponent<MaterialScript> ().itemDropShader;
				mr.material.mainTexture = tmp;
			}
			//gameObject.GetComponent<BoxCollider> ().enabled = true;
			ItemDrop id = gameObject.GetComponent<ItemDrop> ();
			id.item = item;
			id.x = x;
			id.z = z;
		}
		public int GetX()
		{
			return x;
		}
		public int GetZ()
		{
			return z;
		}
		public static bool  active=false;
		void Hovered(int x, int z){
			//Debug.Log ("Hovered: " + x + " "+z);
			if (x == this.x && z == this.z) {
				if (!active) {
					if (item is Weapon) {
						if (((Weapon)item).Description != "") {
							MainScript.weaponDescriptionLong.SetActive (true);
							GameObject.Find ("ItemShortText").GetComponent<Text> ().text = "" + ((Weapon)item).Description;
						} else {
							MainScript.weaponDescriptionShort.SetActive (true);
						}
						GameObject.Find ("ItemDamage").GetComponent<Text> ().text = "" + ((Weapon)item).dmg;
						GameObject.Find ("ItemHit").GetComponent<Text> ().text = "" + ((Weapon)item).hit;
						GameObject.Find ("WIcon").GetComponent<Image> ().sprite =  ((Weapon)item).sprite;
					} else {
						MainScript.itemDescription.SetActive (true);
						GameObject.Find ("ItemIcon").GetComponent<Image> ().sprite =  item.sprite;
					}

					GameObject.Find ("ItemName").GetComponent<Text> ().text = item.Name;
					GameObject item3d = Instantiate (item.gameobject3d) as GameObject;
					item3d.tag= "3DItem";
					item3d.transform.localPosition = new Vector3 (0, 0, 0);
					item3d.layer = 19;
					for (int i = 0; i < item3d.transform.childCount; i++) {
						item3d.transform.GetChild (i).gameObject.layer = 19;
						for (int j = 0; j < item3d.transform.GetChild (i).childCount; j++) {
							item3d.transform.GetChild (i).GetChild (j).gameObject.layer = 19;
						}

					}
					item3d.transform.SetParent (GameObject.Find ("3DItemPosition").transform, false);
				}
				active = true;
			} 
		}
		float hoverTime=0;
		void Update(){
			
		}
		void OnMouseEnter(){
		}

		void OnMouseOver(){
			selected = true;

		}
		void OnMouseExit(){
			selected = false;
		}
		public void Take(Character c){
			c.addItem (item);
			MainScript.fieldHoveredEvent -= Hovered;
			MainScript.GetInstance ().DeleteItemDrop (this);
			GameObject.Destroy (this.gameObject);

			//GameObject.Destroy (this);
		}
	}
}

