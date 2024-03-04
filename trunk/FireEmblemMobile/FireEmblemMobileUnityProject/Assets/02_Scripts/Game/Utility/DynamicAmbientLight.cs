using System.Collections.Generic;
using Game.Graphics.Environment;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Utility
{
    public class DynamicAmbientLight : MonoBehaviour
    {
      
        public Gradient sunset;
        public Gradient sunrise;
        public Gradient sunset_shafts;
        public Gradient sunrise_shafts;

        public Gradient sunset_particles;
        public Gradient sunrise_particles;
        public Gradient sunset_particles_small;
        public Gradient sunrise_particles_small;
        public Gradient sunset_particles_big;
        public Gradient sunrise_particles_big;

        public List<Material> particlesBig;
        public List<Material> particlesSmall;
        public List<Material> particlesNormal;
        
       // public Gradient sunrise;
        public UnityEngine.Rendering.Universal.Light2D lightSource;
        public List<UnityEngine.Rendering.Universal.Light2D> sunShafts;
        public List<UnityEngine.Rendering.Universal.Light2D> sunSpots;
        public float fadeDuration = 1;
     
        private float lightBaseIntensity;

        private float sunShaftBaseIntensity;
        private float sunSpotsBaseIntensity;
        // Start is called before the first frame update
        void Awake()
        {
            lightBaseIntensity = lightSource.intensity;
            
            particlesBig.Clear();
            particlesSmall.Clear();
            particlesNormal.Clear();
            var normalParticles = GameObject.FindGameObjectsWithTag("EnvironmentParticle");
            foreach(var child in normalParticles)
                particlesNormal.Add(child.GetComponent<ParticleSystemRenderer>().material);
            var smallParticles = GameObject.FindGameObjectsWithTag("EnvironmentParticleSmall");
            foreach(var child in smallParticles)
                particlesSmall.Add(child.GetComponent<ParticleSystemRenderer>().material);
            var bigParticles = GameObject.FindGameObjectsWithTag("EnvironmentParticleBig");
            foreach(var child in bigParticles)
                particlesBig.Add(child.GetComponent<ParticleSystemRenderer>().material);
            sunShafts.Clear();
            var sunShaftParent = GameObject.FindWithTag("Sunshafts");
            sunShafts.Add(sunShaftParent.GetComponent<Light2D>());
            foreach(var child in sunShaftParent.GetComponentsInChildren<Light2D>())
                sunShafts.Add(child);
            sunShaftBaseIntensity = sunShafts[0].intensity;
            if(sunSpots.Count>0)
                sunSpotsBaseIntensity = sunSpots[0].intensity;
            //UpdateHour(0);

        }
         float time = 0;
         private Color lerpFromColor;
         private Color lerpTargetColor;
         private Color lerpFromColor_shafts;
         private Color lerpTargetColor_shafts;
         
         private Color lerpFromColor_particles;
         private Color lerpTargetColor_particles;
         private Color lerpFromColor_particles_big;
         private Color lerpTargetColor_particles_big;
         private Color lerpFromColor_particles_small;
         private Color lerpTargetColor_particles_small;

         private float lerpTargetIntensity_sunShaft;
         private float lerpFromIntensity_sunShaft;
         private float lerpTargetIntensity_sunSpot;
         private float lerpFromIntensity_sunSpot;

         public Color currentParticleColor;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                IncreaseHour();
            }

            if (lerp)
            {
                time += Time.deltaTime / fadeDuration;
                lightSource.color = Color.Lerp(lerpFromColor, lerpTargetColor, time);
                foreach (var sunShaft in sunShafts)
                {
                    sunShaft.color = Color.Lerp(lerpFromColor_shafts, lerpTargetColor_shafts, time);
                    sunShaft.intensity =
                        Mathf.SmoothStep(lerpFromIntensity_sunShaft, lerpTargetIntensity_sunShaft, time);
                }
                foreach(var mat in particlesNormal)
                    mat.SetColor(TintColor,Color.Lerp(lerpFromColor_particles, lerpTargetColor_particles, time));
                //currentParticleColor = particlesNormal.GetColor("_TintColor");
                foreach(var mat in particlesBig)
                    mat.SetColor(TintColor,Color.Lerp(lerpFromColor_particles_big, lerpTargetColor_particles_big, time));
                foreach(var mat in particlesSmall)
                    mat.SetColor(TintColor,Color.Lerp(lerpFromColor_particles_small, lerpTargetColor_particles_small, time));
                foreach (var sunSpot in sunSpots)
                {
                    sunSpot.color = Color.Lerp(lerpFromColor_shafts, lerpTargetColor_shafts, time);
                    sunSpot.intensity = Mathf.SmoothStep(lerpFromIntensity_sunSpot, lerpTargetIntensity_sunSpot, time);
                }

                if (time > 1)
                {
                    lerp = false;
                    time = 0;
                }
                
                // if(time<=0.5f)
                //     lightSource.intensity = Mathf.SmoothStep(0.4f, lightBaseIntensity, time*2);
                // else
                // {
                //     lightSource.intensity = Mathf.SmoothStep(lightBaseIntensity, 0.4f, (time-0.5f) * 2);
                // }
               
            }

        }

        public float hour=0;
        private bool lerp = false;
        private static readonly int TintColor = Shader.PropertyToID("_TintColor");

        public void UpdateHour(float hour)
        {
            this.hour = hour;
            lerp = true;
            UpdateGoalValues();
            
        }

        void UpdateGoalValues()
        {
            lerpFromColor = lightSource.color;
            lerpFromColor_shafts = sunShafts[0].color;
            if(particlesSmall.Count>1)
                lerpFromColor_particles_small = particlesSmall[0].GetColor(TintColor);
            if(particlesBig.Count>1)
                lerpFromColor_particles_big = particlesBig[0].GetColor(TintColor);
            if(particlesNormal.Count>1)
                lerpFromColor_particles = particlesNormal[0].GetColor(TintColor);
            if (hour >= 12)
            {
                lerpTargetColor = sunset.Evaluate((hour - 12) / 12f);
                lerpTargetColor_shafts= sunset_shafts.Evaluate((hour - 12) / 12f);
                lerpTargetColor_particles = sunset_particles.Evaluate((hour - 12) / 12f);
                lerpTargetColor_particles_small = sunset_particles_small.Evaluate((hour - 12) / 12f);
                lerpTargetColor_particles_big= sunset_particles_big.Evaluate((hour - 12) / 12f);
            }
            else
            {
                lerpTargetColor= sunrise.Evaluate(hour/12f);
                lerpTargetColor_shafts= sunrise_shafts.Evaluate(hour/12f);
                lerpTargetColor_particles = sunrise_particles.Evaluate(hour/ 12f);
                lerpTargetColor_particles_small = sunrise_particles_small.Evaluate(hour / 12f);
                lerpTargetColor_particles_big= sunrise_particles_big.Evaluate(hour / 12f);
            }

            lerpFromIntensity_sunShaft = sunShafts[0].intensity;
            lerpTargetIntensity_sunShaft = 0 + sunShaftBaseIntensity;
            if (sunSpots.Count <= 0)
                return;
            lerpFromIntensity_sunSpot = sunSpots[0].intensity;
            lerpTargetIntensity_sunSpot = 0 + sunSpotsBaseIntensity;
        }
          public void UpdateHourFixed(float hour)
          {
              this.hour = hour;
              UpdateGoalValues();
              lightSource.color = lerpTargetColor;
                foreach (var sunShaft in sunShafts)
                {
                    sunShaft.color = lerpTargetColor_shafts;
                    sunShaft.intensity = lerpTargetIntensity_sunShaft;
                }
                foreach(var mat in particlesNormal)
                    mat.SetColor(TintColor,lerpTargetColor_particles);
                foreach(var mat in particlesBig)
                    mat.SetColor(TintColor,lerpTargetColor_particles_big);
                foreach(var mat in particlesSmall)
                    mat.SetColor(TintColor,lerpTargetColor_particles_small);
                foreach (var sunSpot in sunSpots)
                {
                    sunSpot.color = lerpTargetColor_shafts;
                    sunSpot.intensity = lerpTargetIntensity_sunSpot;
                }
        }
        private void IncreaseHour()
        {
           
            hour+=6;
            if (hour >= 24)
                hour = 0;
            Debug.Log("Increase Hour: "+hour);
            UpdateHour(hour);
            // if(hour>=6&&hour <=18f)
            //     lightSource.intensity = Mathf.SmoothStep(0.4f, lightBaseIntensity, (hour/24f)*2);
            // else
            // {
            //     lightSource.intensity = Mathf.SmoothStep(lightBaseIntensity, 0.4f, ((hour-12/24) * 2));
            // }
           
        }
        public void ClearSunSpots()
        {
            sunSpots.Clear();
        }
        public void AddSunSpot(Light2D sunSpot)
        {
            sunSpots.Add(sunSpot);
            sunSpotsBaseIntensity = sunSpot.intensity;
            UpdateHour(hour);
        }
    }
}
