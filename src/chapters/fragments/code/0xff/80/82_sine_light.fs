// 82 - "Sine Light"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define AM_C 0.3
#define LM_C 1.0

float dirLight(vec3 p, vec3 lightPos){
  return 0.;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*0.;

  vec3 mv = vec3(sin(t),0, cos(t));
  // vec3 mv;
  vec3 pointLight = vec3(1. ,mv.z*0.,25.);

  float ambient = (sin(length(p)*PI*5.+ t)+1.)/2.;
                  // (cos(p.y*PI*5.+ 1.)+1.)/2.;

  float wavePos = ambient ;
  float z = wavePos;
  vec3 point = vec3(p.x, p.y, z*24.);

  vec3 pointToLight = pointLight - point;
  float distToLight = length(pointToLight);
  vec3 pointLightDir = normalize(pointToLight);

  float distance = distToLight;
  distance = distance * distance; 

  float finalAmbient = (ambient * AM_C);

  vec3 tangent = normalize(vec3(1,0,cos(wavePos)));
  vec3 normal = vec3(-tangent.z,0.,tangent.x);//flip
  float NdotL = max(dot(normal, normalize(pointLightDir)), 0.);
  float finalLambert = NdotL * (LM_C/distance);// * (distToLight* distToLight);

  i = finalLambert +
  	  finalAmbient;

  gl_FragColor = vec4(vec3(i),1.);
}