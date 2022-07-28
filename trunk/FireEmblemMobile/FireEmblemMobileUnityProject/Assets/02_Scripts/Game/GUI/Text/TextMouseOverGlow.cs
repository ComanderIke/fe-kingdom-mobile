using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GUI.Text
{
    public class TextMouseOverGlow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // References
        public TextMeshProUGUI text;
        private static Material m_TextBaseMaterial;
        private static Material m_TextHighlightMaterial;

        private void Awake()
        {
            // Get a reference to the default base material
            m_TextBaseMaterial = text.fontSharedMaterial;

            // Create new instance of the material assigned to the text object
            // Assumes all text objects will use the same highlight
            m_TextHighlightMaterial = new Material(m_TextBaseMaterial);

            // Set Glow Power on the new material instance
            m_TextHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 1.0f);
        }
        int cnt = 0;
        void Update()
        {
            float glow = Mathf.Sin(Time.time);
            glow = (glow + 1) / 2;
            m_TextHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, glow);
            cnt++;
            if(cnt == 1)
            {
                text.fontMaterial = m_TextHighlightMaterial;
                text.UpdateMeshPadding();
                cnt = 0;
            }
            //if (cnt == 1)
            //{
            //    m_TextHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, glow);
           
            //    //text.fontSharedMaterial = m_TextBaseMaterial;
            //    text.UpdateMeshPadding();
            
            //}
        }
        public void OnMouseDown()
        {
            Debug.Log("CLICKED UI ELEMENT!");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Assign the modified highlight material
            text.fontSharedMaterial = m_TextHighlightMaterial;
            text.UpdateMeshPadding();
            Debug.Log("OnPointerDown() Material ID: " + text.fontSharedMaterial.GetInstanceID());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == gameObject)
            {
                // Re-assign the default material
                text.fontSharedMaterial = m_TextBaseMaterial;
                text.UpdateMeshPadding();
                Debug.Log("OnPointerUp() Material ID: " + text.fontSharedMaterial.GetInstanceID());
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Assign the modified highlight material
            text.fontSharedMaterial = m_TextHighlightMaterial;
            text.UpdateMeshPadding();
            Debug.Log("OnPointerEnter() Material ID: " + text.fontSharedMaterial.GetInstanceID());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Re-assign the default material
            text.fontSharedMaterial = m_TextBaseMaterial;
            text.UpdateMeshPadding();
            Debug.Log("OnPointerExit() Material ID: " + text.fontSharedMaterial.GetInstanceID());
        }
    }
}
