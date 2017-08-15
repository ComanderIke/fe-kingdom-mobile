using UnityEngine;
using System.Collections;

public class AutoTransparent : MonoBehaviour
{
    private Shader m_OldShader = null;
    private Color m_OldColor = Color.black;
    private float m_Transparency = 0.3f;
    private const float m_TargetTransparancy = 0.3f;
    private const float m_FallOff = 0.1f; // returns to 100% in 0.1 sec
    private Renderer renderer;
    public void Start()
    {
        
    }
    public void BeTransparent()
    {
       
        // reset the transparency;
        m_Transparency = m_TargetTransparancy;
        if (m_OldShader == null)
        {
            if (renderer == null)
            {
                renderer = GetComponent<Renderer>();
            }
            // Save the current shader
            m_OldShader = renderer.material.shader;
            m_OldColor = renderer.material.color;
            renderer.material.shader = Shader.Find("Transparent/Diffuse");
        }
    }
    void Update()
    {
        if(renderer== null)
        {
            renderer = GetComponent<Renderer>();
        }
        if (m_Transparency < 1.0f)
        {
            Color C = renderer.material.color;
            C.a = m_Transparency;
            renderer.material.color = C;
        }
        else
        {
            // Reset the shader
            renderer.material.shader = m_OldShader;
            renderer.material.color = m_OldColor;
            // And remove this script
            Destroy(this);
        }
        m_Transparency += ((1.0f - m_TargetTransparancy) * Time.deltaTime) / m_FallOff;
    }
}

